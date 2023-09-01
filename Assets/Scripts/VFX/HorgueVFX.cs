using System;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class HorgueVFX : MonoBehaviour
{
	public event Action OnFinished;

	[SerializeField] private ParticleSystem[] _particleSystems;

	[SerializeField] private float _longestDuration;

	public void Play()
	{
		for (int i = 0; i < _particleSystems.Length; i++)
		{
			_particleSystems[i].Play();
		}
	}

	public void ReturnToPool(ObjectPool<HorgueVFX> vfxPool)
	{
		if (this != null && Application.isPlaying && this.isActiveAndEnabled)
			vfxPool.ReturnObjectToPool(this);
	}

	public async void ReturnToPoolOnFinished(ObjectPool<HorgueVFX> vfxPool)
	{
		await Task.Delay((int)(_longestDuration * 1000f));

		ReturnToPool(vfxPool);
	}

	public async void ReturnToPoolAfterTime(ObjectPool<HorgueVFX> vfxPool, float duration)
	{
		await Task.Delay((int)(duration * 1000f));

		ReturnToPool(vfxPool);
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		for (int i = 0; i < _particleSystems.Length; i++)
		{
			if (_particleSystems[i].main.duration > _longestDuration)
				_longestDuration = _particleSystems[i].main.duration;
		}
	}
#endif
}