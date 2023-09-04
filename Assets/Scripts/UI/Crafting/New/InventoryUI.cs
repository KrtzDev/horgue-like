using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : UIMenu
{
	[SerializeField]
	private ShopUI _shopUI;
	[SerializeField]
	private ComparisonUI _comparisonUI;

	[SerializeField]
	private WeaponUI _weaponUI;
	[SerializeField]
	private WeaponPartUI _weaponPartUI;
	[SerializeField]
	private Transform _inventorySlotContainer;
	[SerializeField]
	private HoldButton _buyButton;
	[SerializeField]
	private HoldButton _newShopButton;
	[SerializeField]
	private HoldButton _sellButton;
	[SerializeField]
	private HoldButton _equipButton;

	[SerializeField]
	private Transform _comparisonContainerSelected;
	[SerializeField]
	private Transform _comparisonContainerEquipped;
	[SerializeField]
	private RectTransform _comparisonContainerShopItem;
	[SerializeField]
	private ToolTipUI _toolTipUI_prefab;

	private List<InventorySlot> _inventorySlots;

	private WeaponPartUI _currentSelection;

	private void Awake()
	{
		_inventorySlots = new List<InventorySlot>();
		for (int i = 0; i < _inventorySlotContainer.childCount; i++)
		{
			InventorySlot inventorySlot = _inventorySlotContainer.GetChild(i).GetComponent<InventorySlot>();
			_inventorySlots.Add(inventorySlot);

			inventorySlot.OnSell += OnItemSold;
			inventorySlot.sellButton = _sellButton;
			inventorySlot.OnEquip += OnItemEquip;
			inventorySlot.equipButton = _equipButton;
			inventorySlot.OnSelected += OnInventorySlotSelected;
		}

		_shopUI.OnBought += AddToInventory;

		SetUpNavigation();
	}

	private void OnItemEquip(WeaponPartUI weaponPart)
	{
		Tween move = weaponPart.transform.DOMove(_weaponUI.equipNewPartTransform.position, .5f);
		Tween color = weaponPart.WeaponPartImage.DOColor(Color.white, .5f);

		Sequence sequence = DOTween.Sequence();
		sequence.Append(move)
			.Append(color)
			.OnComplete(() => OnEquipTweenFinished(weaponPart));

		sequence.Play();
	}

	private void OnEquipTweenFinished(WeaponPartUI weaponPart)
	{
		GameManager.Instance.inventory.RemoveFromInventory(weaponPart.weaponPart);

		WeaponPart oldPart =_weaponUI.weapon.GetWeaponPartOfType(weaponPart.weaponPart);
		AddToInventory(oldPart);

		_weaponUI.SetNewWeaponPart(weaponPart);

		AudioManager.Instance.PlaySound("WeaponPartEquip");

		weaponPart.DestroyToolTip();
		Destroy(weaponPart.gameObject);



		StartCoroutine(FillInventoryUI());
	}

	private void OnItemSold(WeaponPart weaponPart)
	{
		StartCoroutine(FillInventoryUI());

	}

	private void AddToInventory(WeaponPart weaponPart)
	{
		GameManager.Instance.inventory.AddToInventory(weaponPart);
		if (gameObject.activeSelf)
			StartCoroutine(FillInventoryUI());
	}

	private void SetUpNavigation()
	{
		for (int i = 0; i < _inventorySlots.Count; i++)
		{

			Navigation navigation = new Navigation();
			navigation.mode = Navigation.Mode.Explicit;
			if (i - 1 >= 0)
				navigation.selectOnLeft = _inventorySlots[i - 1];
			if (i - 8 >= 0)
				navigation.selectOnUp = _inventorySlots[i - 8];
			if (i + 1 < _inventorySlots.Count)
				navigation.selectOnRight = _inventorySlots[i + 1];
			if (i + 8 < _inventorySlots.Count)
				navigation.selectOnDown = _inventorySlots[i + 8];

			_inventorySlots[i].navigation = navigation;
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < _inventorySlots.Count; i++)
			_inventorySlots[i].Setselected(false);
	}

	public override void SetFocusedMenu()
	{
		_equipButton.Enable();
		_sellButton.Enable();
		_buyButton.Disable();
		_newShopButton.Disable();
		StartCoroutine(FocusAfterFrame());
	}

	private IEnumerator FocusAfterFrame()
	{
		yield return new WaitForEndOfFrame();
		StartCoroutine(FillInventoryUI());

		_inventorySlots[0].Select();
	}

	private IEnumerator FillInventoryUI()
	{
		yield return new WaitForEndOfFrame();

		Inventory playerInventory = GameManager.Instance.inventory;
		List<WeaponPart> weaponParts = playerInventory.GetAll();

		ClearInventoryUI();

		for (int i = 0; i < weaponParts.Count; i++)
		{
			WeaponPartUI weaponPartUI = Instantiate(_weaponPartUI, _inventorySlots[i].transform);
			_inventorySlots[i].SetWeaponPart(weaponPartUI);
			weaponPartUI.Initialize(weaponParts[i]);
			weaponPartUI.weaponUI = _weaponUI;
			weaponPartUI.statsContainer = _comparisonContainerSelected;
		}

		SelectInventorySlot();
	}

	private void SelectInventorySlot()
	{
		if (EventSystem.current.currentSelectedGameObject.TryGetComponent(out InventorySlot inventorySlot))
		{
			if (inventorySlot.HasWeaponPart())
			{
				inventorySlot.Select();
				OnInventorySlotSelected(inventorySlot);
				_currentSelection = inventorySlot.GetWeaponPart();
			}
			else
			{
				_currentSelection = null;
			}
			UpdateComparisonUI();
		}
	}

	private void OnInventorySlotSelected(InventorySlot inventorySlot)
	{
		UpdateComparisonUI();
		SetSelected(inventorySlot);
	}

	private void SetSelected(InventorySlot inventorySlot)
	{
		for (int i = 0; i < _inventorySlots.Count; i++)
		{
			_inventorySlots[i].Setselected(false);
		}
		inventorySlot.Setselected(true);
	}

	public void UpdateComparisonUI()
	{
		_comparisonUI.ClearComparisonContainer();

		if (EventSystem.current.currentSelectedGameObject.TryGetComponent(out InventorySlot inventorySlot))
			_currentSelection = inventorySlot.HasWeaponPart() ? inventorySlot.GetWeaponPart() : null;

		if (_currentSelection == null)
			return;

		foreach (ToolTipUI item in _shopUI.shopItems)
		{
			if (item.weaponPart.GetType() == _currentSelection.weaponPart.GetType())
			{
				ToolTipUI toolTipUI = Instantiate(_toolTipUI_prefab, _comparisonContainerShopItem);
				toolTipUI.Initialize(item.weaponPart);

				StartCoroutine(LayoutAfterFrame());
			}
		}

		if (_currentSelection)
			UpdateEquippedComparisonUI(_currentSelection.weaponPart);
	}

	private IEnumerator LayoutAfterFrame()
	{
		yield return new WaitForEndOfFrame();
		LayoutRebuilder.ForceRebuildLayoutImmediate(_comparisonContainerShopItem);
	}

	private void UpdateEquippedComparisonUI(WeaponPart weaponPart)
	{
		ToolTipUI selectedTooltip = Instantiate(_toolTipUI_prefab, _comparisonContainerSelected);
		selectedTooltip.Initialize(weaponPart);

		_comparisonUI.UpdateEquippedComparisonUI(_weaponUI, weaponPart);
	}

	private void ClearInventoryUI()
	{
		for (int i = 0; i < _inventorySlots.Count; i++)
		{
			if (_inventorySlots[i].HasWeaponPart())
				Destroy(_inventorySlots[i].GetWeaponPart().gameObject);
		}
	}
}
