using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    private Animator _animator;
    private Collider _collider;
    [SerializeField] private bool _isTriggered;
    private bool _destroyed;

    [SerializeField] private bool dropsLoot;

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
            DestroyObject();
        }
    }

    public void BulletHit()
    {
        _isTriggered = true;
    }

    private void SpawnLoot()
    {
        if (_loot == null)
            return;

        if(_loot.GetComponent<HealthPack_Collectible>())
        {
            if (GameManager.Instance._currentLevel - 1 < GameManager.Instance.maxLevels)
            {
                _loot.GetComponent<HealthPack_Collectible>().healAmount = GameManager.Instance.GameManagerValues[GameManager.Instance._currentLevel - 1].healthPackValue;
            }
            else
            {
                _loot.GetComponent<HealthPack_Collectible>().healAmount = GameManager.Instance.GameManagerValues[GameManager.Instance.maxLevels].healthPackValue;
            }
        }

        Vector3 _pos = _lootSpawn.transform.position;
        Quaternion _rot = _lootSpawn.transform.rotation;
        Instantiate(_loot, _pos, _rot);
    }

    public void DestroyObject()
    {
        _destroyed = true;
        _collider.enabled = false;
        _animator.SetTrigger("Destroy");

        if (dropsLoot)
            SpawnLoot();

        StartCoroutine(DespawnObject());
    }

    private IEnumerator DespawnObject()
    {
        yield return new WaitForSeconds(_despawnTimer);
        DeactivateObject();
    }

    private void DeactivateObject()
    {
        gameObject.SetActive(false);
    }
}
