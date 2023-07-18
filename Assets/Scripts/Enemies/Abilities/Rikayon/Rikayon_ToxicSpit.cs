using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rikayon_ToxicSpit : MonoBehaviour
{
    private AI_Agent_Rikayon _rikayon;

    [SerializeField] private float _lifeTime;
    private float _lifeTimer;
    [SerializeField] private float _damageTime;
    private float _damageTimer;
    [SerializeField] private int _damagePerTick;

    private void Start()
    {
        _rikayon = GameObject.FindObjectOfType<AI_Agent_Rikayon>();

        _damageTimer = 0;
        _lifeTimer = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _damageTimer < 0)
        {
            switch (_rikayon._currentBossStage)
            {
                case 0:
                    other.GetComponent<HealthComponent>().TakeDamage(_damagePerTick);
                    _damageTimer = _damageTime;
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }
    }

    private void Update()
    {
        if (_damageTimer >= 0)
            _damageTimer -= Time.deltaTime;

        if (_lifeTimer <= _lifeTime)
        {
            _lifeTimer += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
