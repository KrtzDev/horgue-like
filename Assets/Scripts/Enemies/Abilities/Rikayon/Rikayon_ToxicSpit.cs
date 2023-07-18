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

    GameObject _player;
    ParticleSystem ps;
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

    private void Start()
    {
        _rikayon = GameObject.FindObjectOfType<AI_Agent_Rikayon>();
        _player = GameObject.FindGameObjectWithTag("Player");
        ps = GetComponent<ParticleSystem>();
        ps.trigger.AddCollider(_player.GetComponent<Collider>());

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

    private void OnParticleTrigger()
    {
        // get the particles which matched the trigger conditions this frame
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        // iterate through the particles which entered the trigger and make them red
        for (int i = 0; i < numInside; i++)
        {
            ParticleSystem.Particle p = inside[i];
            p.startColor = new Color32(255, 0, 0, 255);
            inside[i] = p;
            Debug.Log(numInside);
        }

        // re-assign the modified particles back into the particle system
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        for (int i = 0; i < ps.trigger.colliderCount; i++)
        {
            if (ps.trigger.GetCollider(i).gameObject.CompareTag("Player"))
            {
                _player = ps.trigger.GetCollider(i).gameObject;
                Debug.Log("Player entered Trigger");

                if (_damageTimer < 0)
                {
                    switch (_rikayon._currentBossStage)
                    {
                        case 0:
                            ps.trigger.GetCollider(0).GetComponent<HealthComponent>().TakeDamage(_damagePerTick);
                            _damageTimer = _damageTime;
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                    }
                }
                return;
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
