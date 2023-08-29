using System;
using System.Collections;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
	public event Action<float> OnHealthPercentChanged;
	public event Action OnDeath;

	public int maxHealth;
	public int currentHealth;
	[HideInInspector] public bool canTakeDamage;
	[HideInInspector] public bool isDead = false;

    [SerializeField] protected Material _damageFlashMaterial;
    [SerializeField] protected float _damageFlashTime;
    protected Material[] _originalMaterials;
    protected SkinnedMeshRenderer[] _skinnedMeshes;
    protected MeshRenderer[] _meshes;
    protected bool _isDamageFlashing;

    private float _playerDamageBlinkTimer;

    protected virtual void Awake()
	{
		canTakeDamage = true;
		isDead = false;

        DeclareMeshes();
    }

    private void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            if(!canTakeDamage)
            {
                _playerDamageBlinkTimer -= Time.deltaTime;
            }
        }
    }

    public virtual void TakeDamage(int damage, bool damageOverTime)
	{
		if(canTakeDamage)
        {
			currentHealth -= damage;
			float currentHealthPct = (float)currentHealth / maxHealth;
			OnHealthPercentChanged?.Invoke(currentHealthPct);

            if (gameObject.CompareTag("Player"))
            {
                FindObjectOfType<UIDamageFlash>().DamageFlash(0.25f, .4f, .2f);

                if (currentHealth <= 0 && !isDead)
                {
                    GameManager.Instance.PlayerDied();
                    isDead = true;
                    canTakeDamage = false;
                    return;
                }
                else
                {
                    if (!damageOverTime)
                    {
                        _playerDamageBlinkTimer = gameObject.GetComponent<PlayerCharacter>().playerData.invincibilityTime;

                        canTakeDamage = false;
                        StartCoroutine(PlayerCanTakeDamage());
                    }
                }
            }
            else
            {
                if (currentHealth <= 0 && !isDead)
                {
                    OnDeath?.Invoke();
                }
            }

            DamageFlash();
        }
	}

    private IEnumerator PlayerCanTakeDamage()
    {
        yield return new WaitForSeconds(gameObject.GetComponent<PlayerCharacter>().playerData.invincibilityTime);

        canTakeDamage = true;
    }

    public virtual void DamageFlash()
    {
        if (_isDamageFlashing)
            return;

        if (GetComponentInChildren<MeshRenderer>() != null)
        {
            for (int i = 0; i < _meshes.Length; i++)
            {
                _originalMaterials[i] = _meshes[i].material;
                _meshes[i].material = _damageFlashMaterial;
            }
        }

        _isDamageFlashing = true;

        StartCoroutine(StopDamageFlash());
    }

    protected IEnumerator StopDamageFlash()
    {
        yield return new WaitForSeconds(_damageFlashTime);

        if (GetComponentInChildren<SkinnedMeshRenderer>() != null)
        {
            for (int i = 0; i < _skinnedMeshes.Length; i++)
            {
                _skinnedMeshes[i].material = _originalMaterials[i];
            }
        }
        else if (GetComponentInChildren<MeshRenderer>() != null)
        {
            for (int i = 0; i < _meshes.Length; i++)
            {
                _meshes[i].material = _originalMaterials[i];
            }
        }

        yield return new WaitForSeconds(_damageFlashTime);

        _isDamageFlashing = false;

        if (gameObject.CompareTag("Player"))
        {
            if (_playerDamageBlinkTimer > 0)
            {
                DamageFlash();
            }
        }
    }

    private void DeclareMeshes()
    {
        if (GetComponentInChildren<SkinnedMeshRenderer>() != null)
        {
            _skinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
            _originalMaterials = new Material[_skinnedMeshes.Length];
        }
        else
        {
            _meshes = GetComponentsInChildren<MeshRenderer>();
            _originalMaterials = new Material[_meshes.Length];
        }

        _isDamageFlashing = false;
    }
}
