﻿using System;
using TrixelCreative.TrixelAudio.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace TrixelCreative.TrixelAudio
{
	public class SongPlayerCore
	{
		private readonly TrixelAudioCore core;
		private readonly CoreConfiguration configuration;
		private readonly AudioSourcePool pool;

		private SongPlayerStateInternal? currentSongState;
		private SongPlayerStateInternal? nextSongState;
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
				currentSongState = nextSongState;
				nextSongState = null;
				PlayInternal(nextSong, nextSongLoops);
				nextSong = null;
			}

			// In case a non-looping song ends without an explicit Stop call.
			pool.ReclaimUnusedAudioSources();
			
			// Update the state of the current song (set its position), after checking
			// to see if the song has ended.
			if (this.currentSource != null)
			{
				if (!this.currentSource.gameObject.activeSelf)
				{
					// Song has ended but Stop() wasn't called, report that the song has stopped
					if (currentSongState != null)
					{
						this.currentSongState.SetPlaybackState(PlaybackState.Stopped);
						this.currentSongState.ClearInvocationLists();
						this.currentSongState = null;
					}
					
					// Give away our pooled reference
					this.currentSong = null;
					this.currentSource = null;
				}
				else
				{
					// Update the song position
					if (this.currentSongState != null)
						this.currentSongState.SetPosition(this.currentSource.time);
				}
			}
		}

		public SongPlayerState Play(SongAsset song, bool loop)
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
				this.nextSongState = new SongPlayerStateInternal(song, this);
				return new SongPlayerState(this.nextSongState);
			}

			this.currentSongState = new SongPlayerStateInternal(song, this);
			PlayInternal(song, loop);
			return new SongPlayerState(this.currentSongState);
		}

		public void Stop()
		{
			if (currentSource != null)
				currentSource.Stop();

			if (currentSongState != null)
			{
				currentSongState.SetPlaybackState(PlaybackState.Stopped);
				currentSongState.ClearInvocationLists();
				currentSongState = null;
			}
			
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
				if (currentSongState != null)
				{
					currentSongState.SetPosition(0);
					currentSongState.SetPlaybackState(PlaybackState.Playing);
				}

				currentSource.loop = loop;
				currentSource.spatialBlend = 0;
				currentSource.outputAudioMixerGroup = this.configuration.MusicMixer;
				song.Setup(currentSource);
			}
		}

		private void NotifyStopRequested(SongPlayerStateInternal internalState)
		{
			if (internalState == currentSongState)
			{
				this.Stop();
				return;
			}

			if (internalState == nextSongState)
			{
				nextSongState = null;
				internalState.SetPlaybackState(PlaybackState.Stopped);
				internalState.ClearInvocationLists();
			}
		}

		internal class SongPlayerStateInternal
		{
			private PlaybackState playbackState;
			private readonly SongPlayerCore core;

			public PlaybackState PlaybackState => playbackState;
			public float Length { get; private set; }
			public float Position { get; private set; } = 0;

			public event Action? Stopped;
			public event Action<PlaybackState>? PlaybackStateChanged;

			internal SongPlayerStateInternal(SongAsset song, SongPlayerCore core)
			{
				this.Length = song.Length;
				this.core = core;
			}

			internal void ClearInvocationLists()
			{
				PlaybackStateChanged = null;
				Stopped = null;
			}

			internal void SetPosition(float position)
			{
				this.Position = position;
			}

			internal void SetPlaybackState(PlaybackState newState)
			{
				if (newState != this.playbackState)
				{
					this.playbackState = newState;
					this.OnPlaybackStateChange();
				}
			}

			private void OnPlaybackStateChange()
			{
				this.PlaybackStateChanged?.Invoke(this.playbackState);

				switch (this.playbackState)
				{
					case TrixelAudio.PlaybackState.Stopped:
						Stopped?.Invoke();
						break;
				}
			}

			internal void RequestStop()
			{
				core.NotifyStopRequested(this);
			}
		}
	}

	public enum PlaybackState
	{
		NotStarted,
		Playing,
		Paused,
		Stopped
	}
}