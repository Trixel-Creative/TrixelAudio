using UnityEngine;

namespace TrixelCreative.TrixelAudio.Data
{
	[CreateAssetMenu(menuName = "TrixelAudio/Core Configuration")]
	public class CoreConfiguration : ScriptableObject
	{
		[SerializeField]
		[Tooltip("Determines how many audio sources are allocated in the scene for sound effects. Effectively this determines how many sound effects can be played at once.")]
		private int soundEffectPoolSize = 50;

		public int SoundEffectPoolSize => soundEffectPoolSize;
        
		private void OnValidate()
		{
			if (soundEffectPoolSize < 1)
				soundEffectPoolSize = 1;
		}
	}
}