using Game.Scripts.Infrastructure.Custom;
using UnityEngine;

namespace Game.Scripts.UI.GUI
{
  public class PickaxeVariantView : MonoBehaviour
  {
    [SerializeField] private CustomText pickaxeName;
    [SerializeField] private CustomText pickaxeTier;
    [SerializeField] private CustomText percent;
  }
}