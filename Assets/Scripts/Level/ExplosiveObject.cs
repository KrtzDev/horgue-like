using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{
    private Explosion _explosion;
    [SerializeField] private bool _isTriggered;
    private bool _destroyed;

    [SerializeField] private GameObject _object;

    [SerializeField] private float _explosionTimer = 2f;

    private void Start()
    {
        _explosion = GetComponentInChildren<Explosion>();
    }

    private void Update()
    {
        /*
        if (_isTriggered && !_destroyed)
        {
            TriggerExplosive();
        }
        */
    }

    public void TriggerExplosive()
    {
        _destroyed = true;
        
        _explosion.StartTimer();

        StartCoroutine(HideObject());
    }

    private IEnumerator HideObject()
    {
        yield return new WaitForSeconds(_explosionTimer);
        _object.SetActive(false);
    }
}
