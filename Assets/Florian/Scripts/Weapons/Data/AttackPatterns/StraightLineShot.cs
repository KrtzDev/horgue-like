using UnityEngine;

[CreateAssetMenu(fileName = "new StraightLineShot", menuName = "ModularWeapon/Data/AttackPattern/StraightLineShot")]
public class StraightLineShot : AttackPattern
{
    public override DamageDealer AttackInPattern(Projectile projectile, Transform spawnPosition)
    {
        Projectile currentProjectile = Instantiate(projectile, spawnPosition.position, spawnPosition.rotation);
        currentProjectile.GetComponent<Rigidbody>().velocity = currentProjectile.transform.forward * 18f;
        Destroy(currentProjectile, 10f);
		return currentProjectile;
    }
}
