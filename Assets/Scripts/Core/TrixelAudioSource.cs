using System;
using TrixelCreative.TrixelAudio.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace TrixelCreative.TrixelAudio
{
	public class TrixelAudioSource : MonoBehaviour
	{
		private TrixelAudioCore core = null!;

		public TrixelAudioCore AudioCore => core;
		
		private void Awake()
		{
			core = FindObjectOfType<TrixelAudioCore>();
			Assert.IsNotNull(core, "[TrixelAudio] Cannot find TrixelAudioCore, this audio source will not function.");
		}

		public void Play(SoundEffectAsset sound)
		{
			core.Play(sound, this.transform);
		}

		public SongPlayerState PlaySong(SongAsset song, bool loop = false)
		{
			return core.PlaySongAsset(song, loop);
		}
	}
}