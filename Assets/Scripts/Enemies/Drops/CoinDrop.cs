using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] GameObject _destroy;
    private bool _hasGivenScore = false;
    public int givenScore;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_hasGivenScore)
        {
            StartPickUpAnimation();
            _hasGivenScore = true;
        }
    }

    private void StartPickUpAnimation()
    {
        gameObject.transform.parent.gameObject.GetComponent<Collider>().enabled = false;
        AudioManager.Instance.PlaySound("Coin");
        _animator.SetBool("pickup", true);
		GameManager.Instance.inventory.Wallet.Store(givenScore);
    }

    public void Delete()
    {
        Destroy(_destroy);
    }
}
