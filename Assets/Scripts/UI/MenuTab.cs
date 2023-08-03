using System;
using UnityEngine;

public class MenuTab : UIButton
{
	[field: SerializeField]
	public GameObject AssociatedMenu { get; private set; }

	private void OnEnable()
	{
		OnButtonSelect += () => FocusMenu(AssociatedMenu);
		OnButtonKeepFocus += () => KeepFocus(AssociatedMenu);
	}

	private void KeepFocus(GameObject menu)
	{
		AssociatedMenu.SetActive(true);
	}

	private void OnDisable()
	{
		OnButtonSelect -= () => FocusMenu(AssociatedMenu);
		OnButtonKeepFocus -= () => KeepFocus(AssociatedMenu);
	}

	private void FocusMenu(GameObject menu)
	{
		Debug.Log("Opened Menu:" + menu.name);
		menu.SetActive(true);
	}

	public void UnFocusMenu()
	{
		AssociatedMenu.SetActive(false);
	}
}
