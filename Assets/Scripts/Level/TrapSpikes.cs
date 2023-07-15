using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikes : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private int _spikeDamage;

    [SerializeField] private bool _isTriggered;

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
            _timing = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isAutomated)
            return;

        if (!_isTriggered)
        {
            _animator.SetTrigger("Triggered");
            _isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isAutomated)
            return;

        _isTriggered = false;
    }
}
