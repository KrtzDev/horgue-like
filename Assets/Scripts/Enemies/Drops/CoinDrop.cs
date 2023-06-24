using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    private void SetDeactive()
    {
        this.gameObject.SetActive(false);
    }

    private void Awake()
    {
        StartCoroutine(Delete());
    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(2);
        Destroy(transform.gameObject);
    }
}
