using System.Collections.Generic;

namespace Game.Scripts.Infrastructure.Extensions
{
  public static class ListExtensions
  {
    public static T Random<T>(this List<T> source)
    {
      var rnd = UnityEngine.Random.Range(0, source.Count);
      return source[rnd];
    }
  }
}