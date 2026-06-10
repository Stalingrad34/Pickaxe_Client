using System.Collections.Generic;
using Game.Scripts.Gameplay.Pickaxe;

namespace Game.Scripts.Gameplay.ECS.Pickaxe.Components
{
  public struct RebuildPickaxeEvent
  {
    public string OwnerID;
    public Dictionary<PickaxeType, int> Pickaxes;
  }
}