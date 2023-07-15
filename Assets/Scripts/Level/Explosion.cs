using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private int _explosionDamage = 50;
    [SerializeField] private float _explosionRadius;

    [SerializeField] private bool _explosionTriggered;

    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _parent;

    // Possibility to have DamageType?

    private void Start()
    {
        _explosionRadius = transform.localScale.x;
    }

    public void StartTimer()
    {
        if (!_explosionTriggered)
        {
            _animator.SetTrigger("ExplosionTriggered");
            _explosionTriggered = true;
        }
    }

    public void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosionRadius / 2);
        foreach (var hitCollider in hitColliders)
        {
            // Possibility to have different amounts of damage for enemies and players
            /*
            if (hitCollider.CompareTag("Player"))
            {

            }
            else if (hitCollider.CompareTag("Enemy"))
            {

            }
            */

            if (hitCollider.GetComponent<HealthComponent>() != null)
            {
                hitCollider.GetComponent<HealthComponent>().TakeDamage(_explosionDamage);
            }

            StartCoroutine(Despawn());
        }
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(0.3f);
        _parent.SetActive(false);
    }
}