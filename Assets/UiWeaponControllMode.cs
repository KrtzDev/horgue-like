using TMPro;
using UnityEngine;

public class UiWeaponControllMode : MonoBehaviour
{
	[SerializeField] private TMP_Text _modeText;

	private void Update()
	{
		if (GameManager.Instance != null)
			_modeText.text = GameManager.Instance.weaponControll.ToString();
	}
}
