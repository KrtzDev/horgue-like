using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIWeapon : MonoBehaviour
{
    [SerializeField] private Image _weapon1Icon;
    [SerializeField] private Image _weapon2Icon;

    [SerializeField] private TextMeshProUGUI _weapon1Bullets;
    [SerializeField] private TextMeshProUGUI _weapon2Bullets;

    private int _maxCapacityWeapon1;
    private int _maxCapacityWeapon2;

    private WeaponHolster _weaponHolster;

    private void Start() 
    {
        _weaponHolster = FindObjectOfType<WeaponHolster>();
        _maxCapacityWeapon1 = _weaponHolster.weapons[0]._capacity;
        _maxCapacityWeapon2 = _weaponHolster.weapons[1]._capacity;

        _weapon1Bullets.text = _weaponHolster.weapons[0]._capacity + " / " + _maxCapacityWeapon1;
        _weapon2Bullets.text = _weaponHolster.weapons[1]._capacity + " / " + _maxCapacityWeapon2;
    }

    private void Update() 
    {
        _weapon1Bullets.text = _weaponHolster.weapons[0]._capacity + " / " + _maxCapacityWeapon1;
        _weapon2Bullets.text = _weaponHolster.weapons[1]._capacity + " / " + _maxCapacityWeapon2;    

        if(_weaponHolster.weapons[0]._isReloading)
        {
            StartCoroutine(ReloadWeapon1(_weaponHolster.weapons[0]._reloadTime));
        }

        if (_weaponHolster.weapons[1]._isReloading)
        {
            StartCoroutine(ReloadWeapon2(_weaponHolster.weapons[1]._reloadTime));
        }
    }

    private IEnumerator ReloadWeapon1(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            _weapon1Icon.fillAmount = Mathf.Lerp(0, 1, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ReloadWeapon2(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            _weapon2Icon.fillAmount = Mathf.Lerp(0, 1, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
