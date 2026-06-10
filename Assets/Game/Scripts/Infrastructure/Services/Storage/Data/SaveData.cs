using System;

namespace Game.Scripts.Infrastructure.Services.Storage.Data
{
  [Serializable]
  public class SaveData
  {
    public PlayerStorageData Player = new();
    public EconomyStorageData Economy = new();
    public PickaxesStorageData Pickaxes = new();
  }
}