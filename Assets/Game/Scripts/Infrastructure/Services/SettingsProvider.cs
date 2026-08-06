using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services
{
  public class SettingsProvider : IService
  {
    public readonly ReactiveProperty<bool> SoundDisabled = new();

    public void ChangeSoundDisabled()
    {
      SoundDisabled.Value = !SoundDisabled.Value;
    }
  }
}