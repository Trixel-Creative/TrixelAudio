using TrixelCreative.TrixelAudio.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TrixelCreative.TrixelAudio.Players
{
	public class PlaySoundOnHover :
		AudioPlayerBase,
		IPointerEnterHandler
	{
		[SerializeField]
		private SoundEffectAsset? onHoverSound;

		/// <inheritdoc />
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (onHoverSound == null)
				return;

			AudioSource.Play(onHoverSound);
		}
	}
}