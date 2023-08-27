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

            hp._currentHealth += _healAmount;
            if(hp._currentHealth > hp._maxHealth)
            {
                hp._currentHealth = hp._maxHealth;
            }

            AudioManager.Instance.PlaySound("HealthPack");
            Destroy(transform.parent.gameObject);
        }
    }
}
