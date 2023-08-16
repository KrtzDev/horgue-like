using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipUI : Selectable
{
	public event Action<ToolTipUI> OnBuy;
	public event Action<WeaponPart> OnSelected;
	public event Action OnDeselected;

	[SerializeField]
	public HoldButton buyButton;

	[SerializeField]
	private Image _selectionIndicator;

	[SerializeField]
	private Image _partImage;
	[SerializeField]
	private LayoutGroup _layoutGroup;
	[SerializeField]
	private RectTransform _statParent;
	[SerializeField]
	private StatUI _statUI_prefab;

	public WeaponPart weaponPart;
	private int _value;

	private bool _selected;

	public void Initialize(WeaponPart weaponPartData)
	{
		_partImage.sprite = weaponPartData.WeaponPartUISprite;
		weaponPart = weaponPartData;
		_value = weaponPartData.value;

		InitializeStats(weaponPartData);
		LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup.transform as RectTransform);	

		if(buyButton)
			buyButton.OnButtonExecute += Buy;
	}

	private void InitializeStats(WeaponPart weaponPartData)
	{
		StatUI currentStat = Instantiate(_statUI_prefab, _statParent);
		currentStat.Initialize("Damage: ", weaponPartData.baseDamage.ToString());
		currentStat = Instantiate(_statUI_prefab, _statParent);
		currentStat.Initialize("Attack Speed: ", weaponPartData.attackSpeed.ToString());
		currentStat = Instantiate(_statUI_prefab, _statParent);
		currentStat.Initialize("Reload Time: ", weaponPartData.cooldown.ToString());
		currentStat = Instantiate(_statUI_prefab, _statParent);
		currentStat.Initialize("Projectile Size: ", weaponPartData.projectileSize.ToString());
		currentStat = Instantiate(_statUI_prefab, _statParent);
		currentStat.Initialize("Crit Chance: ", weaponPartData.critChance.ToString());
		currentStat = Instantiate(_statUI_prefab, _statParent);
		currentStat.Initialize("Range: ", weaponPartData.range.ToString());

		if (weaponPartData is Barrel)
		{
			Barrel barrel = weaponPartData as Barrel;

			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Projectile trajectory: ", barrel.attackPattern.PatternName());
		}
		else if (weaponPartData is Magazine)
		{
			Magazine mag = weaponPartData as Magazine;

			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Capacity: ", mag.capacity.ToString());
		}
		else if (weaponPartData is Ammunition)
		{
			Ammunition ammunition = weaponPartData as Ammunition;

			if (ammunition.statusEffect != null)
			{
				currentStat = Instantiate(_statUI_prefab, _statParent);
				currentStat.Initialize("Impact Effect", ammunition.statusEffect.StatusName());
			}
		}
	}

	private void Buy()
	{
		if (!_selected)
			return;

		GameManager.Instance.inventory.Wallet.TryPay(_value);

		OnBuy.Invoke(this);
		buyButton.OnButtonExecute -= Buy;

		Destroy(gameObject);
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		OnSelected.Invoke(weaponPart);

		_selectionIndicator.enabled = true;
		_selected = true;
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		OnDeselected.Invoke();

		_selectionIndicator.enabled = false;
		_selected = false;
	}
}