using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Player General Ability")]
    public float _abilityCDTimer;
    public float _currentMaxCD;

    private PlayerCharacter _character;
    private PlayerMovement _playerMovement;
    private PlayerInputMappings _inputActions;

    private bool _isUsingAbility;
    private bool _buttonHeld;

    [Header("Which (one) Ability can be used?")]

    public bool _canUseDashAbility;
    public bool _canUseJetpackAbility;
    public bool _canUseEarthquakeAbility;
    public bool _canUseStealthAbility;

    [Header("Player Dash Ability")]
    [SerializeField] private GameObject _dashVisuals;
    [SerializeField] private float _dashCD;
    [SerializeField] private float _dashForce = 30;
    [SerializeField] private float _dashTime = 0.25f;

    [Header("Player Jetpack Ability")]
    [SerializeField] private GameObject _jetpackVisuals;
    [SerializeField] private float _jetpackCD;
    [Range(0.0f, 100.0f)] public float jetpackFuel;
    private float _maxJetpackFuel;
    [SerializeField] private float _energyExpansionRate;
    [SerializeField] private float _jetpackForce;
    private float _startMass;
    [SerializeField] private float _massMultiplier;
    private float _startHeight;
    private float _currentHeight;

    [Header("Player Earthquake Ability")]
    [SerializeField] private GameObject _earthquakeVisuals;
    [SerializeField] private float _earthquakeCD;

    [Header("Player Stealth Ability")]
    [SerializeField] private GameObject _decoy;
    [SerializeField] private float _stealthCD;
    [SerializeField] private float _stealthTime;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private float _movementSpeedMultiplier;

    private float _stealthMovementSpeed;
    private float _originalMovementSpeed;

    private void Awake()
    {
        _inputActions = new PlayerInputMappings();
        _inputActions.Character.Ability.started += UseAbility;
        _inputActions.Character.Ability.canceled += HoldAbility;
        _enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();

        _abilityCDTimer = -1;
    }

    private void Start()
    {
        _character = GetComponent<PlayerCharacter>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void Update()
    {
        TrackTimer();
        JetpackForce();
    }

    // General

    private void HoldAbility(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            Debug.Log("Release Button");
            _buttonHeld = false;
        }
    }

    public void ActivateVisuals()
    {
        if (_canUseDashAbility)
        {
            _dashVisuals.SetActive(true);
        }
        else if (_canUseJetpackAbility)
        {
            _jetpackVisuals.SetActive(true);
        }
        else if (_canUseEarthquakeAbility)
        {
            _earthquakeVisuals.SetActive(true);
        }
    }

    private void UseAbility(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            _buttonHeld = true;
        }

        if (ctx.started && !_isUsingAbility && GameManager.Instance._playerCanUseAbilities)
        {
            if (_canUseDashAbility && _abilityCDTimer <= 0)
            {
                DashAbility();
            }
            else if (_canUseJetpackAbility && _abilityCDTimer <= 0)
            {
                JetpackAbility();
            }
            else if (_canUseEarthquakeAbility && _abilityCDTimer <= 0)
            {
                EarthquakeAbility();
            }
            else if (_canUseStealthAbility && _abilityCDTimer <= 0)
            {
                StealthAbility();
            }
        }
    }

    private void TrackTimer()
    {
        if (_abilityCDTimer > 0) _abilityCDTimer -= Time.deltaTime;
    }

    private void ResetAbilityTimer(float cd)
    {
        _currentMaxCD = cd;
        _abilityCDTimer = cd;
    }

    // Dash

    private void DashAbility()
    {
        _playerMovement.CanMove = false;
        _isUsingAbility = true;

        Vector3 dashDir = transform.TransformDirection(Vector3.forward);
        Vector3 forceToApply = dashDir * _dashForce;

        _character.CharacterRigidbody.velocity = Vector3.zero;
        _character.CharacterRigidbody.useGravity = false;
        _character.CharacterRigidbody.AddForce(forceToApply, ForceMode.Impulse);

        StartCoroutine(StopDash());
    }

    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(_dashTime);

        ResetDashAbility();
    }

    private void ResetDashAbility()
    {
        _playerMovement.CanMove = true;
        _character.CharacterRigidbody.useGravity = true;
        _isUsingAbility = false;

        ResetAbilityTimer(_dashCD);
    }

    // Jetpack

    private void JetpackAbility()
    {
        _jetpackVisuals.GetComponentInChildren<Animator>().SetBool("jetpackOn", true);
        _jetpackVisuals.GetComponentInChildren<Animator>().SetBool("jetpackOff", false);

        _maxJetpackFuel = jetpackFuel;

        _startMass = _character.CharacterRigidbody.mass;
        _startHeight = transform.position.y;

        _isUsingAbility = true;
    }

    private void JetpackForce()
    {
        if(_canUseJetpackAbility && _isUsingAbility)
        {
            _currentHeight = transform.position.y;

            if(_currentHeight > _startHeight + 1)
            {
                if(_buttonHeld)
                {
                    _character.CharacterRigidbody.mass = _startMass * (1 + _massMultiplier *  (_currentHeight - _startHeight));
                }
                else
                {
                    _character.CharacterRigidbody.mass = _startMass;
                }
            }
        }

        if(_buttonHeld && _canUseJetpackAbility && _isUsingAbility && jetpackFuel > 0)
        {
            Vector2 _jetpackDir = new Vector2(_character.CharacterRigidbody.velocity.x, _jetpackForce);
            _character.CharacterRigidbody.AddForce(_jetpackDir, ForceMode.Force);

            jetpackFuel -= _energyExpansionRate * Time.deltaTime;
        }
        else if (jetpackFuel <= 0)
        {
            ResetJetpackAbility();
        }
    }

    private void ResetJetpackAbility()
    {
        _isUsingAbility = false;

        jetpackFuel = _maxJetpackFuel;
        _character.CharacterRigidbody.mass = _startMass;

        _jetpackVisuals.GetComponentInChildren<Animator>().SetBool("jetpackOn", false);
        _jetpackVisuals.GetComponentInChildren<Animator>().SetBool("jetpackOff", true);

        ResetAbilityTimer(_jetpackCD);
    }

    // Earthquake

    private void EarthquakeAbility()
    {

    }

    private void ResetEarthquakeAbility()
    {
        ResetAbilityTimer(_earthquakeCD);
    }

    // Stealth

    private void StealthAbility()
    {
        _isUsingAbility = true;

        _decoy.transform.parent = null;
        _decoy.GetComponent<CapsuleCollider>().enabled = true;
        _decoy.transform.position = this.transform.position;
        _decoy.transform.rotation = this.transform.rotation;
        _decoy.transform.GetChild(0).gameObject.SetActive(true);

        _originalMovementSpeed = _playerMovement.MovementSpeed;
        _stealthMovementSpeed = _originalMovementSpeed * _movementSpeedMultiplier;
        _playerMovement.MovementSpeed = _stealthMovementSpeed;

        if(_enemySpawner._enemyObjectPoolParent.transform.childCount != 0)
        {
            for (int i = 0; i < _enemySpawner._enemyObjectPoolParent.transform.childCount; i++)
            {
                for (int j = 0; j < _enemySpawner._enemyObjectPoolParent.transform.GetChild(i).childCount; j++)
                {
                    // _EnemySpawner.transform.GetChild(i).GetChild(j).GetComponent<EnemyMovement>().PlayerTarget = _Decoy.transform;
                    _enemySpawner._enemyObjectPoolParent.transform.GetChild(i).GetChild(j).GetComponent<AI_Agent_Enemy>()._followDecoy = true;
                }
            }
        }

        StartCoroutine(StopStealth());
    }

    private IEnumerator StopStealth()
    {
        yield return new WaitForSeconds(_stealthTime);

        ResetStealthbility();
    }

    private void ResetStealthbility()
    {
        _isUsingAbility = false;

        for (int i = 0; i < _enemySpawner._enemyObjectPoolParent.transform.childCount; i++)
        {
            for (int j = 0; j < _enemySpawner._enemyObjectPoolParent.transform.GetChild(i).childCount; j++)
            {
                // _EnemySpawner.transform.GetChild(i).GetChild(j).GetComponent<EnemyMovement>().PlayerTarget = this.transform;
                _enemySpawner._enemyObjectPoolParent.transform.GetChild(i).GetChild(j).GetComponent<AI_Agent_Enemy>()._followDecoy = false;
            }
        }

        _playerMovement.MovementSpeed = _originalMovementSpeed;

        _decoy.transform.GetChild(0).gameObject.SetActive(false);
        _decoy.transform.position = new Vector3(0, 0, 0);
        _decoy.transform.rotation = Quaternion.identity;
        _decoy.GetComponent<CapsuleCollider>().enabled = false;
        _decoy.transform.parent = this.transform;

        ResetAbilityTimer(_stealthCD);
    }

}
