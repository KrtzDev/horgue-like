using UnityEngine;

[CreateAssetMenu(fileName = "new StraightLineShot", menuName = "ModularWeapon/Data/AttackPattern/StraightLineShot")]
public class StraightLineShot : AttackPattern
{
	public override string PatternName()
	{
		return "StraightLine";
	}

	public override void AttackInPattern(Projectile projectile)
    {
		projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * 18f;
    }

}
