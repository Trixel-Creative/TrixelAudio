using System.Diagnostics;
using TrixelCreative.TrixelAudio.Data;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Assertions;

namespace TrixelCreative.TrixelAudio
{
	public class SongPlayerCore
	{
		private readonly TrixelAudioCore core;
		private readonly CoreConfiguration configuration;
		private readonly AudioSourcePool pool;

		private AudioSource? currentSource;
		private SongAsset? currentSong;
		private SongAsset? nextSong;
		private bool isPaused;
		private bool nextSongLoops;

		public SongPlayerCore(TrixelAudioCore core, CoreConfiguration configuration)
		{
			this.core = core;
			this.configuration = configuration;

			this.pool = new AudioSourcePool(this.core, 6, "Music");
			pool.Initialize();
		}

		public void Update()
		{
			if (isPaused)
				return;
			
			// TODO: Fading
			if (nextSong != null)
			{
				Stop();
				PlayInternal(nextSong, nextSongLoops);
			}

			// In case a non-looping song ends without an explicit Stop call.
			pool.ReclaimUnusedAudioSources();
			
			// If our current audio source has been reclaimed above, we must lose the reference!!
			if (this.currentSource != null && !this.currentSource.gameObject.activeSelf)
			{
				this.currentSong = null;
				this.currentSource = null;
			}
		}

		public void Play(SongAsset song, bool loop)
		{
			// Force-stops the current song if we're paused, this prevents an awkward fade when fading is added.
			if (currentSource != null && isPaused)
			{
				Stop();
			}

			// TODO: Fade times
			if (currentSong != null)
			{
				nextSong = song;
				nextSongLoops = loop;
				return;
			}

			PlayInternal(song, loop);
		}

		public void Stop()
		{
			if (currentSource != null)
				currentSource.Stop();
			
			isPaused = false;
			pool.ReclaimUnusedAudioSources();
			currentSong = null;
		}
		
		private void PlayInternal(SongAsset song, bool loop)
		{
			currentSong = song;
			isPaused = false;
			
			// Claim an audio source
			currentSource = pool.GetNextAvailableAudioSource();
			Assert.IsNotNull(currentSource, "[TrixelAudio] SongPlayer has no available audio sources in its pool, and cannot play the song. This is most definitely a bug and is in need of restitching.");

			if (currentSource != null)
			{
				currentSource.loop = loop;
				currentSource.spatialBlend = 0;
				currentSource.outputAudioMixerGroup = this.configuration.MusicMixer;
				song.Setup(currentSource);
			}
		}
	}
}