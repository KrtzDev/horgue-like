using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    [SerializeField]
    private AudioSource _collectSound;

    private void PlaySound()
    {
        _collectSound.Play();

    }

    private void DeleteGameObject()
    {
        Destroy(transform.parent.gameObject);
    }
}
