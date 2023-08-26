using UnityEngine;

public class ComparisonUI : MonoBehaviour
{
	public WeaponPart CurrentSelected =>
		_selectionContainer.childCount > 0 ?
		_selectionContainer.GetChild(0)?.GetComponent<ToolTipUI>().weaponPart :
		null;


	[SerializeField]
	private Transform _selectionContainer;
	[SerializeField]
	private Transform _equippedContainer;
	[SerializeField]
	private Transform _shopItemContainer;

	public void Compare()
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
