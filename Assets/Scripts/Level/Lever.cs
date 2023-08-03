using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private Animator _animator;
    
    [SerializeField] private bool _switchA;
    [SerializeField] private bool _switchB;

    [SerializeField] private AI_Agent_Sentry _connectedSentry;

    private bool playerInRange;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && playerInRange)
        {
            Switch();
        }
    }

    private void Switch()
    {
        _switchA = !_switchA;
        _switchB = !_switchB;

        _animator.SetBool("isSwitchA", _switchA);

        SwitchOutcome();
    }

    private void SwitchOutcome()
    {
        if (_switchA && !_switchB)
        {
            _connectedSentry.SwitchSentryStatus(SentryStatus.Ally);
        }
        else if (!_switchA && _switchB)
        {
            _connectedSentry.SwitchSentryStatus(SentryStatus.Enemy);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
