using UnityEngine;
using UnityEngine.Audio;

namespace TrixelCreative.TrixelAudio.Data
{
	[CreateAssetMenu(menuName = "TrixelAudio/Core Configuration")]
	public class CoreConfiguration : ScriptableObject
	{
		[Header("Memory management")]
		[SerializeField]
		[Tooltip("Determines how many audio sources are allocated in the scene for sound effects. Effectively this determines how many sound effects can be played at once.")]
		private int soundEffectPoolSize = 50;

		[Header("Audio mixers")]
		[SerializeField]
		private AudioMixerGroup soundEffectsGroup = null!;

		[SerializeField]
		private AudioMixerGroup musicGroup = null!;

		[SerializeField]
		private AudioMixerGroup ambienceGroup = null!;

		[SerializeField]
		private AudioMixerGroup dialogueGroup = null!;

		[Header("UI")]
		[SerializeField]
		[Tooltip("Choose a User Interface Sound Scheme asset to use for user interface navigation sounds.")]
		private UserInterfaceSoundSchemeAsset uiSoundScheme = null!;
		
		public AudioMixerGroup SoundEffectsMixer => soundEffectsGroup;
		public AudioMixerGroup MusicMixer => musicGroup;
		public AudioMixerGroup AmbienceMixer => ambienceGroup;
		public AudioMixerGroup DialogueMixer => dialogueGroup;

		public int SoundEffectPoolSize => soundEffectPoolSize;

		public UserInterfaceSoundSchemeAsset? UserInterfaceSoundScheme => uiSoundScheme;
		
		private void OnValidate()
		{
			if (soundEffectPoolSize < 1)
				soundEffectPoolSize = 1;
		}
	}
}