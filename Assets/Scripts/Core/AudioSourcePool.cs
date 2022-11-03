﻿using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace TrixelCreative.TrixelAudio
{
	public class AudioSourcePool
	{
		private readonly TrixelAudioCore core;
		private readonly AudioSource[] pool;
		private readonly bool[] used;

		public AudioSourcePool(TrixelAudioCore core, int poolSize)
		{
			this.core = core;
			
			Assert.IsFalse(poolSize < 1, "[TrixelAudio] Sound effect pool size is below 1.");
			
			// Pre-allocate the pool
			this.pool = new AudioSource[poolSize];
		}

		public void Initialize()
		{
			for (var i = 0; i < pool.Length; i++)
			{
				// Consume any existing AudioSources if they're not destroyed.
				AudioSource existing = pool[i];
				if (existing != null && existing.transform.parent != this.core.transform)
				{
					existing.transform.SetParent(this.core.transform);
					continue;
				}
				
				// No existing source in this slot, create one
				// We start the object as inactive, we'll activate it when we need it.
				GameObject go = new GameObject($"TrixelAudio Sound Effect Pool Object [{i}]");
				go.SetActive(false);
				go.transform.SetParent(core.gameObject.transform);
				existing = go.AddComponent<AudioSource>();
				pool[i] = existing;
			}
		}

		public void ReclaimUnusedAudioSources()
		{
			for (var i = 0; i < pool.Length; i++)
			{
				AudioSource source = pool[i];
				if (!source.gameObject.activeSelf)
					continue;
				
				if (!source.isPlaying)
					source.gameObject.SetActive(false);
			}
		}
		
		public AudioSource? GetNextAvailableAudioSource()
		{
			for (var i = 0; i < pool.Length; i++)
			{
				AudioSource source = pool[i];
				if (!source.gameObject.activeInHierarchy)
				{
					source.gameObject.SetActive(true);
					return source;
				}
			}
			
			return null;
		}
	}
}