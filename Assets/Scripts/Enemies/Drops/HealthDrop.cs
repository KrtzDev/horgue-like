using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    [SerializeField] private int _healAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            HealthComponent hp = other.GetComponent<HealthComponent>();

            hp.currentHealth += _healAmount;
            if (hp.currentHealth > hp.maxHealth)
            {
                hp.currentHealth = hp.maxHealth;
            }

            AudioManager.Instance.PlaySound("HealthPack");
            Destroy(transform.parent.gameObject);
        }
    }
}
