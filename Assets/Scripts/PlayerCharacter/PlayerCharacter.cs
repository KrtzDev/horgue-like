using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _characterRigidbody;
    [SerializeField]
    private Camera _camera;
	[SerializeField]
	private Transform _weaponSpawnTransform;
	[SerializeField]
	private Inventory _inventory;
    [SerializeField]
    public PlayerData playerData;
    [HideInInspector] public HealthComponent healthComponent;
    private PlayerInputMappings _inputActions;

    public Rigidbody CharacterRigidbody { get => _characterRigidbody; set => _characterRigidbody = value; }
    public Camera Camera { get => _camera; set => _camera = value; }
	public Transform WeaponSpawnTransform { get => _weaponSpawnTransform; set => _weaponSpawnTransform = value; }
	public Inventory Inventory { get => _inventory; set => _inventory = value; }

    private float _waterDamageTimer;

    public LayerMask WalkLayer;
    public LayerMask GroundLayer;
    public LayerMask EnemyLayer;

    private void Awake()
    {
        healthComponent = this.GetComponent<HealthComponent>();
        healthComponent.maxHealth = (int)(playerData.maxHealth * (1 + (playerData.levelMultiplier * GameManager.Instance._currentLevelArray)));
        healthComponent.currentHealth = healthComponent.maxHealth;

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
        if (ctx.performed && !GameManager.Instance._gameIsPaused && SceneManager.GetActiveScene().name.StartsWith("SCENE_Level"))
        {
            UIManager.Instance.PauseMenu.gameObject.SetActive(true);
            GameManager.Instance._gameIsPaused = true;
            Time.timeScale = 0;
        }
    }
}
