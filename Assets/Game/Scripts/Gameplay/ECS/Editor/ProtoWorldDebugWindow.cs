#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Game.Scripts.Gameplay.ECS;
using Leopotam.EcsProto;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Editor
{
  public sealed class ProtoWorldDebugWindow : EditorWindow
  {
    private readonly List<EntityInfo> _entities = new();
    private readonly List<IProtoPool> _selectedPools = new();
    private readonly Dictionary<Type, bool> _foldouts = new();

    private ECSRunner _runner;
    private ProtoWorld _world;
    private Vector2 _entitiesScroll;
    private Vector2 _detailsScroll;
    private string _entitySearch = string.Empty;
    private int _selectedEntityId = -1;
    private bool _autoRefresh = true;
    private double _nextRefreshTime;

    [MenuItem("Window/ECS/LeoECS Proto World Debugger")]
    private static void Open()
    {
      GetWindow<ProtoWorldDebugWindow>("Proto World").Show();
    }

    private void OnEnable()
    {
      EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
      Refresh();
    }

    private void OnDisable()
    {
      EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void Update()
    {
      if (!_autoRefresh || EditorApplication.timeSinceStartup < _nextRefreshTime)
        return;

      _nextRefreshTime = EditorApplication.timeSinceStartup + 0.35f;
      Refresh();
      Repaint();
    }

    private void OnGUI()
    {
      DrawToolbar();

      if (_world == null)
      {
        EditorGUILayout.HelpBox("Enter Play Mode and make sure an ECSRunner exists in the scene.", MessageType.Info);
        return;
      }

      DrawSummary();

      using (new EditorGUILayout.HorizontalScope())
      {
        DrawEntitiesPanel();
        DrawDetailsPanel();
      }
    }

    private void DrawToolbar()
    {
      using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
      {
        _autoRefresh = GUILayout.Toggle(_autoRefresh, "Auto", EditorStyles.toolbarButton, GUILayout.Width(48));

        if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(70)))
          Refresh();

        GUILayout.Space(8);
        GUILayout.Label(_runner != null ? _runner.name : "No ECSRunner", EditorStyles.miniLabel);
        GUILayout.FlexibleSpace();
      }
    }

    private void DrawSummary()
    {
      var pools = _world.Pools();
      var recycled = _world.RecycledEntities();
      var alive = 0;

      for (var i = 0; i < _world.EntityGens().Len(); i++)
      {
        if (_world.EntityGens().Get(i) > 0)
          alive++;
      }

      using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
      {
        GUILayout.Label($"Entities: {alive}", EditorStyles.boldLabel);
        GUILayout.Label($"Pools: {pools.Len()}");
        GUILayout.Label($"Recycled: {recycled.Len()}");
        GUILayout.Label($"Mask words: {_world.EntityMaskItemLen()}");
      }
    }

    private void DrawEntitiesPanel()
    {
      using (new EditorGUILayout.VerticalScope(GUILayout.Width(position.width * 0.38f)))
      {
        EditorGUILayout.LabelField("Entities", EditorStyles.boldLabel);
        _entitySearch = EditorGUILayout.TextField("Search", _entitySearch);

        _entitiesScroll = EditorGUILayout.BeginScrollView(_entitiesScroll);
        foreach (var entity in _entities)
        {
          if (!MatchesSearch(entity))
            continue;

          var selected = entity.Id == _selectedEntityId;
          var label = $"#{entity.Id}  components: {entity.ComponentsCount}";

          if (GUILayout.Toggle(selected, label, "Button") != selected)
          {
            _selectedEntityId = selected ? -1 : entity.Id;
            RefreshSelectedPools();
          }
        }
        EditorGUILayout.EndScrollView();
      }
    }

    private void DrawDetailsPanel()
    {
      using (new EditorGUILayout.VerticalScope())
      {
        EditorGUILayout.LabelField(_selectedEntityId >= 0 ? $"Entity #{_selectedEntityId}" : "Entity Details", EditorStyles.boldLabel);

        _detailsScroll = EditorGUILayout.BeginScrollView(_detailsScroll);
        if (_selectedEntityId < 0)
        {
          EditorGUILayout.HelpBox("Select an entity to inspect its components.", MessageType.None);
        }
        else if (_selectedPools.Count == 0)
        {
          EditorGUILayout.HelpBox("Selected entity has no components or is no longer alive.", MessageType.Warning);
        }
        else
        {
          var entity = MakeEntity(_selectedEntityId);
          foreach (var pool in _selectedPools)
            DrawComponent(pool, entity);
        }

        EditorGUILayout.EndScrollView();
      }
    }

    private void DrawComponent(IProtoPool pool, ProtoEntity entity)
    {
      var type = pool.ItemType();
      var opened = _foldouts.TryGetValue(type, out var state) && state;
      opened = EditorGUILayout.Foldout(opened, CleanTypeName(type), true);
      _foldouts[type] = opened;

      if (!opened)
        return;

      using (new EditorGUI.IndentLevelScope())
      {
        object component;
        try
        {
          component = pool.Raw(entity);
        }
        catch (Exception e)
        {
          EditorGUILayout.HelpBox(e.Message, MessageType.Warning);
          return;
        }

        if (component == null)
        {
          EditorGUILayout.LabelField("<null>");
          return;
        }

        DrawObjectFields(component, type);
      }
    }

    private static void DrawObjectFields(object component, Type type)
    {
      var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

      foreach (var field in type.GetFields(flags))
      {
        if (field.IsStatic)
          continue;

        object value;
        try
        {
          value = field.GetValue(component);
        }
        catch
        {
          value = "<unreadable>";
        }

        EditorGUILayout.LabelField(field.Name, FormatValue(value));
      }

      foreach (var property in type.GetProperties(flags))
      {
        if (!property.CanRead || property.GetIndexParameters().Length > 0)
          continue;

        object value;
        try
        {
          value = property.GetValue(component);
        }
        catch
        {
          continue;
        }

        EditorGUILayout.LabelField(property.Name, FormatValue(value));
      }
    }

    private void Refresh()
    {
      _runner = FindRunner();
      _world = _runner != null ? _runner.World : null;

      _entities.Clear();
      _selectedPools.Clear();

      if (_world == null || !_world.IsAlive())
        return;

      var gens = _world.EntityGens();
      for (var i = 0; i < gens.Len(); i++)
      {
        if (gens.Get(i) <= 0)
          continue;

        var entity = MakeEntity(i);
        _entities.Add(new EntityInfo
        {
          Id = i,
          ComponentsCount = _world.ComponentsCount(entity)
        });
      }

      RefreshSelectedPools();
    }

    private void RefreshSelectedPools()
    {
      _selectedPools.Clear();
      if (_world == null || _selectedEntityId < 0)
        return;

      var gens = _world.EntityGens();
      if (_selectedEntityId >= gens.Len() || gens.Get(_selectedEntityId) <= 0)
        return;

      var entity = MakeEntity(_selectedEntityId);
      var pools = _world.Pools();

      for (var i = 0; i < pools.Len(); i++)
      {
        var pool = pools.Get(i);
        if (pool != null && pool.Has(entity))
          _selectedPools.Add(pool);
      }
    }

    private static ECSRunner FindRunner()
    {
#if UNITY_2023_1_OR_NEWER
      return FindFirstObjectByType<ECSRunner>();
#else
      return FindObjectOfType<ECSRunner>();
#endif
    }

    private static ProtoEntity MakeEntity(int id)
    {
#if LEOECSPROTO_SMALL_WORLD
      return (ProtoEntity)(ushort)id;
#else
      return (ProtoEntity)id;
#endif
    }

    private bool MatchesSearch(EntityInfo entity)
    {
      if (string.IsNullOrWhiteSpace(_entitySearch))
        return true;

      return entity.Id.ToString().IndexOf(_entitySearch, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private static string CleanTypeName(Type type)
    {
      var name = type.Name;
      var tick = name.IndexOf('`');
      return tick >= 0 ? name.Substring(0, tick) : name;
    }

    private static string FormatValue(object value)
    {
      return value switch
      {
        null => "<null>",
        string text => text,
        UnityEngine.Object unityObject => unityObject ? unityObject.name : "<missing object>",
        _ => value.ToString()
      };
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
      if (state == PlayModeStateChange.EnteredPlayMode || state == PlayModeStateChange.ExitingPlayMode)
        Refresh();
    }

    private struct EntityInfo
    {
      public int Id;
      public int ComponentsCount;
    }
  }
}
#endif
