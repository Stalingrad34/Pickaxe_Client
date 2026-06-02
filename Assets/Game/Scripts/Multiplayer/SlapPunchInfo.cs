using System;

namespace Game.Scripts.Multiplayer
{
  [Serializable]
  public struct SlapPunchInfo
  {
    public string PlayerId;
    public Vector3Float Force;
    public string Weapon;
  }
}