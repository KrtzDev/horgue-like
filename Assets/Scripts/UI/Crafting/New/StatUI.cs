using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _statName;
	[SerializeField]
	private TMP_Text _value;

	[SerializeField]
	public Image statBackground;
	[SerializeField]
	public Color positiveColor;
	[SerializeField]
	public Color negativeColor;
	public Color highlightColor;

	[HideInInspector]
	public Color standartBackgroundColor;

	public void Initialize(string statName, string value)
	{
		_statName.text = statName;
		_value.text = value;
		standartBackgroundColor = statBackground.color;
	}

	public void Initialize(string statName, string value, Color color)
	{
		_statName.text = statName;
		_value.text = value;
		statBackground.color = color;
		standartBackgroundColor = statBackground.color;
	}
}