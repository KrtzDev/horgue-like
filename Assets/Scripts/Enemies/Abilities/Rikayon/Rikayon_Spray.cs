using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rikayon_Spray : MonoBehaviour
{
    private AI_Agent_Rikayon _rikayon;

    private float _damageTimer;
    [SerializeField] private float _damageTime;
    [SerializeField] private int _damagePerTick;

    GameObject _player;
    ParticleSystem ps;

    private void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> insideList = new List<ParticleSystem.Particle>();
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, insideList);

        Debug.Log(numInside);

        if (numInside > 0)
        {
            if (_damageTimer < 0)
            {
                switch (_rikayon._currentBossStage)
                {
                    case 0:
                        ps.trigger.GetCollider(0).GetComponentInParent<HealthComponent>().TakeDamage(_damagePerTick);
                        _damageTimer = _damageTime;
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                }
            }
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, insideList);
    }

    private void Start()
    {
        _rikayon = GameObject.FindObjectOfType<AI_Agent_Rikayon>();
        ps = GetComponent<ParticleSystem>();
        _player = GameObject.FindWithTag("Player");
        ps.trigger.AddCollider(_player.GetComponentInChildren<BoxCollider>());
        _damageTimer = 0;
    }

    private void Update()
    {
        if(_damageTimer >= 0)
            _damageTimer -= Time.deltaTime;
    }
}
