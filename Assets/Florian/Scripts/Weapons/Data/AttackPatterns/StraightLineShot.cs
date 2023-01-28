using UnityEngine;

[CreateAssetMenu(fileName = "new StraightLineShot", menuName = "ModularWeapon/Data/AttackPattern/StraightLineShot")]
public class StraightLineShot : AttackPattern
{
    public override void AttackInPattern()
    {
        Debug.Log("Shot in Straight Line");
    }
}
