using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Infrastructure.Services;
using PrimeTween;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Infrastructure
{
    public class AudioController : MonoBehaviour
    {
        public static AudioController Instance;
        
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private List<AudioSource> soundSources;
        [SerializeField] private float volumeDownDuration = 3;
        [SerializeField] private float volumeUpDuration = 3;
        [SerializeField] private float soundPower;
        [SerializeField] private float musicPower;
    
        private Dictionary<string, AudioClip> _soundMap;
        private Dictionary<string, float> _soundTimes = new();
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
            _soundMap = GetSounds();
        }
        
        public void Init()
        {
            musicSource.volume = musicPower;
            ServiceProvider.Get<SettingsProvider>().SoundValue.Subscribe(SoundValueChanged).AddTo(gameObject);
        }

        private void OnMusicOffChanged(bool off)
        {
            musicSource.mute = off;
        }

        private void SoundValueChanged(float value)
        {
            AudioListener.volume = value;
        }

        public void PlayAudioClipFromSoundMap(string sound, bool loop = false, float volumeRatio = 1, float delay = 0)
        {
            if (string.IsNullOrEmpty(sound))
                return;

            if (!_soundMap.ContainsKey(sound))
            {
                Debug.LogError("SoundMap not contains " + sound);
                return;
            }

            if (volumeRatio > 1)
                volumeRatio = 1;
            else if (volumeRatio < 0)
                volumeRatio = 0;

            if (_soundTimes.TryGetValue(sound, out var time) && Time.realtimeSinceStartup - time < 0.1f)
            {
                return;
            }
            _soundTimes[sound] = Time.realtimeSinceStartup;

            var freeSoundSource = soundSources.FirstOrDefault(src => !src.isPlaying);
            if (freeSoundSource == default)
            {
                Debug.LogWarning("All AudioSources is busy for " + sound);
                return;
            }
        
            freeSoundSource.loop = loop;
            freeSoundSource.clip = _soundMap[sound];
            freeSoundSource.volume = soundPower * volumeRatio;
            if (delay > 0)
                freeSoundSource.PlayDelayed(delay);
            else
                freeSoundSource.Play();
        }
        
        public void PlayAudioClipFromSoundMapSingle(string sound, bool loop = false, float volumeRatio = 1, float delay = 0)
        {
            if (string.IsNullOrEmpty(sound))
                return;
            
            var playedSources = soundSources.Where(src => src.isPlaying && src.clip == _soundMap[sound]);
            if (playedSources.Any())
                return;
            
            PlayAudioClipFromSoundMap(sound, loop, volumeRatio, delay);
        }
        
        public void PlayMusicSmoothly(string musicName)
        {
            if (!_soundMap.ContainsKey(musicName))
            {
                Debug.LogError("MusicMap not contains " + musicName);
                return;
            }

            if (musicSource.isPlaying)
            {
                if (musicSource.clip == _soundMap[musicName])
                    return;
            
                SmoothlyVolumeDown(musicSource, volumeDownDuration, callback:() =>
                {
                    musicSource.clip = _soundMap[musicName];
                    musicSource.Play();
                    SmoothlyVolumeUp(musicSource, volumeUpDuration, musicPower);
                });
            }
            else
            {
                musicSource.clip = _soundMap[musicName];
                musicSource.volume = 0;
                musicSource.Play();
                SmoothlyVolumeUp(musicSource, volumeUpDuration,musicPower);
            }
        }
    
        public void StopPlayedSound(string sound, bool smoothly = false)
        {
            var playedSources = soundSources.Where(src => src.isPlaying && src.clip == _soundMap[sound]);
            if (smoothly)
            {
                foreach (var playedSource in playedSources)
                {
                    SmoothlyVolumeDown(playedSource, volumeDownDuration);
                }
            }
            else
            {
                foreach (var playedSource in playedSources)
                {
                    playedSource.Stop();
                }
            }
        }

        public void MusicSmoothlyVolumeDown(float volumeRation) =>
            SmoothlyVolumeDown(musicSource, volumeDownDuration, musicPower * volumeRation);
    
        public void MusicSmoothlyVolumeUp() =>
            SmoothlyVolumeUp(musicSource, volumeUpDuration, musicPower);

        private void SmoothlyVolumeDown(AudioSource source, float duration, float endValue = 0, Action callback = null)
        {
            Tween.StopAll(source);
            Tween.AudioVolume(source, endValue, duration, Ease.Linear)
                .OnComplete(callback);
        }

        private void SmoothlyVolumeUp(AudioSource source, float duration, float endValue = 1)
        {
            Tween.StopAll(source);
            Tween.AudioVolume(source, endValue, duration, Ease.Linear);
        }

        public void PlayMusic(string musicName)
        {
            if (!_soundMap.TryGetValue(musicName, out var music))
            {
                Debug.LogError("MusicMap not contains " + musicName);
                return;
            }

            musicSource.clip = music;
            musicSource.Play();
        }
    
        public void PauseMusic()
        {
            if (musicSource.isPlaying)
                musicSource.Pause();
        }
    
        public void ResumeMusic()
        {
            if (!musicSource.isPlaying)
            {
                musicSource.UnPause();
            }
        }

        public void StopMusic(bool isSmootly = true)
        {
            if (!musicSource.isPlaying)
                return;

            if (isSmootly)
                SmoothlyVolumeDown(musicSource, volumeDownDuration, callback: musicSource.Stop);
            else
                musicSource.Stop();
        }
        
        private Dictionary<string, AudioClip> GetSounds()
        {
            var result = new Dictionary<string, AudioClip>();
            var allSounds = AssetProvider.GetAllSounds();
            foreach (var sound in allSounds)
            {
                result.Add(sound.name, sound);
            }

            return result;
        }
    }
}
