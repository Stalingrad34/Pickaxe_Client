using UnityEngine;

namespace Game.Scripts.Multiplayer
{
  public static class Extensions
  {
    public static Vector3 ToVector3(this Vector2Float vector)
    {
      return new Vector3(vector.x, 0, vector.z);
    }
    
    public static Vector3 ToVector3(this Vector3Float vector)
    {
      return new Vector3(vector.x, vector.y, vector.z);
    }
  }
}