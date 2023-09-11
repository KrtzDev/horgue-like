using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Collectible : Collectible
{
    [SerializeField] private Animator _animator;
    [SerializeField] GameObject _destroy;
    [SerializeField] private Material _coinGold;
    [SerializeField] private Material _coinSilver;
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
        if(!GameManager.Instance.roundWon)
        {
            GameManager.Instance.inventory.Wallet.Store(givenScore);
            StatsTracker.Instance.coinsCollectedLevel += givenScore;
        }
        else
        {
            GameManager.Instance.inventory.Wallet.Store(givenScore / 2);
            StatsTracker.Instance.coinsCollectedEndOfRound += givenScore / 2;
        }
    }

    public void ReturnToObjectPool()
    {
        GameManager.Instance.coinPool.ReturnObjectToPool(GetComponentInParent<CollectibleAttractor>());
    }

    public void SetCoinGold()
    {
        GetComponent<MeshRenderer>().material = _coinGold;
    }

    public void SetCoinSilver()
    {
        GetComponent<MeshRenderer>().material = _coinSilver;
    }
}
