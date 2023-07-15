using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableCrate : MonoBehaviour
{
    private Animator _animator;
    private Collider _collider;
    [SerializeField] private bool _isTriggered;
    private bool _destroyed;

    [SerializeField] private GameObject _loot;
    [SerializeField] private Transform _lootSpawn;

    [SerializeField] private float _despawnTimer = 2f;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (_isTriggered && !_destroyed)
        {
            DestroyCrate();
        }
    }

    private void SpawnLoot()
    {
        if (_loot == null)
            return;

        Vector3 _pos = _lootSpawn.transform.position;
        Quaternion _rot = _lootSpawn.transform.rotation;
        Instantiate(_loot, _pos, _rot);
    }

    private void DestroyCrate()
    {
        _destroyed = true;
        _collider.enabled = false;
        _animator.SetTrigger("Destroy");
        SpawnLoot();

        StartCoroutine(DespawnCrate());
    }

    private IEnumerator DespawnCrate()
    {
        yield return new WaitForSeconds(_despawnTimer);
        DeactivateCrate();
    }

    private void DeactivateCrate()
    {
        gameObject.SetActive(false);
    }
}
