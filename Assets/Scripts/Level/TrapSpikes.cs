using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikes : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private int _spikeDamage;

    private bool _isTriggered;

    [Header("On Trigger")]
    [SerializeField] private GameObject _cube;
    [SerializeField] private Material _trapMaterialOnActivation;
    private Material _oldTrapMaterial;
    [SerializeField] private float _delay;
    [SerializeField] private float _rearming;
    
    [Header("Automatic")]
    [SerializeField] private bool _isAutomated;
    [SerializeField] private float _cooldown;
    
    private float _timing;

    private Spikes _spikes;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spikes = GetComponentInChildren<Spikes>();

        _spikes._trapDamage = _spikeDamage;
        _oldTrapMaterial = _cube.GetComponent<MeshRenderer>().material;

    }

    private void Update()
    {
        if (!_isAutomated)
            return;

        if(Time.time > _timing + _cooldown)
        {
            _cube.GetComponent<MeshRenderer>().material = _trapMaterialOnActivation;

            if (Time.time > _timing + _cooldown + _delay)
            {
                _animator.SetTrigger("Triggered");
                // AudioManager.Instance.PlaySound("TrapSpikes");
                _timing = Time.time;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isAutomated)
            return;

        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            if (!_isTriggered)
            {
                StartCoroutine(ActivateTrap());
                _isTriggered = true;
            }
        }
    }

    private IEnumerator ActivateTrap()
    {
        _cube.GetComponent<MeshRenderer>().material = _trapMaterialOnActivation;

        yield return new WaitForSeconds(_delay);

        // AudioManager.Instance.PlaySound("TrapSpikes");
        _animator.SetTrigger("Triggered");
    }

    public void RearmTrap()
    {
        if (_isAutomated)
            return;

        StartCoroutine(ReamingTrap());
    }

    private IEnumerator ReamingTrap()
    {
        yield return new WaitForSeconds(_rearming);

        _isTriggered = false;
    }

    private void ResetMaterial()
    {
        _cube.GetComponent<MeshRenderer>().material = _oldTrapMaterial;
    }
}
