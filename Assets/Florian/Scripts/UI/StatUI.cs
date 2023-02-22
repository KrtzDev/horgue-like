using TMPro;
using UnityEngine;

public class StatUI : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _statName;
	[SerializeField]
	private TMP_Text _value;

	public void Initialize(string statName, string value)
	{
		_statName.text = statName;
		_value.text = value;
	}
}