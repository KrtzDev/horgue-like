using UnityEngine;
using UnityEngine.UI;

public class ToolTipUI : MonoBehaviour
{
	[SerializeField]
	private Image _image;
	[SerializeField]
	private RectTransform _statParent;
	[SerializeField]
	private StatUI _statUI_prefab;

	public void Initialize(WeaponPart weaponPartData)
	{
		_image.sprite = weaponPartData.WeaponPartUISprite;
		if (weaponPartData is Grip)
		{
			Grip grip = weaponPartData as Grip;
			StatUI currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Attack Speed: ", grip.attackSpeed.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Reload Time: ", grip.cooldown.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Crit Chance: ", grip.critChance.ToString());
		}
		else if (weaponPartData is Barrel)
		{
			Barrel barrel = weaponPartData as Barrel;
			StatUI currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Damage: ", barrel.baseDamage.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Attack Speed: ", barrel.attackSpeed.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Reload Time: ", barrel.cooldown.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Projectile Size: ", barrel.projectileSize.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Crit Chance: ", barrel.critChance.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Range: ", barrel.range.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Projectile trajectory: ", barrel.attackPattern.name.ToString());
		}
		else if (weaponPartData is Magazine)
		{
			Magazine mag = weaponPartData as Magazine;
			StatUI currentStat = Instantiate(_statUI_prefab,_statParent);
			currentStat.Initialize("Attack Speed: ",mag.attackSpeed.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Reload Time: ", mag.cooldown.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Projectile Size: ", mag.projectileSize.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Capacity: ", mag.capacity.ToString());
		}
		else if (weaponPartData is Ammunition)
		{
			Ammunition ammunition = weaponPartData as Ammunition;
			StatUI currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Damage: ", ammunition.baseDamage.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Attack Speed: ", ammunition.attackSpeed.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Reload Time: ", ammunition.cooldown.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Projectile Size: ", ammunition.projectileSize.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Crit Chance: ", ammunition.critChance.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Range: ", ammunition.range.ToString());
		}
		else if (weaponPartData is TriggerMechanism)
		{
			TriggerMechanism trigger = weaponPartData as TriggerMechanism;
			StatUI currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Damage: ", trigger.baseDamage.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Attack Speed: ", trigger.attackSpeed.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Reload Time: ", trigger.cooldown.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Crit Chance: ", trigger.critChance.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Range: ", trigger.range.ToString());
		}
		else if (weaponPartData is Sight)
		{
			Sight sight = weaponPartData as Sight;
			StatUI currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Attack Speed: ", sight.attackSpeed.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Crit Chance: ", sight.critChance.ToString());
			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Range: ", sight.range.ToString());
		}
	}
}