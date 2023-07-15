using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    [SerializeField] private bool _switchA;
    [SerializeField] private bool _switchB;

    [SerializeField] private GameObject _connectedObject;

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
