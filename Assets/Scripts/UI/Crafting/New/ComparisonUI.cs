using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ComparisonUI : MonoBehaviour
{
	public WeaponPart CurrentSelected =>
		_selectionContainer.childCount > 0 ?
		_selectionContainer.GetChild(0)?.GetComponent<ToolTipUI>().weaponPart :
		null;

	[SerializeField]
	private ToolTipUI _toolTipUI_prefab;

	[SerializeField]
	private Transform _selectionContainer;
	[SerializeField]
	private Transform _equippedContainer;
	[SerializeField]
	private Transform _shopItemContainer;
	[SerializeField]
	private Scrollbar _shopItemScrollBar;

	private Vector2 _scrollInput;

	private void OnEnable()
	{
		InputManager.Instance.CharacterInputActions.UI.ScrollWheel.performed += Scroll;
	}

	private void Scroll(InputAction.CallbackContext ctx) => _scrollInput = ctx.ReadValue<Vector2>();

	private void Update()
	{
		if(Mathf.Abs(_scrollInput.y) > 0)
			_shopItemScrollBar.value = Mathf.Clamp(_shopItemScrollBar.value + _scrollInput.y * .7f * Time.deltaTime, 0, 1);
	}

	public void ClearComparisonContainer()
	{
		for (int i = 0; i < _selectionContainer.childCount; i++)
			Destroy(_selectionContainer.GetChild(i).gameObject);

		for (int i = 0; i < _equippedContainer.childCount; i++)
			Destroy(_equippedContainer.GetChild(i).gameObject);

		for (int i = 0; i < _shopItemContainer.childCount; i++)
			Destroy(_shopItemContainer.GetChild(i).gameObject);
	}

	public void UpdateEquippedComparisonUI(WeaponUI weaponUI, WeaponPart weaponPart)
	{
		ToolTipUI toolTipUI = Instantiate(_toolTipUI_prefab, _equippedContainer);
		toolTipUI.Initialize(weaponUI.weapon.GetWeaponPartOfType(weaponPart));

		StartCoroutine(CompareAfterFrame());
	}

	private IEnumerator CompareAfterFrame()
	{
		yield return new WaitForEndOfFrame();
		Compare();
	}

	private void Compare()
	{
		ToolTipUI[] selection = GetTooltipUIsFromContainer(_selectionContainer);
		ToolTipUI[] equipped = GetTooltipUIsFromContainer(_equippedContainer);
		ToolTipUI[] shopItem = GetTooltipUIsFromContainer(_shopItemContainer);

		for (int i = 0; i < selection.Length; i++)
			CompareParts(selection[i], equipped[0]);
		for (int i = 0; i < shopItem.Length; i++)
			CompareParts(shopItem[i], equipped[0]);
	}

	private ToolTipUI[] GetTooltipUIsFromContainer(Transform container)
	{
		ToolTipUI[] selection = new ToolTipUI[container.childCount];
		for (int i = 0; i < selection.Length; i++)
			selection[i] = container.GetChild(i).GetComponent<ToolTipUI>();

		return selection;
	}

	private void CompareParts(ToolTipUI newPart, ToolTipUI equipped)
	{
		WeaponPart newWeaponPart = newPart.weaponPart;
		WeaponPart equippedWeaponPart = equipped.weaponPart;

		if (newWeaponPart.baseDamage > equippedWeaponPart.baseDamage)
			newPart.damageStatUI.statBackground.color = newPart.damageStatUI.positiveColor;
		else if (newWeaponPart.baseDamage < equippedWeaponPart.baseDamage)
			newPart.damageStatUI.statBackground.color = newPart.damageStatUI.negativeColor;

		if (newWeaponPart.attackSpeed > equippedWeaponPart.attackSpeed)
			newPart.attackSpeedStatUI.statBackground.color = newPart.attackSpeedStatUI.positiveColor;
		else if (newWeaponPart.attackSpeed < equippedWeaponPart.attackSpeed)
			newPart.attackSpeedStatUI.statBackground.color = newPart.attackSpeedStatUI.negativeColor;

		if (newWeaponPart.cooldown < equippedWeaponPart.cooldown)
			newPart.cooldownStatUI.statBackground.color = newPart.cooldownStatUI.positiveColor;
		else if (newWeaponPart.cooldown > equippedWeaponPart.cooldown)
			newPart.cooldownStatUI.statBackground.color = newPart.cooldownStatUI.negativeColor;

		if (newWeaponPart.projectileSize > equippedWeaponPart.projectileSize)
			newPart.projectileSizeStatUI.statBackground.color = newPart.projectileSizeStatUI.positiveColor;
		else if (newWeaponPart.projectileSize < equippedWeaponPart.projectileSize)
			newPart.projectileSizeStatUI.statBackground.color = newPart.projectileSizeStatUI.negativeColor;

		if (newWeaponPart.critChance > equippedWeaponPart.critChance)
			newPart.critChanceStatUI.statBackground.color = newPart.critChanceStatUI.positiveColor;
		else if (newWeaponPart.critChance < equippedWeaponPart.critChance)
			newPart.critChanceStatUI.statBackground.color = newPart.critChanceStatUI.negativeColor;

		if (newWeaponPart.range > equippedWeaponPart.range)
			newPart.rangeStatUI.statBackground.color = newPart.rangeStatUI.positiveColor;
		else if (newWeaponPart.range < equippedWeaponPart.range)
			newPart.rangeStatUI.statBackground.color = newPart.rangeStatUI.negativeColor;
	}
}
