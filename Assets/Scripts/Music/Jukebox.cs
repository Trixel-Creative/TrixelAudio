using System;
using TrixelCreative.TrixelAudio.Data;
using UnityEngine;

namespace TrixelCreative.TrixelAudio.Music
{
	public class Jukebox : JukeboxBase
	{
		[SerializeField]
		private SongAsset? song;

		[SerializeField]
		private bool playOnAwake = true;

		[SerializeField]
		private bool loop = false;

		private void Start()
		{
			if (playOnAwake)
				Play();
		}

		public void Play()
		{
			if (song != null)
			{
				this.AudioSource.PlaySong(this.song, this.loop);
			}
		}
	}
}