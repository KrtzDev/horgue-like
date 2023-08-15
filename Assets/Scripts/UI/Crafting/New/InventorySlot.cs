using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : Selectable
{
	public Action OnSubmit;
	public Action<WeaponPart> OnSell;
	public Action<WeaponPartUI> OnEquip;
	public Action OnSelected;

	public HoldButton sellButton;
	public HoldButton equipButton;

	[SerializeField]
	private Image _selectionIndicator;

	[HideInInspector]
	private WeaponPartUI _weaponPart;

	private bool _selected;

	public bool HasWeaponPart() => _weaponPart != null;

	public WeaponPartUI GetWeaponPart() => _weaponPart;

	public void SetWeaponPart(WeaponPartUI weaponPart)
	{
		_weaponPart = weaponPart;

		sellButton.OnButtonExecute += Sell;
		equipButton.OnButtonExecute += Equip;
	}

	private void Equip()
	{
		if (!_selected || _weaponPart == null)
			return;

		_selected = false;

		OnEquip.Invoke(_weaponPart);
		equipButton.OnButtonExecute -= Equip;

		_weaponPart.DestroyToolTip();
	}

	private void Sell()
	{ 
		if (!_selected || _weaponPart == null)
			return;

		_selected = false;

		GameManager.Instance.inventory.Wallet.Store(_weaponPart.weaponPart.value);
		GameManager.Instance.inventory.RemoveFromInventory(_weaponPart.weaponPart);

		OnSell.Invoke(_weaponPart.weaponPart);
		sellButton.OnButtonExecute -= Sell;

		_weaponPart.DestroyToolTip();
		Destroy(_weaponPart.gameObject);
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);

		OnSelected.Invoke();
		_selectionIndicator.enabled= true;

		if (_weaponPart)
			_weaponPart.Select();

		_selected = true;
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		if (_weaponPart)
			_weaponPart.DestroyToolTip();

		base.OnDeselect(eventData);
		Deselect();

		_selected = false;
	}

	public void Deselect()
	{
		_selectionIndicator.enabled = false;
	}
}