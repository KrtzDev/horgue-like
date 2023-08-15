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
	private WeaponUI _weaponUI;
	[SerializeField]
	private WeaponPartUI _weaponPartUI;
	[SerializeField]
	private Transform _inventorySlotContainer;
	[SerializeField]
	private HoldButton _buyButton;
	[SerializeField]
	private HoldButton _sellButton;
	[SerializeField]
	private HoldButton _equipButton;

	[SerializeField]
	private Transform _selectedStatsUIContainer;

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
			inventorySlot.OnSelected += UpdateComparisonUI;
		}

		_shopUI.OnBought += AddToInventory;

		SetUpNavigation();
	}

	private void OnItemEquip(WeaponPartUI weaponPart)
	{
		GameManager.Instance.inventory.RemoveFromInventory(weaponPart.weaponPart);
		_weaponUI.SetNewWeaponPart(weaponPart);
		
		StartCoroutine(FillInventoryUI());
	}

	private void OnItemSold(WeaponPart weaponPart)
	{
		StartCoroutine(FillInventoryUI());
	}

	private void AddToInventory(WeaponPart weaponPart)
	{
		GameManager.Instance.inventory.AddToInventory(weaponPart);
		if(gameObject.activeSelf)
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
		{
			_inventorySlots[i].Deselect();
		}
	}

	public override void SetFocusedMenu()
	{
		_equipButton.Enable();
		_sellButton.Enable();
		_buyButton.Disable();
		StartCoroutine(FocusAfterFrame());
	}

	private IEnumerator FocusAfterFrame()
	{
		yield return new WaitForEndOfFrame();
		_inventorySlots[0].Select();

		StartCoroutine(FillInventoryUI());
	}

	private IEnumerator FillInventoryUI()
	{
		Inventory playerInventory = GameManager.Instance.inventory;
		List<WeaponPart> weaponParts = playerInventory.GetAll();

		ClearInventoryUI();

		yield return new WaitForEndOfFrame();

		for (int i = 0; i < weaponParts.Count; i++)
		{
			WeaponPartUI weaponPartUI = Instantiate(_weaponPartUI, _inventorySlots[i].transform);
			_inventorySlots[i].SetWeaponPart(weaponPartUI);
			weaponPartUI.Initialize(weaponParts[i]);
			weaponPartUI.weaponUI = _weaponUI;
			weaponPartUI.statsContainer = _selectedStatsUIContainer;
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
				_currentSelection = inventorySlot.GetWeaponPart();
			}
			else
			{
				_currentSelection = null;
			}
			UpdateComparisonUI();
		}
	}

	public void UpdateComparisonUI()
	{
		for (int i = 0; i < _selectedStatsUIContainer.childCount; i++)
			Destroy(_selectedStatsUIContainer.GetChild(i).gameObject);

		if (EventSystem.current.currentSelectedGameObject.TryGetComponent(out InventorySlot inventorySlot))
			_currentSelection = inventorySlot.HasWeaponPart() ? inventorySlot.GetWeaponPart() : null;

		_shopUI.UpdateComparisonUI(_currentSelection);
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
