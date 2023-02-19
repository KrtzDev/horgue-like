using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveEndScreen : MonoBehaviour
{
	[field: SerializeField]
	public TMP_Text TitleText { get; private set; }

	public void NextWave()
	{
		SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
	}
}
