using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Endscreen : MonoBehaviour
{
	[field: SerializeField]
	public TMP_Text TitleText { get; private set; }

	[field: SerializeField]
	public RectTransform RewardParent { get; private set; }

	public void BackToMainMenu()
	{
		SceneLoader.Instance.LoadScene(1);
	}

	public void CraftingMenu()
	{
		SceneLoader.Instance.LoadScene("SCENE_Weapon_Crafting");
	}

	public void NextWave()
	{
		SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
	}
}
