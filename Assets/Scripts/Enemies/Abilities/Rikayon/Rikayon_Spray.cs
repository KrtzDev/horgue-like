using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rikayon_Spray : MonoBehaviour
{
    private AI_Agent_Rikayon _rikayon;

    private float _damageTimer;
    [SerializeField] private float _damageTime;
    [SerializeField] private Vector2 _bossStageDamageMultiplier;
    [SerializeField] private int _damagePerTick;

    GameObject _player;
    ParticleSystem ps;

    private void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> enterList = new List<ParticleSystem.Particle>();
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterList);

        if (numEnter > 0)
        {
            if (_damageTimer < 0)
            {
                switch (_rikayon._currentBossStage)
                {
                    case 0:
                        ps.trigger.GetCollider(0).GetComponent<HealthComponent>().TakeDamage(_damagePerTick);
                        _damageTimer = _damageTime;
                        break;
                    case 1:
                        ps.trigger.GetCollider(0).GetComponent<HealthComponent>().TakeDamage((int)(_damagePerTick * _bossStageDamageMultiplier.x));
                        _damageTimer = _damageTime;
                        break;
                    case 2:
                        ps.trigger.GetCollider(0).GetComponent<HealthComponent>().TakeDamage((int)(_damagePerTick * _bossStageDamageMultiplier.y));
                        _damageTimer = _damageTime;
                        break;
                }
            }
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterList);
    }

    private void Start()
    {
        _rikayon = GameObject.FindObjectOfType<AI_Agent_Rikayon>();
        ps = GetComponent<ParticleSystem>();
        _player = GameObject.FindWithTag("Player");
        ps.trigger.AddCollider(_player.GetComponent<BoxCollider>());
        _damageTimer = 0;
    }

    private void Update()
    {
        if(_damageTimer >= 0)
            _damageTimer -= Time.deltaTime;
    }
}
