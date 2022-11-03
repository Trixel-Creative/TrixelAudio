using System;
using TrixelCreative.TrixelAudio.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace TrixelCreative.TrixelAudio
{
    public class TrixelAudioCore : MonoBehaviour
    {
        private AudioSourcePool soundEffectPool = null!;
        private SongPlayerCore songPlayer = null!;

        [Header("Configuration")]
        [SerializeField]
        private CoreConfiguration configuration = null!;
        
        private void Awake()
        {
            // Assertions
            Assert.IsNotNull(configuration, "[TrixelAudio] Missing core configuration for TrixelAudio Core");

            // Ensure we stay alive across scene loads
            DontDestroyOnLoad(this);

            // Initialize the sound effect pool
            soundEffectPool = new AudioSourcePool(this, configuration.SoundEffectPoolSize, "Sfx");
            
            // Initialize the song player
            this.songPlayer = new SongPlayerCore(this, configuration);
        }

        private void Start()
        {
            // Initialize the sound effect pool
            this.soundEffectPool.Initialize();
        }

        private void Update()
        {
            soundEffectPool.ReclaimUnusedAudioSources();
            this.songPlayer.Update();
        }

        public void Play(SoundEffectAsset sound, Transform soundTransform)
        {
            if (TryAcquireAudioSource(out AudioSource source))
            {
                // Move the audio source to the same world location as the object requesting us to play
                Transform sourceTransform = source.transform;
                sourceTransform.position = soundTransform.position;
                sourceTransform.rotation = soundTransform.rotation;

                // Assign the mixer group
                source.outputAudioMixerGroup = this.configuration.SoundEffectsMixer;
                
                // Play the sound
                sound.PlayOnAudioSource(source);
            }
        }

        private bool TryAcquireAudioSource(out AudioSource pooledSource)
        {
            pooledSource = null!;
            
            AudioSource? attempt = this.soundEffectPool.GetNextAvailableAudioSource();
            if (attempt == null)
            {
                Debug.LogWarning($"[TrixelAudio] Pool's closed due to maximum audio source limit.");
                return false;
            }

            pooledSource = attempt;
            return true;
        }

        public void PlaySongAsset(SongAsset song, bool loop = false)
        {
            this.songPlayer.Play(song, loop);
        }
    }
}
