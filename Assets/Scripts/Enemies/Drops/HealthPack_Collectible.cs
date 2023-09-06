using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack_Collectible : Collectible
{
    [SerializeField] public int healAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !_wasPickedUp)
        {
            _wasPickedUp = true;

            HealthComponent hp = other.GetComponent<HealthComponent>();

            hp.currentHealth += healAmount;
            if (hp.currentHealth > hp.maxHealth)
            {
                hp.currentHealth = hp.maxHealth;
            }

            AudioManager.Instance.PlaySound("HealthPack");
            Destroy(transform.parent.gameObject);
        }
    }
}
