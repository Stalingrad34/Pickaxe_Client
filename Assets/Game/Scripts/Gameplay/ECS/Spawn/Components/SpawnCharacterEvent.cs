using Game.Scripts.Gameplay.Units;

namespace Game.Scripts.Gameplay.ECS.Spawn.Components
{
  public struct SpawnCharacterEvent
  {
    public UnitData Data;
    public string PrefabPath;
  }
}