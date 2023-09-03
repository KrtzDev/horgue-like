using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashVFX : MonoBehaviour
{
    private PlayerAbilities _playerAbility;

    void Start()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
        _playerAbility = gameObject.GetComponentInParent<PlayerAbilities>();
    }

    private void Update()
    {
        if (!_playerAbility.IsUsingAbility)
        {
            Destroy(gameObject);
        }
    }
}
