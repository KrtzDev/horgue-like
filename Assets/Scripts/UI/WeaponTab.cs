using System;

public class WeaponTab : UIButton
{
	public event Action<Weapon> OnWeaponTabSelect;

	public Weapon AssociatedWeapon { get; set; }

	private void OnEnable()
	{
		OnButtonSelect += () => OnWeaponTabSelect(AssociatedWeapon);
	}

	private void OnDisable()
	{
		OnButtonSelect -= () => OnWeaponTabSelect(AssociatedWeapon);
	}

}
