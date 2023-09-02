using UnityEngine;

namespace TrixelCreative.TrixelAudio.Data
{
	[CreateAssetMenu(menuName = "TrixelAudio/User Interface Sound Scheme")]
	public class UserInterfaceSoundSchemeAsset : ScriptableObject
	{
		[SerializeField]
		private SoundEffectAsset selectSound = null!;

		[SerializeField]
		private SoundEffectAsset navigateSound = null!;

		[SerializeField]
		private SoundEffectAsset cancelSound = null!;

		public void PlaySelectSound(AudioSource audioSource)
		{
			if (selectSound == null)
				return;

			selectSound.PlayOnAudioSource(audioSource);
		}
		
		public void PlayNavigateSound(AudioSource audioSource)
		{
			if (navigateSound == null)
				return;

			navigateSound.PlayOnAudioSource(audioSource);
		}
		
		public void PlayCancelSound(AudioSource audioSource)
		{
			if (cancelSound == null)
				return;

			cancelSound.PlayOnAudioSource(audioSource);
		}
		
		


	}
}