using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ModularWeapon/Pattern")]
public class Pattern : ScriptableObject, IPattern
{
    [SerializeField]
    public List<Transform> spawnPositions = new List<Transform>();

    public void AttackInPattern()
    {
        Debug.Log("Pattern" + spawnPositions.Count);
    }
}
