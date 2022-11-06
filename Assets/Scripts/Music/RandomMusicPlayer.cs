using System;
using TrixelCreative.TrixelAudio.Data;
using TrixelCreative.TrixelAudio.Utility;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace TrixelCreative.TrixelAudio.Music
{
	public class RandomMusicPlayer : MonoBehaviour, IJukebox
	{
		[SerializeField]
		private PlaylistAsset playlist = null!;
		
		private TrixelAudioSource audioSource = null!;
		private SongAsset? currentlyPlayingSong = null;
		private SongPlayerState? currentSongState = null;

		/// <inheritdoc />
		public SongAsset? Song => currentlyPlayingSong;

		/// <inheritdoc />
		public PlaybackState PlaybackState
		{
			get
			{
				if (currentSongState == null)
					return PlaybackState.Stopped;

				return currentSongState.PlaybackState;
			}
		}

		private void Awake()
		{
			Assert.IsNotNull(playlist);
			this.MustGetComponent(out audioSource);
		}

		private void HandleStopped()
		{
			// Remove the Stopped event
			if (currentSongState != null)
			{
				currentSongState.Stopped -= HandleStopped;
			}

			currentSongState = null;
			currentlyPlayingSong = null;
			
			// Play the next song.
			this.PlayNextSong();
		}

		private void PlayNextSong()
		{
			// If we're already playing, don't do anything
			if (currentlyPlayingSong != null)
				return;
			
			// Pick a song to play.
			SongAsset songToPlay = this.playlist.Songs[Random.Range(0, playlist.Songs.Count)];
			
			// Load the song
			this.currentlyPlayingSong = songToPlay;
			this.currentSongState = this.audioSource.PlaySong(this.currentlyPlayingSong, false);
			this.currentSongState.Stopped += HandleStopped;
		}
		
		/// <inheritdoc />
		public void Play()
		{
			// TODO: Pausing support
			PlayNextSong();
		}

		/// <inheritdoc />
		public void Stop()
		{
			// If we're playing a song, stop it.
			if (currentSongState != null)
			{
				// Unbind the Stopped event to prevent us from playing a new song.
				currentSongState.Stopped -= HandleStopped;
				currentSongState.Stop();

				currentSongState = null;
			}

			currentlyPlayingSong = null;
		}
	}
}