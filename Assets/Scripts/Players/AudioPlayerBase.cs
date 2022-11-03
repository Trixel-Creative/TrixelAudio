using System;
using TrixelCreative.TrixelAudio.Utility;
using UnityEngine;

namespace TrixelCreative.TrixelAudio.Players
{
	[RequireComponent(typeof(TrixelAudioSource))]
	public abstract class AudioPlayerBase : MonoBehaviour
	{
		private TrixelAudioSource audioSource = null!;

		protected TrixelAudioSource AudioSource => audioSource;
		
		private void Awake()
		{
			this.MustGetComponent(out audioSource);
			OnAwake();
		}
		
		protected virtual void OnAwake() { }
	}
}