using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    [SerializeField] private int _healAmount;
    [SerializeField] private AudioSource _collectSound;

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

            StartCoroutine(DeleteGameObject());
        }
    }

    IEnumerator DeleteGameObject()
    {
        _collectSound.Play();
        gameObject.SetActive(false);

        yield return new WaitForSeconds(_collectSound.clip.length);

        Destroy(transform.parent.gameObject);
    }
}
