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

    public bool IsUsingAbility;
    public bool ButtonHeld;

    [Header("Which (one) Ability can be used?")]

    public bool _canUseDashAbility;
    public bool _canUseJetpackAbility;
    public bool _canUseEarthquakeAbility;
    public bool _canUseStealthAbility;

    [Header("Player Dash Ability")]
    [SerializeField] private GameObject _dashVisuals;
    [SerializeField] private Transform _dashEffectPosition;
    [SerializeField] private ParticleSystem _dashEffect;
    [SerializeField] private float _dashCD;
    [SerializeField] private float _dashForce = 30;
    [SerializeField] private float _dashTime = 0.25f;

    [Header("Player Jetpack Ability")]
    public bool IsUsingJetpack;
    [SerializeField] private GameObject _jetpackVisuals;
    [SerializeField] private Transform _jetpackEffectPosition;
    [SerializeField] private ParticleSystem _jetpackEffect;
    [SerializeField] private float _jetpackCD;
    [Range(0.0f, 100.0f)] public float jetpackFuel;
    public float _maxJetpackFuel;
    [SerializeField] private float _energyExpansionRate;
    [SerializeField] private float _jetpackThrustForce;
    [SerializeField] private float _jetpackHeatFallOffTime;
    [SerializeField] private float _jetpackHeatFallOffMultiplier;
    [SerializeField] private float _jetpackActiveTimer;

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
            ButtonHeld = false;
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
            ButtonHeld = true;
        }

        if (ctx.started && !IsUsingAbility && GameManager.Instance._playerCanUseAbilities)
        {
            if (_canUseDashAbility && _abilityCDTimer <= 0)
            {
                DashAbility();
            }
            else if (_canUseJetpackAbility && _abilityCDTimer <= 0)
            {
                JetpackAbility();
                JetpackParticleEffect();
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
        else if (ctx.started && IsUsingAbility && _canUseJetpackAbility && GameManager.Instance._playerCanUseAbilities)
        {
            JetpackParticleEffect();
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
        IsUsingAbility = true;

        Vector3 dashDir = transform.TransformDirection(Vector3.forward);
        Vector3 forceToApply = dashDir * _dashForce;

        _character.CharacterRigidbody.velocity = Vector3.zero;
        _character.CharacterRigidbody.useGravity = false;
        _character.CharacterRigidbody.AddForce(forceToApply, ForceMode.Impulse);

        DashParticleEffect();

        StartCoroutine(StopDash());
    }

    private void DashParticleEffect()
    {
        ParticleSystem jp_effect = _dashEffect;
        Instantiate(jp_effect, _dashEffectPosition);
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
        IsUsingAbility = false;

        ResetAbilityTimer(_dashCD);
    }

    // Jetpack

    private void JetpackAbility()
    {
        _jetpackVisuals.GetComponentInChildren<Animator>().SetBool("jetpackOn", true);
        _jetpackVisuals.GetComponentInChildren<Animator>().SetBool("jetpackOff", false);

        _maxJetpackFuel = jetpackFuel;

        IsUsingAbility = true;
    }

    private void JetpackParticleEffect()
    {
        ParticleSystem jp_effect = _jetpackEffect;
        Instantiate(jp_effect, _jetpackEffectPosition);
    }

    private void JetpackForce()
    {
        if(_canUseJetpackAbility && IsUsingAbility)
        {
            if (ButtonHeld && jetpackFuel > 0)
            {
                IsUsingJetpack = true;

                jetpackFuel -= _energyExpansionRate * Time.deltaTime;

                _jetpackActiveTimer += Time.deltaTime;

                float multiplier = 1;

                if(_jetpackActiveTimer >= _jetpackHeatFallOffTime)
                {
                    if(_jetpackActiveTimer < 1)
                    {
                        multiplier = 1 - (1 * _jetpackHeatFallOffMultiplier);
                    }
                    else
                    {
                        multiplier = 1 - (_jetpackActiveTimer * _jetpackHeatFallOffMultiplier);
                    }
                }

                _character.CharacterRigidbody.velocity = new Vector3(_character.CharacterRigidbody.velocity.x, 0, _character.CharacterRigidbody.velocity.z);
                _character.CharacterRigidbody.AddForce(_character.CharacterRigidbody.transform.up * _jetpackThrustForce * multiplier, ForceMode.Impulse);
            }
            else if (!ButtonHeld)
            {
                if(_jetpackActiveTimer > 0)
                    _jetpackActiveTimer -= 5 * Time.deltaTime;

                IsUsingJetpack = false;
            }

            if (jetpackFuel <= 0)
            {
                ResetJetpackAbility();
            }
        }
    }

    private void ResetJetpackAbility()
    {
        IsUsingJetpack = false;
        IsUsingAbility = false;
        _jetpackActiveTimer = 0;

        _jetpackVisuals.GetComponentInChildren<Animator>().SetBool("jetpackOn", false);
        _jetpackVisuals.GetComponentInChildren<Animator>().SetBool("jetpackOff", true);

        ResetAbilityTimer(_jetpackCD);
        StartCoroutine(ResetJetpackFuel());
    }

    private IEnumerator ResetJetpackFuel()
    {
        yield return new WaitForSeconds(_jetpackCD);
        jetpackFuel = _maxJetpackFuel;
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
        IsUsingAbility = true;

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
        IsUsingAbility = false;

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
