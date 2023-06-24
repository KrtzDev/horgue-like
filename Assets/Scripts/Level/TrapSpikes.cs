using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikes : MonoBehaviour
{
    private Animator _spikes;

    [SerializeField]
    private bool _isTriggered;

    [SerializeField]
    private bool _isAutomated;
    [SerializeField]
    private float _cooldown;
    private float _timing;

    private void Start()
    {
        _spikes = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_isAutomated)
            return;

        if (Time.time > _timing + _cooldown)
        {
            _spikes.SetTrigger("Triggered");
            _timing = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isAutomated)
            return;

        if (!_isTriggered)
        {
            _spikes.SetTrigger("Triggered");
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
