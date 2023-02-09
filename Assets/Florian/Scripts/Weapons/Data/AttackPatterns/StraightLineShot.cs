using UnityEngine;

[CreateAssetMenu(fileName = "new StraightLineShot", menuName = "ModularWeapon/Data/AttackPattern/StraightLineShot")]
public class StraightLineShot : AttackPattern
{
    public override void AttackInPattern(Projectile projectile, Transform spawnPosition)
    {
        Projectile currentProjectile = Instantiate(projectile, spawnPosition.position, spawnPosition.rotation);
        currentProjectile.GetComponent<Rigidbody>().velocity = currentProjectile.transform.forward * 12f;
        Destroy(currentProjectile, 10f);
    }
}
