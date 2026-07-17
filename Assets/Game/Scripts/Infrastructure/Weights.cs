using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Infrastructure
{
  
  public static class Weights
  {
    public static T GetWeightedRandom<T>(this List<WeightedItem<T>> items)
    {
      var totalWeight = items.Sum(x => x.Weight);
      var randomNumber = Random.value * totalWeight;
        
      foreach (var item in items)
      {
        randomNumber -= item.Weight;
        if (randomNumber <= 0)
          return item.Item;
      }
        
      return items[^1].Item;
    }
  }
  
  public struct WeightedItem<T>
  {
    public readonly T Item;
    public readonly int Weight;

    public WeightedItem(T item, int weight)
    {
      Item = item;
      Weight = weight;
    }
  }
}