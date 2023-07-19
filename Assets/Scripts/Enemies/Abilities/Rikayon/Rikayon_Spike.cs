using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rikayon_Spike : MonoBehaviour
{
    private AI_Agent_Rikayon _rikayon;

    [SerializeField] private Vector2 _bossStageDamageMultiplier;
    [SerializeField] private int _damageOnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            switch (_rikayon._currentBossStage)
            {
                case 0:
                    other.GetComponent<HealthComponent>().TakeDamage(_damageOnTrigger);
                    break;
                case 1:
                    other.GetComponent<HealthComponent>().TakeDamage((int)(_damageOnTrigger * _bossStageDamageMultiplier.x));
                    break;
                case 2:
                    other.GetComponent<HealthComponent>().TakeDamage((int)(_damageOnTrigger * _bossStageDamageMultiplier.y));
                    break;
            }
        }
    }

    private void Start()
    {
        _rikayon = GameObject.FindObjectOfType<AI_Agent_Rikayon>();
    }
}
