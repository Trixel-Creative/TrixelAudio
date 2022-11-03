#nullable enable

using System;
using TrixelCreative.TrixelAudio.Data;
using TrixelCreative.TrixelAudio.Utility;
using UnityEngine;

namespace TrixelCreative.TrixelAudio.Music
{
	[RequireComponent(typeof(TrixelAudioSource))]
	public abstract class JukeboxBase : MonoBehaviour, IJukebox
	{
		private TrixelAudioSource audioSource = null!;

		[SerializeField]
		private SongAsset? song;

		[SerializeField]
		private bool loop = false;

		protected TrixelAudioSource AudioSource => audioSource;

		private void Awake()
		{
			this.MustGetComponent(out audioSource);

			OnAwake();
		}

		protected virtual void OnAwake() { }
		
		public void Play()
		{
			if (song != null)
			{
				this.AudioSource.PlaySong(this.song, this.loop);
			}
		}
	}
}