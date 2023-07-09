using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _collectSound;
    [SerializeField] GameObject _destroy;
    public int _givenScore;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartPickUpAnimation();
        }
    }

    private void StartPickUpAnimation()
    {
        _collectSound.Play();
        _animator.SetBool("pickup", true);
        GameManager.Instance._currentScore += _givenScore;
    }

    public void Delete()
    {
        Destroy(_destroy);
    }
}
