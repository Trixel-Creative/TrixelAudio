using TrixelCreative.TrixelAudio.Data;
using UnityEngine;

namespace TrixelCreative.TrixelAudio.Players
{
	public class SoundEvent : AudioPlayerBase
	{
		[SerializeField]
		private SoundEffectAsset? soundEffect;

		public void Play()
		{
			if (soundEffect != null)
				AudioSource.Play(soundEffect);
		}
	}
}