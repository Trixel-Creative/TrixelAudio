using System;
using TrixelCreative.TrixelAudio.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace TrixelCreative.TrixelAudio
{
	public class TrixelAudioSource : MonoBehaviour
	{
		private TrixelAudioCore? core = null!;

		public TrixelAudioCore? AudioCoreOld  => core;
		
		private void Awake()
		{
			core = FindObjectOfType<TrixelAudioCore>();
			if (core == null)
				Debug.LogWarning("[TrixelAudio] TrixelAudioCore was not found in any active scene. Sound will not work.");
		}

		public void Play(SoundEffectAsset sound)
		{
			if (core == null)
				return;
			
			core.Play(sound, this.transform);
		}

		public SongPlayerState PlaySong(SongAsset song, bool loop = false)
		{
			if (core == null)
				return SongPlayerState.Invalid;
			
			return core.PlaySongAsset(song, loop);
		}

		public bool TryAcquireAudioSource(out AudioSource pooledAudioSource)
		{
			pooledAudioSource = null;

			if (this.core == null)
				return false;

			return this.core.TryAcquireAudioSource(out pooledAudioSource);
		}

		public bool TryGetUiSounds(out UserInterfaceSoundSchemeAsset uiSounds)
		{
			uiSounds = default!;
			if (core == null)
				return false;

			uiSounds = core.Configuration.UserInterfaceSoundScheme!;
			return uiSounds != null;
		}
	}
}