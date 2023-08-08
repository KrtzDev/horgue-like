using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	[SerializeField]
	private WeaponUI _weaponUI;
	[SerializeField]
	private WeaponPartUI _weaponPartUI;
	[SerializeField]
	private Transform _inventorySlotContainer;

	private InventorySlot[] _inventorySlots;

	private void Awake()
	{
		_inventorySlots = new InventorySlot[_inventorySlotContainer.childCount];
		for (int i = 0; i < _inventorySlotContainer.childCount; i++)
		{
			_inventorySlots[i] = _inventorySlotContainer.GetChild(i).GetComponent<InventorySlot>();
			_inventorySlots[i].index = i;
		}

		SetUpNavigation();
	}

	private void SetUpNavigation()
	{
		for (int i = 0; i < _inventorySlots.Length; i++)
		{

			Navigation navigation = new Navigation();
			navigation.mode = Navigation.Mode.Explicit;
			if(i - 1 > 0)
				navigation.selectOnLeft = _inventorySlots[i - 1];
			if(i - 8 > 0)
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

	private void FillInventoryUI()
	{
		Inventory playerInventory = GameManager.Instance._player.GetComponent<PlayerCharacter>().Inventory;
		WeaponPart[] weaponParts = playerInventory.GetAll();
		for (int i = 0; i < weaponParts.Length; i++)
		{
			_inventorySlots[i].SetWeaponPart(weaponParts[i]);

			WeaponPartUI newReward = Instantiate(_weaponPartUI, _inventorySlots[i].transform);
			newReward.Initialize(weaponParts[i]);
			newReward.weaponUI = _weaponUI;
		}
	}
}
