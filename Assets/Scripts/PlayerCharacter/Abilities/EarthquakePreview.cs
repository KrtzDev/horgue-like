using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakePreview : MonoBehaviour
{
    private PlayerAbilities _playerAbilities;
    private float _activeTimer;

    private void Start()
    {
        _playerAbilities = FindObjectOfType<PlayerAbilities>();
        _activeTimer = 0;
    }

    private void Update()
    {
        _activeTimer += Time.deltaTime;

        if(_activeTimer > _playerAbilities._earthquakeLoadUpTime)
        {
            StartCoroutine(Destroy_WaitNextFrame());
        }
    }

    private IEnumerator Destroy_WaitNextFrame()
    {
        yield return new WaitForEndOfFrame();

        Destroy(gameObject);
    }
}
