using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : Selectable
{
	public Action OnSubmit;
	public Action<WeaponPart> OnSell;
	public Action<WeaponPartUI> OnEquip;
	public Action<InventorySlot> OnSelected;
	public Action OnDeselected;

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

		sellButton.OnButtonExecute -= Sell;
		equipButton.OnButtonExecute -= Equip;
		sellButton.OnButtonExecute += Sell;
		equipButton.OnButtonExecute += Equip;
	}

	private void Equip()
	{
		if (!_selected || !HasWeaponPart())
			return;

		_selected = false;

		OnEquip.Invoke(_weaponPart);
		equipButton.OnButtonExecute -= Equip;
		sellButton.OnButtonExecute -= Sell;
	}

	private void Sell()
	{
		if (!_selected || !HasWeaponPart())
			return;

		sellButton.OnButtonExecute -= Sell;
		equipButton.OnButtonExecute -= Equip;
		_selected = false;

		GameManager.Instance.inventory.Wallet.Store((int)(_weaponPart.weaponPart.cost * 0.5f));
		GameManager.Instance.inventory.RemoveFromInventory(_weaponPart.weaponPart);

		_weaponPart.DestroyToolTip();
		WeaponPartUI[] weaponPartUIs = transform.GetComponentsInChildren<WeaponPartUI>();
		for (int i = 0; i < weaponPartUIs.Length; i++)
			Destroy(weaponPartUIs[i].gameObject);

		OnSell.Invoke(_weaponPart.weaponPart);
	}

	public void Setselected(bool selected)
	{
		_selected = selected;
		_selectionIndicator.enabled = selected;
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		OnSelected.Invoke(this);

		if (_weaponPart)
			_weaponPart.Select();
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		OnDeselected?.Invoke();

		if (_weaponPart)
		{
			_weaponPart.DestroyToolTip();
			_weaponPart.Deselect();
		}
	}
}