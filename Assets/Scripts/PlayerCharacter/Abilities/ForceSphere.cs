using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceSphere : MonoBehaviour
{
    private PlayerAbilities _playerAbilities;
    private PlayerCharacter _playerCharacter;
    private MeshRenderer _meshRenderer;

    private float _scaleModifier = 1;
    private float _timer = 0;

    private float _waitTime = 1f;
    private bool _isActive = true;

    private void Start()
    {
        _playerAbilities = FindObjectOfType<PlayerAbilities>();
        _playerCharacter = FindObjectOfType<PlayerCharacter>();
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();

        _isActive = true;

        StartCoroutine(LerpScale());
        StartCoroutine(LerpColor());
    }

    private void Update()
    {
        if(_timer > _playerAbilities.ForceSphereDuration && _isActive)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, _scaleModifier, _playerCharacter.EnemyLayer);
            foreach (Collider enemy in enemies)
            {
                Vector3 direction = (enemy.transform.position - transform.position) / (enemy.transform.position - transform.position).magnitude;
                direction = new Vector3(direction.x, 0, direction.y);

                enemy.GetComponent<NavMeshAgent>().enabled = false;
                enemy.GetComponent<Rigidbody>().AddForce(direction * _playerAbilities.ForceSphereForce, ForceMode.Impulse);
                StartCoroutine(EnableEnemyAfterKnockback(enemy));
            }

            _playerAbilities.ResetForceSphereAbility();
            _isActive = false;
            _meshRenderer.enabled = false;
            StartCoroutine(Destroy());
        }
        else if(_isActive)
        {
            _timer += Time.deltaTime;

        }
    }

    private IEnumerator LerpScale()
    {
        float startValue = _playerAbilities.ForceSphereStartRadius;
        Vector3 startScale = transform.localScale;
        while (_timer < _playerAbilities.ForceSphereDuration)
        {
            _scaleModifier = Mathf.Lerp(startValue, _playerAbilities.ForceSphereEndRadius, _timer / _playerAbilities.ForceSphereDuration);
            transform.localScale = startScale * _scaleModifier;
            yield return null;
        }

        transform.localScale = startScale * _playerAbilities.ForceSphereEndRadius;
        _scaleModifier = _playerAbilities.ForceSphereEndRadius;
    }

    private IEnumerator LerpColor()
    {
        _meshRenderer.material.color = new Color(_meshRenderer.material.color.r, _meshRenderer.material.color.g, _meshRenderer.material.color.b, _playerAbilities.ForceSphereStartTransparency);
        Color startValue = _meshRenderer.material.color;
        while(_timer < _playerAbilities.ForceSphereDuration)
        {
            _meshRenderer.material.color = Color.Lerp(startValue, new Color(_meshRenderer.material.color.r, _meshRenderer.material.color.g, _meshRenderer.material.color.b, _playerAbilities.ForceSphereEndTransparency), _timer / _playerAbilities.ForceSphereDuration);
            yield return null;
        }
        _meshRenderer.material.color = new Color(_meshRenderer.material.color.r, _meshRenderer.material.color.g, _meshRenderer.material.color.b, _playerAbilities.ForceSphereEndTransparency);
    }

    private IEnumerator EnableEnemyAfterKnockback(Collider enemy)
    {
        yield return new WaitForSeconds(_waitTime);

        enemy.GetComponent<Rigidbody>().velocity = new Vector3(0, enemy.GetComponent<Rigidbody>().velocity.y, 0);

        NavMeshHit nv_hit;

        if (NavMesh.SamplePosition(enemy.gameObject.transform.position, out nv_hit, 2f, NavMesh.AllAreas))
        {
            enemy.GetComponent<NavMeshAgent>().Warp(nv_hit.position);
            enemy.GetComponent<NavMeshAgent>().enabled = true;       
        }
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(_waitTime + 1);

        Destroy(gameObject);
    }
}
