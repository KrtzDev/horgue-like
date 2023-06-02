using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    public int HealthAmount;
    public LayerMask _groundLayer;
    [SerializeField]
    private AudioSource _collectSound;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            HealthComponent hp = other.GetComponent<HealthComponent>();

            hp.CurrentHealth += HealthAmount;
            if(hp.CurrentHealth > hp.MaxHealth)
            {
                hp.CurrentHealth = hp.MaxHealth;
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
