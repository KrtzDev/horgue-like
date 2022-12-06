using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ModularWeapon/WeaponType")]
public class WeaponBehaviour : ScriptableObject
{
    [Header("Slots")]
    [SerializeField]
    private List<Pattern> _patterns = new List<Pattern>();


    private Pattern _attackPattern;



    public void Attack()
    {
        foreach (Pattern pattern in _patterns)
        {
            _attackPattern = pattern;
        }

        _attackPattern.AttackInPattern();
    }
}
