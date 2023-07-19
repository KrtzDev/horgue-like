using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private bool _isOpened;

    private bool playerInRange;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && playerInRange && !_isOpened)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        _isOpened = true;

        _animator.SetTrigger("Open");
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
