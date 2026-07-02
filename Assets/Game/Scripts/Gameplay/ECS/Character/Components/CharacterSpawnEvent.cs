using Game.Scripts.Gameplay.Units;

namespace Game.Scripts.Gameplay.ECS.Spawn.Components
{
  public struct CharacterSpawnEvent
  {
    public UnitData Data;
    public string PrefabPath;
  }
}