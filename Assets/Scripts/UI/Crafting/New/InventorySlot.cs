using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : Selectable
{
	public Action OnSubmit;

	[SerializeField]
	private Image _selectionIndicator;

	[HideInInspector]
	private WeaponPartUI _weaponPart;

	protected override void Awake()
	{
		base.Awake();
		OnSubmit += StartEquip;
	}


	public void SetWeaponPart(WeaponPartUI weaponPart)
	{
		this._weaponPart = weaponPart;
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		_selectionIndicator.enabled= true;

		if (_weaponPart)
			_weaponPart.Select();
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		if (_weaponPart)
			_weaponPart.DestroyToolTip();

		base.OnDeselect(eventData);
		Deselect();
	}

	public void Deselect()
	{
		_selectionIndicator.enabled = false;
	}

	private void StartEquip()
	{
		Debug.Log("Eqip");
	}
}