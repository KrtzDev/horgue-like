using UnityEngine;

public class MenuTab : UIButton
{
	[field: SerializeField]
	public UIMenu AssociatedMenu { get; private set; }

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
		AssociatedMenu.gameObject.SetActive(true);
	}

	private void FocusMenu()
	{
		AssociatedMenu.gameObject.SetActive(true);
		AssociatedMenu.SetFocusedMenu();
	}

	public void UnFocusMenu()
	{
		AssociatedMenu.gameObject.SetActive(false);
	}
}
