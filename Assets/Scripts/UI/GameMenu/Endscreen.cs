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
	private TMP_Text _levelStatsValues;

	[SerializeField]
	private TMP_Text _totalStatsValues;

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

		SetStatsText();

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

		SetStatsText();

		_titleText.text = "Lost";
	}

	private void SetStatsText()
    {
		SetLevelStatsText();
		SetTotalStatsText();
    }

	private void SetLevelStatsText()
    {
		_levelStatsValues.text =
			FormatNumber(StatsTracker.Instance.damageDealtLevel) + "\n" +
			FormatNumber(StatsTracker.Instance.shotsFiredLevel) + "\n" +
			FormatNumber(StatsTracker.Instance.jumpsUsedLevel) + "\n" +
			FormatNumber(StatsTracker.Instance.enemiesKilledLevel) + "\n" +
			FormatNumber(StatsTracker.Instance.scoreCollectedLevel);
	}

	private void SetTotalStatsText()
    {
		_totalStatsValues.text =
			FormatNumber(StatsTracker.Instance.damageDealtTotal) + "\n" +
			FormatNumber(StatsTracker.Instance.shotsFiredTotal) + "\n" +
			FormatNumber(StatsTracker.Instance.jumpsUsedTotal) + "\n" +
			FormatNumber(StatsTracker.Instance.enemiesKilledTotal) + "\n" +
			FormatNumber(StatsTracker.Instance.scoreCollectedTotal);
	}

	private string FormatNumber(float num)
    {
		float numStr;
		string suffix;

		if (num < 1000)
		{
			numStr = num;
			suffix = "";
			return numStr.ToString() + " " + suffix;
		}
		else if (num < 1000000)
		{
			numStr = num / 1000;
			suffix = "K";
		}
		else if (num < 1000000000)
		{
			numStr = num / 1000000;
			suffix = "M";
		}
		else
		{
			numStr = num / 1000000000;
			suffix = "B";
		}

		return string.Format("{0:0.00}", numStr).ToString() + " " + suffix;
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
