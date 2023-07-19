using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rikayon_ToxicSpit : MonoBehaviour
{
    private AI_Agent_Rikayon _rikayon;

    [SerializeField] private Vector2 _bossStageLifeTimeMultiplier;
    [SerializeField] private float _lifeTime;
    private float _lifeTimer;
    [SerializeField] private Vector2 _bossStageDamageMultiplier;
    [SerializeField] private float _damageTime;
    private float _damageTimer;
    [SerializeField] private int _damagePerTick;

    GameObject _player;
    ParticleSystem ps;

    private void Start()
    {
        _rikayon = GameObject.FindObjectOfType<AI_Agent_Rikayon>();
        ps = GetComponent<ParticleSystem>();
        _player = GameObject.FindWithTag("Player");
        ps.trigger.AddCollider(_player.GetComponent<BoxCollider>());

        _damageTimer = 0;
        _lifeTimer = 0;

        switch (_rikayon._currentBossStage)
        {
            case 1:
                _lifeTime *= _bossStageLifeTimeMultiplier.x;
                break;
            case 2:
                _lifeTime *= _bossStageLifeTimeMultiplier.y;
                break;
        }
    }

    private void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> insideList = new List<ParticleSystem.Particle>();
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, insideList);

        if (numInside > 0)
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

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, insideList);
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
