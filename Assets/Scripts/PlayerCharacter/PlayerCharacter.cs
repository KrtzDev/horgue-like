using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _characterRigidbody;
    [SerializeField]
    private Camera _camera;
	[SerializeField]
	private Transform _weaponSpawnTransform;
    [SerializeField]
    public PlayerData _playerData;
    private HealthComponent _healthComponent;
    private PlayerInputMappings _inputActions;

    public Rigidbody CharacterRigidbody { get => _characterRigidbody; set => _characterRigidbody = value; }
    public Camera Camera { get => _camera; set => _camera = value; }
	public Transform WeaponSpawnTransform { get => _weaponSpawnTransform; set => _weaponSpawnTransform = value; }

    private float _waterDamageTimer;

    private void Awake()
    {
        _healthComponent = this.GetComponent<HealthComponent>();
        _healthComponent._maxHealth = _playerData._maxHealth;
        _healthComponent._currentHealth = _healthComponent._maxHealth;

        _inputActions = InputManager.Instance?.CharacterInputActions;
        _inputActions.Character.Pause.performed += PauseGame;
    }

    private void Update()
    {
        if(_waterDamageTimer >= 0)
            _waterDamageTimer -= Time.deltaTime;
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.CompareTag("Wasser") && _waterDamageTimer <= 0)
        {
            this.GetComponent<HealthComponent>().TakeDamage(1);
            _waterDamageTimer = 0.5f;
        }

    }

    private void PauseGame(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !GameManager.Instance._gameIsPaused)
        {
            UIManager.Instance.PauseMenu.gameObject.SetActive(true);
            GameManager.Instance._gameIsPaused = true;
            Time.timeScale = 0;
        }
    }
}
