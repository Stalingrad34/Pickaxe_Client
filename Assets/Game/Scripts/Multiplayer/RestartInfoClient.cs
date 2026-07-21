using System;
using Game.Scripts.Multiplayer.Generated;

namespace Game.Scripts.Multiplayer
{
  [Serializable]
  public struct RestartInfoClient
  {
    public string id;
    public Player player;
  }
}