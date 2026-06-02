using System;
using Game.Scripts.Infrastructure.Custom;
using UnityEngine;

namespace Game.Scripts.UI
{
  public class LocalizableText : MonoBehaviour
  {
    [SerializeField] private CustomText text;
    [SerializeField] private string key;

    private void Awake()
    {
      text.SetText(key);
    }
  }
}