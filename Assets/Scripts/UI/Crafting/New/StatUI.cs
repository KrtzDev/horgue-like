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


	private Color _standartBackgroundColor;

	public void Initialize(string statName, string value)
	{
		_statName.text = statName;
		_value.text = value;
		_standartBackgroundColor = statBackground.color;
	}
}