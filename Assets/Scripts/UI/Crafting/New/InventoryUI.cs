using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : UIMenu
{
	[SerializeField]
	private WeaponUI _weaponUI;
	[SerializeField]
	private WeaponPartUI _weaponPartUI;
	[SerializeField]
	private Transform _inventorySlotContainer;

	[SerializeField]
	private Transform _selectedStatsUIContainer;

	private InventorySlot[] _inventorySlots;

	private void Awake()
	{
		_inventorySlots = new InventorySlot[_inventorySlotContainer.childCount];
		for (int i = 0; i < _inventorySlotContainer.childCount; i++)
		{
			_inventorySlots[i] = _inventorySlotContainer.GetChild(i).GetComponent<InventorySlot>();
		}

		InputManager.Instance.CharacterInputActions.UI.Submit.started += OnSubmit;

		SetUpNavigation();
	}

	private void OnSubmit(InputAction.CallbackContext obj)
	{
		if(EventSystem.current.currentSelectedGameObject.TryGetComponent<InventorySlot>(out InventorySlot slot))
		{
			slot.OnSubmit.Invoke();
		}
	}

	private void SetUpNavigation()
	{
		for (int i = 0; i < _inventorySlots.Length; i++)
		{

			Navigation navigation = new Navigation();
			navigation.mode = Navigation.Mode.Explicit;
			if(i - 1 >= 0)
				navigation.selectOnLeft = _inventorySlots[i - 1];
			if(i - 8 >= 0)
				navigation.selectOnUp = _inventorySlots[i - 8];
			if(i + 1 < _inventorySlots.Length)
				navigation.selectOnRight = _inventorySlots[i + 1];
			if(i + 8 < _inventorySlots.Length)
				navigation.selectOnDown = _inventorySlots[i + 8];

			_inventorySlots[i].navigation = navigation;
		}
	}

	private void Start()
	{
		FillInventoryUI();
	}

	private void OnDisable()
	{
		for (int i = 0; i < _inventorySlots.Length; i++)
		{
			_inventorySlots[i].Deselect();
		}
	}

	public override  void SetFocusedMenu()
	{
		StartCoroutine(FocusAfterFrame());
	}

	private IEnumerator FocusAfterFrame()
	{
		yield return new WaitForEndOfFrame();
		_inventorySlots[0].Select();
	}

	private void FillInventoryUI()
	{
		Inventory playerInventory = GameManager.Instance.inventory;
		WeaponPart[] weaponParts = playerInventory.GetAll();
		for (int i = 0; i < weaponParts.Length; i++)
		{
			WeaponPartUI weaponPartUI = Instantiate(_weaponPartUI, _inventorySlots[i].transform);
			_inventorySlots[i].SetWeaponPart(weaponPartUI);
			weaponPartUI.Initialize(weaponParts[i]);
			weaponPartUI.weaponUI = _weaponUI;
			weaponPartUI.statsContainer = _selectedStatsUIContainer;
		}
	}
}
