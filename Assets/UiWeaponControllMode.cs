using TMPro;
using UnityEngine;

public class UiWeaponControllMode : MonoBehaviour
{
	[SerializeField] private TMP_Text _modeText;

    private void Start()
    {
		if (GameManager.Instance != null)
		{
			_modeText.text = "All Auto";
		}
	}

    private void Update()
	{
		if (GameManager.Instance != null && Time.timeScale != 0)
        {
			switch (GameManager.Instance.weaponControll)
			{
				case WeaponControllKind.AllAuto:
					_modeText.text = "All Auto";
					break;
				case WeaponControllKind.AutoShootManualAim:
					_modeText.text = "Auto Shoot Manual Aim";
					break;
				case WeaponControllKind.AllManual:
					_modeText.text = "All Manual";
					break;

			}
        }
			
	}
}
