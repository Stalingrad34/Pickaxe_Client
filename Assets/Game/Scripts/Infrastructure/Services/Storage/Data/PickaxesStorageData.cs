using System;
using System.Collections.Generic;
using Game.Scripts.Gameplay.Pickaxe;

namespace Game.Scripts.Infrastructure.Services.Storage.Data
{
  [Serializable]
  public class PickaxesStorageData
  {
    public ulong PickaxesNominal;
    public Dictionary<PickaxeType, int> Pickaxes = new();
  }
}