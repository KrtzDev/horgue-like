using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Endscreen : MonoBehaviour
{
	[field: SerializeField]
	public RectTransform RewardParent { get; private set; }

	[SerializeField]
	private TMP_Text _titleText;

	[SerializeField]
	private TMP_Text _StatsText;

	[SerializeField]
	private List<GameObject> _wonStateUIElements = new List<GameObject>();
	[SerializeField]
	private List<GameObject> _lostStateUIElements = new List<GameObject>();

	public void ShowWonStateUI()
	{
		foreach (var UIElement in _wonStateUIElements)
		{
			UIElement.SetActive(true);
		}
		foreach (var UIElement in _lostStateUIElements)
		{
			UIElement.SetActive(false);
		}

		_titleText.text = "Won";
	}

	public void ShowLostStateUI()
	{
		foreach (var UIElement in _wonStateUIElements)
		{
			UIElement.SetActive(false);
		}
		foreach (var UIElement in _lostStateUIElements)
		{
			UIElement.SetActive(true);
		}

		_titleText.text = "Lost";
	}

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
