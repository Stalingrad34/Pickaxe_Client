using System;
using System.Collections.Generic;
using System.Linq;
using Colyseus;
using Colyseus.Schema;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Multiplayer.Generated;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Multiplayer
{
  public class MultiplayerManager : Manager<MultiplayerManager>, IService
  {
    public event Action<string, Player> OnPlayerConnected;
    public event Action<string, Player> OnPlayerDisconnected;
    public event Action<bool> OnRingActivated;
    public event Action<float> OnHeadPosition;
    public event Action<string> OnHeadType;
    public readonly ReactiveCollection<ChatMessage> ChatMessages = new();
    public readonly Dictionary<string, Color> MessageColors = new();
    
    private Room<State> _room;
    private StateCallbackStrategy<State> _stateCallbackStrategy;
    private readonly List<Action> _callbacks = new ();
    private readonly Dictionary<string, PlayerChangesHandler> _changesHandlers = new();

    public void Init(ConnectConfig config)
    {
      _colyseusSettings.colyseusServerAddress = config.Server.IP;
      _colyseusSettings.colyseusServerPort = config.Server.Port;
      _colyseusSettings.useSecureProtocol = config.Server.UseSecureProtocol;
      
      Instance.InitializeClient();

#if UNITY_EDITOR
      ApplicationLifecycleProvider.ApplicationQuit += () => Disconnect().Forget();
#endif
      
      DontDestroyOnLoad(this);
    }

    public async UniTask<Room<State>> Connect(PlayerService player, string roomName, Vector3Float position, float rotation)
    {
      var data = new Dictionary<string, object>()
      {
        {"position", position},
        {"rotation", rotation},
        {"speed", 1},
        /*{"weapon", database.Weapon.Value},
        {"body", database.Body.Value},
        {"face", database.Face.Value},*/
        {"name", player.PlayerName.Value},
      };
      _room = await Instance.client.JoinOrCreate<State>(roomName, data).AsUniTask();

      _stateCallbackStrategy = Callbacks.Get(_room);
      _callbacks.Add(_stateCallbackStrategy.OnAdd(s => s.players, OnPlayerAdd));
      _callbacks.Add(_stateCallbackStrategy.OnRemove(s => s.players, OnPlayerRemove));
      _callbacks.Add(_stateCallbackStrategy.OnAdd(s => s.chat, OnMessageAdd));
      _callbacks.Add(_stateCallbackStrategy.OnRemove(s => s.chat, OnMessageRemove));
      _callbacks.Add(_stateCallbackStrategy.Listen(s => s.isRingActivated, OnRingActivatedChanged));
      _callbacks.Add(_stateCallbackStrategy.Listen(s => s.headPosProgress, OnHeadPositionChanged));
      _callbacks.Add(_stateCallbackStrategy.Listen(s => s.headType, OnHeadTypeChanged));
      
      return _room;
    }

    public async UniTask Disconnect()
    {
      if (_room == null)
        return;
      
      _callbacks.ForEach(c => c());
      _callbacks.Clear();
      ChatMessages?.Clear();
      
      foreach (var handler in _changesHandlers.Values)
      {
        handler.Dispose();
      }
      _changesHandlers.Clear();
      
      try
      {
        await _room.Leave().AsUniTask();
        _room = null;
      }
      catch (Exception e)
      {
        Debug.LogWarning(e);
      }
    }

    public bool IsPlayer(string key)
    {
      return key.Equals(_room?.SessionId);
    }

    private void OnPlayerAdd(string key, Player player)
    {
      var handler = new PlayerChangesHandler(key, player, _stateCallbackStrategy);
      _changesHandlers.Add(key, handler);
      OnPlayerConnected?.Invoke(key, player);
    }
    
    private void OnPlayerRemove(string key, Player player)
    {
      _changesHandlers.Remove(key);
      OnPlayerDisconnected?.Invoke(key, player);
    }

    private void OnMessageAdd(int index, ChatMessage message)
    {
      ChatMessages.Add(message);
    }

    private Color GetColor(ChatMessage message)
    {
      if (MessageColors.TryGetValue(message.name, out var color))
        return color;

      color = new Color(Random.Range(100, 255), Random.Range(100, 255), Random.Range(100, 255));
      if (Random.value > 0.66f)
        color.r = 0;
      if (Random.value > 0.33f)
        color.g = 0;
      else
        color.b = 0;
      
      MessageColors.Add(message.name, color);
      return color;
    }

    private void OnMessageRemove(int index, ChatMessage message)
    {
      var data = ChatMessages.FirstOrDefault(d => d == message);
      if (data != null) 
        ChatMessages.Remove(data);
    }
    
    private void OnRingActivatedChanged(bool value, bool _)
    {
      OnRingActivated?.Invoke(value);
    }
    
    private void OnHeadPositionChanged(float value, float _)
    {
      OnHeadPosition?.Invoke(value);
    }
    
    private void OnHeadTypeChanged(string value, string _)
    {
      OnHeadType?.Invoke(value);
    }
    
    public void SendChatMessage(string message)
    {
      Send("chat", new Dictionary<string, object>(){{"message", message}});
    }

    public void Send(string key, Dictionary<string, object> data)
    {
      data["playerId"] = _room?.SessionId;
      _room?.Send(key, data);
    }
    
    public void Send(string key)
    {
      _room?.Send(key, _room?.SessionId);
    }
    
    public void Send(string key, string json)
    {
      _room?.Send(key, json);
    }
  }
}
