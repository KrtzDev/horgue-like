using UnityEngine;
using DG.Tweening;
using System;

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
		AssociatedMenu.CanvasGroup.DOFade(1f,.2f).OnComplete(() => SetObjectActive(true));
	}

	public void UnFocusMenu()
	{
		AssociatedMenu.CanvasGroup.DOFade(0f,.2f).OnComplete(() => SetObjectActive(false));
	}

	private void SetObjectActive(bool active)
	{
		AssociatedMenu.gameObject.SetActive(active);
		if (active == true)
			AssociatedMenu.SetFocusedMenu();
	}
}
