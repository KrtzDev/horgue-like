using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikes : MonoBehaviour
{
    [SerializeField]
    private Animator spikes;

    [SerializeField]
    private bool isTriggered;

    private void Start()
    {
        spikes = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            spikes.SetTrigger("Triggered");
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isTriggered = false;
    }
}
