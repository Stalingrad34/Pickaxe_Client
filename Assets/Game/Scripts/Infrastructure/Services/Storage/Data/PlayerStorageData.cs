using System;

namespace Game.Scripts.Infrastructure.Services.Storage.Data
{
  [Serializable]
  public class PlayerStorageData
  {
    public string PlayerName;
    public string Language = "ru";
  }
}