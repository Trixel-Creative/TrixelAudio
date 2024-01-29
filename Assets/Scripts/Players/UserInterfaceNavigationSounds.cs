using TrixelCreative.TrixelAudio.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TrixelCreative.TrixelAudio.Players
{
	[RequireComponent(typeof(RectTransform))]
	public sealed class UserInterfaceNavigationSounds : 
		AudioPlayerBase,
		ISelectHandler,
		IPointerEnterHandler,
		IPointerClickHandler,
		ISubmitHandler,
		ICancelHandler
	{
		/// <inheritdoc />
		public void OnSelect(BaseEventData eventData)
		{
			if (!TryGetUiSounds(out UserInterfaceSoundSchemeAsset uiSounds))
				return;

			if (!AudioSource.TryAcquireAudioSource(out AudioSource pooledSource))
				return;
			
			uiSounds.PlayNavigateSound(pooledSource);
		}

		/// <inheritdoc />
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!TryGetUiSounds(out UserInterfaceSoundSchemeAsset uiSounds))
				return;

			if (!AudioSource.TryAcquireAudioSource(out AudioSource? pooledSource))
				return;
			
			uiSounds.PlayNavigateSound(pooledSource);
		}

		/// <inheritdoc />
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!TryGetUiSounds(out UserInterfaceSoundSchemeAsset uiSounds))
				return;

			if (!AudioSource.TryAcquireAudioSource(out AudioSource pooledSource))
				return;
			
			uiSounds.PlaySelectSound(pooledSource);
		}

		/// <inheritdoc />
		public void OnSubmit(BaseEventData eventData)
		{
			if (!TryGetUiSounds(out UserInterfaceSoundSchemeAsset uiSounds))
				return;

			if (!AudioSource.TryAcquireAudioSource(out AudioSource pooledSource))
				return;
			
			uiSounds.PlaySelectSound(pooledSource);
		}

		private bool TryGetUiSounds(out UserInterfaceSoundSchemeAsset soundScheme)
		{
			return this.AudioSource.TryGetUiSounds(out soundScheme);
		}

		/// <inheritdoc />
		public void OnCancel(BaseEventData eventData)
		{
			if (!TryGetUiSounds(out UserInterfaceSoundSchemeAsset uiSounds))
				return;

			if (!AudioSource.TryAcquireAudioSource(out AudioSource pooledSource))
				return;
			
			uiSounds.PlayCancelSound(pooledSource);
		}
	}
}