using System;
using System.Collections.Generic;
using Game.Scripts.Tutorial;

namespace Game.Scripts.Infrastructure.Services.Storage.Data
{
  [Serializable]
  public class TutorialsStorageData
  {
    public TutorialType CurrentTutorialType;
    public Dictionary<TutorialType, TutorialData> Data = new();
  }
}