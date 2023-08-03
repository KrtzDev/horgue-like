using UnityEngine;

public class MenuTab : UIButton
{
	[field: SerializeField]
	public GameObject AssociatedMenu { get; private set; }

	private void OnEnable()
	{
		OnButtonSelect += FocusMenu;
		OnButtonKeepFocus += KeepMenuFocus;
	}

	private void OnDisable()
	{
		OnButtonSelect -= FocusMenu;
		OnButtonKeepFocus -= KeepMenuFocus;
	}

	private void KeepMenuFocus()
	{
		AssociatedMenu.SetActive(true);
	}

	private void FocusMenu()
	{
		AssociatedMenu.SetActive(true);
	}

	public void UnFocusMenu()
	{
		AssociatedMenu.SetActive(false);
	}
}
