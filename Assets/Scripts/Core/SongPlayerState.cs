using System;

namespace TrixelCreative.TrixelAudio
{
	public class SongPlayerState
	{
		private readonly SongPlayerCore.SongPlayerStateInternal internalState;

		public float Length => internalState.Length;
		public float Position => internalState.Position;
		public PlaybackState PlaybackState => internalState.PlaybackState;

		public event Action? Stopped; 

		internal SongPlayerState(SongPlayerCore.SongPlayerStateInternal internalState)
		{
			this.internalState = internalState;
			this.internalState.Stopped += HandleStoppedInternally;
		}

		public void Stop()
		{
			if (this.PlaybackState == PlaybackState.Stopped)
				return;

			this.internalState.RequestStop();
		}

		private void HandleStoppedInternally()
		{
			this.Stopped?.Invoke();
		}
	}
}