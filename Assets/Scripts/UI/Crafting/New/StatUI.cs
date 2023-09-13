using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
	[Header("Name & Value")]
	public TMP_Text statName;
	public TMP_Text statValue;

	[Header("Colors")]
	public Image statBackground;
	public Color positiveColor;
	public Color negativeColor;
	public Color highlightColor;
	public Color highlightTextColor;

	[HideInInspector]
	public Color standardBackgroundColor;
	[HideInInspector]
	public Color standardTextColor;

	public void Initialize(string name, string value)
	{
		statName.text = name;
		statValue.text = value;
		standardBackgroundColor = statBackground.color;
		standardTextColor = statName.color;
	}

	public void Initialize(string name, string value, Color bgColor, Color textColor)
	{
		statName.text = name;
		statValue.text = value;
		statBackground.color = bgColor;
		statName.color = textColor;
		statValue.color = textColor;
		standardBackgroundColor = statBackground.color;
		standardTextColor = statName.color;
	}
}