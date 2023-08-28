using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Player General Ability")]
    public float _abilityCDTimer;
    public float _currentMaxCD;

    private PlayerCharacter _playerCharacter;
    private PlayerMovement _playerMovement;
    private PlayerInputMappings _inputActions;

    public bool IsUsingAbility;
    public bool ButtonHeld;

    [Header("Which (one) Ability can be used?")]

    public bool CanUseDashAbility;
    public bool CanUseForceSphereAbility;
    public bool CanUseJetpackAbility;
    public bool CanUseEarthquakeAbility;
    public bool CanUseStealthAbility;

    [Header("Dash Ability")]
    [SerializeField] private GameObject _dashVisuals;
    [SerializeField] private Transform _dashEffectPosition;
    [SerializeField] private ParticleSystem _dashEffect;
    [SerializeField] private float _dashCD;
    [SerializeField] private float _dashForce = 30;
    [SerializeField] private float _dashTime = 0.25f;

    [Header("Force Sphere Ability")]
    [SerializeField] private GameObject _forceSphereVisuals;
    [SerializeField] private GameObject _forceSpherePrefab;
    [SerializeField] private Transform _forceSphereSpawnPosition;
    [SerializeField] private Transform _forceSphereEffectPosition;
    [SerializeField] private ParticleSystem _forceSphereEffect;
    [SerializeField] private float _forceSphereCD;
    public float ForceSphereStartRadius;
    public float ForceSphereEndRadius;
    public float ForceSphereDuration;
    [Range(0.0f, 1.0f)] public float ForceSphereStartTransparency;
    [Range(0.0f, 1.0f)] public float ForceSphereEndTransparency;
    public float ForceSphereForce;

    [Header("Jetpack Ability")]
    public bool IsUsingJetpack;
    [SerializeField] private GameObject _jetpackVisuals;
    [SerializeField] private Transform _jetpackEffectPosition;
    [SerializeField] private ParticleSystem _jetpackEffect;
    [SerializeField] private float _jetpackCD;
    [Range(0.0f, 100.0f)] public float JetpackFuel;
    public float MaxJetpackFuel;
    [SerializeField] private float _energyExpansionRate;
    [SerializeField] private float _jetpackThrustForce;
    [SerializeField] private float _jetpackHeatFallOffTime;
    [SerializeField] private float _jetpackHeatFallOffMultiplier;
    private float _jetpackActiveTimer;

    [Header("Earthquake Ability")]
    [SerializeField] private GameObject _earthquakeVisuals;
    [SerializeField] private GameObject _earthquakePreviewPrefab;
    [SerializeField] private GameObject _earthquakePrefab;
    [SerializeField] private Transform _earthquakeSpawnPosition;
    [SerializeField] private Transform _earthquakeEffectPosition;
    [SerializeField] private ParticleSystem _earthquakeEffect;
    [SerializeField] private float _earthquakeCD;
    [SerializeField] private float _earthquakeRadius;
    [SerializeField] private float _earthquakeForce;
    public float _earthquakeLoadUpTime;
    public float _earthquakeActiveTime;

    [Header("Stealth Ability")]
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
        // _enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();

        _abilityCDTimer = -1;
    }

    private void Start()
    {
        _playerCharacter = GetComponent<PlayerCharacter>();
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
        if (CanUseDashAbility)
        {
            _dashVisuals.SetActive(true);
        }
        else if (CanUseForceSphereAbility)
        {
            _forceSphereVisuals.SetActive(true);
        }
        else if (CanUseJetpackAbility)
        {
            _jetpackVisuals.SetActive(true);
        }
        else if (CanUseEarthquakeAbility)
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
            if (CanUseDashAbility && _abilityCDTimer <= 0)
            {
                DashAbility();
            }
            else if (CanUseForceSphereAbility && _abilityCDTimer <= 0)
            {
                ForceSphereAbility();
            }
            else if (CanUseJetpackAbility && _abilityCDTimer <= 0)
            {
                JetpackAbility();
                JetpackParticleEffect();
            }
            else if (CanUseStealthAbility && _abilityCDTimer <= 0)
            {
                DecoyAbility();
            }
            else if (CanUseEarthquakeAbility && _playerCharacter.GetComponent<PlayerJump>().IsGrounded && _abilityCDTimer <= 0)
            {
                EarthquakeAbility();
            }
            
        }
        else if (ctx.started && IsUsingAbility && CanUseJetpackAbility && GameManager.Instance._playerCanUseAbilities)
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

        _playerCharacter.CharacterRigidbody.velocity = Vector3.zero;
        _playerCharacter.CharacterRigidbody.useGravity = false;
        _playerCharacter.CharacterRigidbody.AddForce(forceToApply, ForceMode.Impulse);
        _playerCharacter.healthComponent.canTakeDamage = false;

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
        _playerCharacter.CharacterRigidbody.useGravity = true;
        IsUsingAbility = false;

        _playerCharacter.healthComponent.canTakeDamage = true;

        ResetAbilityTimer(_dashCD);
    }

    // Force Sphere

    private void ForceSphereAbility()
    {
        IsUsingAbility = true;

        GameObject forceSphere = _forceSpherePrefab;
        Instantiate(forceSphere, _forceSphereSpawnPosition);
    }

    public void ResetForceSphereAbility()
    {
        IsUsingAbility = false;

        ResetAbilityTimer(_forceSphereCD);
    }

    // Jetpack

    private void JetpackAbility()
    {
        _jetpackVisuals.GetComponentInChildren<Animator>().SetBool("jetpackOn", true);
        _jetpackVisuals.GetComponentInChildren<Animator>().SetBool("jetpackOff", false);

        MaxJetpackFuel = JetpackFuel;

        IsUsingAbility = true;
    }

    private void JetpackParticleEffect()
    {
        ParticleSystem jp_effect = _jetpackEffect;
        Instantiate(jp_effect, _jetpackEffectPosition);
    }

    private void JetpackForce()
    {
        if(CanUseJetpackAbility && IsUsingAbility)
        {
            if (ButtonHeld && JetpackFuel > 0)
            {
                IsUsingJetpack = true;

                JetpackFuel -= _energyExpansionRate * Time.deltaTime;

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

                _playerCharacter.CharacterRigidbody.velocity = new Vector3(_playerCharacter.CharacterRigidbody.velocity.x, 0, _playerCharacter.CharacterRigidbody.velocity.z);
                // _playerCharacter.CharacterRigidbody.AddForce(_playerCharacter.CharacterRigidbody.transform.up * _jetpackThrustForce * multiplier, ForceMode.Impulse);
                _playerCharacter.transform.position += new Vector3(0, _jetpackThrustForce * 8 * multiplier * Time.deltaTime, 0);
            }
            else if (!ButtonHeld)
            {
                if(_jetpackActiveTimer > 0)
                    _jetpackActiveTimer -= 5 * Time.deltaTime;

                IsUsingJetpack = false;
            }

            if (JetpackFuel <= 0)
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
        JetpackFuel = MaxJetpackFuel;
    }

    // Earthquake

    private void EarthquakeAbility()
    {
        // Instantiate Circle

        IsUsingAbility = true;

        GameObject earthquakePreview = _earthquakePreviewPrefab;
        earthquakePreview.transform.localScale = new Vector3(_earthquakeRadius, earthquakePreview.transform.localScale.y, _earthquakeRadius);
        Instantiate(earthquakePreview, _earthquakeSpawnPosition.transform.position, Quaternion.identity);

        StartCoroutine(Earthquake(_earthquakeSpawnPosition.transform.position));
    }

    private IEnumerator Earthquake(Vector3 spawnPos)
    {
        yield return new WaitForSeconds(_earthquakeLoadUpTime);

        // Earthquake

        GameObject earthquake = _earthquakePrefab;
        earthquake.transform.localScale = new Vector3(_earthquakeRadius, earthquake.transform.localScale.y, _earthquakeRadius);
        Instantiate(earthquake, spawnPos, Quaternion.identity);

        EarthquakeParticleEffect(spawnPos);

        yield return new WaitForSeconds(_earthquakeActiveTime);

        ResetEarthquakeAbility();
    }

    private void EarthquakeParticleEffect(Vector3 spawnPos)
    {
        ParticleSystem eq_effect = _earthquakeEffect;
        ParticleSystem.ShapeModule shape = eq_effect.shape;
        
        shape.radius = _earthquakeRadius / 2;

        Instantiate(eq_effect, spawnPos, Quaternion.identity);
    }

    private void ResetEarthquakeAbility()
    {
        IsUsingAbility = false;

        ResetAbilityTimer(_earthquakeCD);
    }

    // Stealth

    private void DecoyAbility()
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
                    _enemySpawner._enemyObjectPoolParent.transform.GetChild(i).GetChild(j).GetComponent<AI_Agent_Enemy>().FollowDecoy = true;
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
                _enemySpawner._enemyObjectPoolParent.transform.GetChild(i).GetChild(j).GetComponent<AI_Agent_Enemy>().FollowDecoy = false;
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
