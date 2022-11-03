#nullable enable

using TrixelCreative.TrixelAudio.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TrixelCreative.TrixelAudio.Players
{
	public class PlaySoundOnClick :
		AudioPlayerBase,
		IPointerClickHandler
	{
		[SerializeField]
		private SoundEffectAsset? onClickSound;
		
		/// <inheritdoc />
		public void OnPointerClick(PointerEventData eventData)
		{
			if (onClickSound == null)
				return;
			
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				AudioSource.Play(onClickSound);
			}
		}
	}
}