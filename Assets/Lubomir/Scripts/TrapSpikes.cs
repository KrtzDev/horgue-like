using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikes : MonoBehaviour
{
    private Animator _spikes;

    [SerializeField]
    private bool _isTriggered;

    private void Start()
    {
        _spikes = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isTriggered)
        {
            _spikes.SetTrigger("Triggered");
            _isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isTriggered = false;
    }
}
