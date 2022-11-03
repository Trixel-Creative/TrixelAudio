using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace TrixelCreative.TrixelAudio
{
	public class AudioSourcePool
	{
		private readonly TrixelAudioCore core;
		private readonly AudioSource[] pool = Array.Empty<AudioSource>();
		private int highestAcquiredIndex = -1;
		private readonly string poolName;

		public AudioSourcePool(TrixelAudioCore core, int poolSize, string poolName)
		{
			this.core = core;
			this.poolName = poolName;
			
			Assert.IsFalse(poolSize < 1, "[TrixelAudio] Sound effect pool size is below 1.");
			
			// Pre-allocate the pool
			this.pool = new AudioSource[poolSize];
		}

		public void Initialize()
		{
			highestAcquiredIndex = -1;
			
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
				GameObject go = new GameObject($"{poolName} Pool Object [{i}]");
				go.SetActive(false);
				go.transform.SetParent(core.gameObject.transform);
				existing = go.AddComponent<AudioSource>();
				pool[i] = existing;
			}
		}

		public void ReclaimUnusedAudioSources()
		{
			int lastStillPlaying = -1;
			for (var i = 0; i <= highestAcquiredIndex; i++)
			{
				AudioSource source = pool[i];
				if (!source.gameObject.activeSelf)
					continue;

				if (!source.isPlaying)
				{
					source.gameObject.SetActive(false);
					if (highestAcquiredIndex <= i)
					{
						highestAcquiredIndex = lastStillPlaying;
					}
				}
				else
				{
					lastStillPlaying = i;
				}
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
					if (i >= highestAcquiredIndex)
						highestAcquiredIndex = i;
					return source;
				}
			}
			
			return null;
		}
	}
}