using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    [SerializeField] private Animator _animator;
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
        gameObject.transform.parent.gameObject.GetComponent<Collider>().enabled = false;
        AudioManager.Instance.PlaySound("Coin");
        _animator.SetBool("pickup", true);
        GameManager.Instance._currentScore += _givenScore;
    }

    public void Delete()
    {
        Destroy(_destroy);
    }
}
