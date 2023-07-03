using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class HorgueVFX : MonoBehaviour
{
	[SerializeField] private ParticleSystem[] _particleSystems;

	[SerializeField] private float _longestDuration;

	public void Play()
	{
		for (int i = 0; i < _particleSystems.Length; i++)
		{
			_particleSystems[i].Play();
		}
		DestroyAfterDuration();
	}

	private async void DestroyAfterDuration()
	{
		await Task.Delay((int)(_longestDuration * 1000f));

		if(Application.isPlaying)
			Destroy(gameObject);
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