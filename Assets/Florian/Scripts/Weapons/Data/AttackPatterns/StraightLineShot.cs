using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new StraightLineShot", menuName = "ModularWeapon/Data/AttackPattern/StraightLineShot")]
public class StraightLineShot : AttackPattern
{
    public override void AttackInPattern(Projectile projectile, Transform spawnPosition)
    {
        Instantiate(projectile,spawnPosition.position,spawnPosition.rotation);
    }
}
