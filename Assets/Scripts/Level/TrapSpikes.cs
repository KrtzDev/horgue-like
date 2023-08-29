using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikes : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private int _spikeDamage;

    private bool _isTriggered;
    
    [Header("On Trigger")]
    [SerializeField] private float _delay;
    [SerializeField] private float _rearming;
    
    [Header("Automatic")]
    [SerializeField] private bool _isAutomated;
    [SerializeField] private float _cooldown;
    
    private float _timing;

    private Spikes _spikes;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spikes = GetComponentInChildren<Spikes>();

        _spikes._trapDamage = _spikeDamage;
    }

    private void Update()
    {
        if (!_isAutomated)
            return;

        if (Time.time > _timing + _cooldown)
        {
            _animator.SetTrigger("Triggered");
            AudioManager.Instance.PlaySound("TrapSpikes");
            _timing = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            if (_isAutomated)
                return;

            if (!_isTriggered)
            {
                StartCoroutine(ActivateTrap());
                _isTriggered = true;
            }
        }
    }

    private IEnumerator ActivateTrap()
    {
        yield return new WaitForSeconds(_cooldown);

        AudioManager.Instance.PlaySound("TrapSpikes");
        _animator.SetTrigger("Triggered");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            if (_isAutomated)
                return;

            _isTriggered = false;
        }
    }

    public void RearmTrap()
    {
        if (_isAutomated)
            return;

        StartCoroutine(TrapDelay());
    }

    private IEnumerator TrapDelay()
    {
        yield return new WaitForSeconds(_rearming);

        _isTriggered = false;
    }
}
