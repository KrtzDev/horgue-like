using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _characterRigidbody;
    [SerializeField]
    private Camera _camera;
	[SerializeField]
	private Transform _WeaponSpawnTransform;
    [SerializeField]
    public PlayerData _playerData;
    private HealthComponent _healthComponent;

    public Rigidbody CharacterRigidbody { get => _characterRigidbody; set => _characterRigidbody = value; }
    public Camera Camera { get => _camera; set => _camera = value; }
	public Transform WeaponSpawnTransform { get => _WeaponSpawnTransform; set => _WeaponSpawnTransform = value; }

    private float waterDamageTimer;

    private void Awake()
    {
        _healthComponent = this.GetComponent<HealthComponent>();
        _healthComponent.MaxHealth = _playerData._maxHealth;
        _healthComponent.CurrentHealth = _healthComponent.MaxHealth;
    }

    private void Update()
    {
        if(waterDamageTimer >= 0)
            waterDamageTimer -= Time.deltaTime;
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.CompareTag("Wasser") && waterDamageTimer <= 0)
        {
            this.GetComponent<HealthComponent>().TakeDamage(1);
            waterDamageTimer = 0.5f;
        }

    }
}
