using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackVFX : MonoBehaviour
{
    private PlayerAbilities _playerAbility;

    void Start()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
        _playerAbility = gameObject.GetComponentInParent<PlayerAbilities>();
    }

    private void Update()
    {
        if(!_playerAbility.IsUsingJetpack)
        {
            gameObject.GetComponent<ParticleSystem>().Stop();
            StartCoroutine(DestroyPS());
        }
    }

    private IEnumerator DestroyPS()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
