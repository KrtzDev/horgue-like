using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : MonoBehaviour
{
    private PlayerAbilities _playerAbilities;
    private float _activeTimer;

    private void Start()
    {
        _playerAbilities = FindObjectOfType<PlayerAbilities>();
        _activeTimer = 0;
    }

    private void Update()
    {
        _activeTimer += Time.deltaTime;

        if (_activeTimer > _playerAbilities._earthquakeActiveTime)
        {
            Destroy(gameObject);
        }
    }
}
