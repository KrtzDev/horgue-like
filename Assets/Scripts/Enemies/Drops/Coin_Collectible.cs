using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Collectible : Collectible
{
    [SerializeField] private Animator _animator;
    [SerializeField] GameObject _destroy;
    public int givenScore;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_wasPickedUp)
        {
            StartPickUpAnimation();
            _wasPickedUp = true;
        }
    }

    private void StartPickUpAnimation()
    {
        gameObject.transform.parent.gameObject.GetComponent<Collider>().enabled = false;
        AudioManager.Instance.PlaySound("Coin");
        _animator.SetBool("pickup", true);
		GameManager.Instance.inventory.Wallet.Store(givenScore);
        StatsTracker.Instance.scoreCollectedLevel += givenScore;
    }

    public void Delete()
    {
        Destroy(_destroy);
    }
}
