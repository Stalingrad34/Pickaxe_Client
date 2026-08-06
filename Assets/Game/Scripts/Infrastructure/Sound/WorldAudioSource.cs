using UnityEngine;

namespace Game.Scripts.Infrastructure.Sound
{
  public class WorldAudioSource : MonoBehaviour
  {
    [SerializeField] private string key;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
      AudioController.Instance.RegisterWorldSource(key, this);
    }

    public void PlaySound(AudioClip clip)
    {
      audioSource.PlayOneShot(clip);
    }
  }
}