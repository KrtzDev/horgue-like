using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement_Mobility : MonoBehaviour
{
    [Header("Player General Ability")]
    [SerializeField] private float _abilityCDTimer;
    private bool _isUsingAbility;
    [SerializeField]
    private bool _isGrounded;
    [SerializeField]
    private float _groundCheckValueY;
    [SerializeField]
    private Vector3 _groundCheckBox;
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private LayerMask _enemyLayer;

    private Player_Character _character;
    private Player_Movement _playerMovement;
    private Player_Input_Mappings _inputActions;

    [Header("Which (one) Ability can be used?")]

    [SerializeField]
    bool _canUseJumpAbility;
    [SerializeField]
    bool _canUseDashAbility;
    [SerializeField]
    bool _canUseStealthAbility;
    [SerializeField]
    bool _canUseFlickerStrikeAbility;

    [Header("Player Jump Ability")]
    [SerializeField]
    private float _jumpCD;
    private bool _hasJumped;
    [SerializeField] private float _fallMultiplier = 7;
    [SerializeField] private float _jumpVelocityFalloff = 8;
    [SerializeField] private float _jumpForce = 15;

    [Header("Player Dash Ability")]
    [SerializeField]
    private GameObject _Decoy;
    [SerializeField]
    private float _dashCD;
    [SerializeField]
    private float _dashForce = 30;
    [SerializeField]
    private float _dashTime = 0.25f;

    [Header("Player Stealth Ability")]
    [SerializeField]
    private float _stealthCD;
    [SerializeField]
    private float _stealthTime;
    [SerializeField]
    private EnemySpawner _EnemySpawner;
    [SerializeField]
    private float _movementSpeedMultiplier;
    private float _stealthMovementSpeed;
    private float _originalMovementSpeed;

    [Header("Player Flicker Strike Ability")]
    [SerializeField]
    private float _flickerStrikeCD;
    [SerializeField]
    private float _flickerStrikeTime;
    [SerializeField]
    private float _flickerStrikeRange;
    private Enemy _closestEnemy;
    private Player_Simple_Shot _simpleShot;

    private void Awake()
    {
        _inputActions = new Player_Input_Mappings();
        _inputActions.Character.MovementAction.performed += UseAbility;
        _EnemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        _simpleShot = this.GetComponent<Player_Simple_Shot>();
    }

    private void Start()
    {
        _character = GetComponent<Player_Character>();
        _playerMovement = GetComponent<Player_Movement>();
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
        FallPhysics();
        CheckIfGrounded();
    }

    // General

    private void UseAbility(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !_isUsingAbility)
        {
            if (_canUseJumpAbility && _abilityCDTimer <= 0)
            {
                JumpAbility();
            }

            if (_canUseDashAbility && _abilityCDTimer <= 0)
            {
                DashAbility();
            }

            if (_canUseStealthAbility && _abilityCDTimer <= 0)
            {
                StealthAbility();
            }

            if (_canUseFlickerStrikeAbility && _abilityCDTimer <= 0)
            {
                FlickerStrikeAbility();
            }
        }
    }

    private void TrackTimer()
    {
        if (_abilityCDTimer > 0) _abilityCDTimer -= Time.deltaTime;
    }

    private void CheckIfGrounded()
    {
        Collider[] hitColliders = (Physics.OverlapBox(this.transform.position + new Vector3(0, _groundCheckValueY, 0), _groundCheckBox, Quaternion.identity, _groundLayer));

        if(hitColliders.Length > 0)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    // Jump

    private void ResetAbilityTimer(float cd)
    {
        _abilityCDTimer = cd;
    }

    private void JumpAbility()
    {
        _isUsingAbility = true;
        _hasJumped = true;

        Vector2 jumpDir = new Vector2(_character.CharacterRigidbody.velocity.x, _jumpForce);
        _character.CharacterRigidbody.AddForce(jumpDir, ForceMode.Impulse);
    }

    private void FallPhysics()
    {
        if ((_character.CharacterRigidbody.velocity.y < _jumpVelocityFalloff || _character.CharacterRigidbody.velocity.y > 0 && !_inputActions.Character.MovementAction.triggered) && !_isGrounded)
        {
            _character.CharacterRigidbody.velocity += _fallMultiplier * Physics.gravity.y * Vector3.up * Time.deltaTime;
        }

        if (_hasJumped)
        {
            if (_isGrounded && _character.CharacterRigidbody.velocity.y < 1)
            {
                ResetJumpAbility();
            }
        }
    }

    private void ResetJumpAbility()
    {
        _isUsingAbility = false;
        _hasJumped = false;

        ResetAbilityTimer(_jumpCD);
    }

    // Dash

    private void DashAbility()
    {
        _playerMovement._canMove = false;
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
        _playerMovement._canMove = true;
        _character.CharacterRigidbody.useGravity = true;
        _isUsingAbility = false;

        ResetAbilityTimer(_dashCD);
    }

    // Stealth

    private void StealthAbility()
    {
        _isUsingAbility = true;

        _Decoy.transform.parent = null;
        _Decoy.GetComponent<CapsuleCollider>().enabled = true;
        _Decoy.transform.position = this.transform.position;
        _Decoy.transform.rotation = this.transform.rotation;
        _Decoy.transform.GetChild(0).gameObject.SetActive(true);

        _originalMovementSpeed = _playerMovement._movementSpeed;
        _stealthMovementSpeed = _originalMovementSpeed * _movementSpeedMultiplier;
        _playerMovement._movementSpeed = _stealthMovementSpeed;

        for (int i = 0; i < _EnemySpawner.transform.childCount; i++)
        {
            for (int j = 0; j < _EnemySpawner.transform.GetChild(i).childCount; j++)
            {
                _EnemySpawner.transform.GetChild(i).GetChild(j).GetComponent<EnemyMovement>().playerTarget = _Decoy.transform;
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

        for (int i = 0; i < _EnemySpawner.transform.childCount; i++)
        {
            for (int j = 0; j < _EnemySpawner.transform.GetChild(i).childCount; j++)
            {
                _EnemySpawner.transform.GetChild(i).GetChild(j).GetComponent<EnemyMovement>().playerTarget = this.transform;
            }
        }

        _playerMovement._movementSpeed = _originalMovementSpeed;

        _Decoy.transform.GetChild(0).gameObject.SetActive(false);
        _Decoy.transform.position = new Vector3(0, 0, 0);
        _Decoy.transform.rotation = Quaternion.identity;
        _Decoy.GetComponent<CapsuleCollider>().enabled = false;
        _Decoy.transform.parent = this.transform;

        ResetAbilityTimer(_stealthCD);
    }

    // Flicker Strike

    private void FlickerStrikeAbility()
    {
        FindClosestEnemy();

        if(_closestEnemy != null)
        {
            _simpleShot.canShoot = false;

            _isUsingAbility = true;
            _playerMovement._canMove = false;

            Vector3 dashDir = (_closestEnemy.transform.position - transform.position).normalized;
            Vector3 forceToApply = dashDir * _dashForce;

            _character.transform.rotation = Quaternion.LookRotation(dashDir, Vector3.up);

            _character.CharacterRigidbody.velocity = Vector3.zero;
            _character.CharacterRigidbody.useGravity = false;
            _character.CharacterRigidbody.AddForce(forceToApply, ForceMode.Impulse);

            StartCoroutine(StopFlickerStrike());
        }
    }

    private IEnumerator StopFlickerStrike()
    {
        yield return new WaitForSeconds(_flickerStrikeTime);

        ResetFlickerStrikeAbility();
    }

    private void FindClosestEnemy()
    {
        float currentclosestdistance = Mathf.Infinity;
        _closestEnemy = null;

        Collider[] enemies = Physics.OverlapSphere(transform.position, _flickerStrikeRange, _enemyLayer);
        foreach (var enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < currentclosestdistance)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (enemy.transform.position - transform.position), out hit, distanceToEnemy, _groundLayer))
                {
                }
                else
                {
                    _closestEnemy = enemy.GetComponent<Enemy>();
                    currentclosestdistance = distanceToEnemy;
                }
            }

        }
    }

    private void ResetFlickerStrikeAbility()
    {
        _isUsingAbility = false;
        _playerMovement._canMove = true;
        _simpleShot.canShoot = true;

        ResetAbilityTimer(_flickerStrikeCD);
    }

    #region Draw_Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position + new Vector3(0, _groundCheckValueY, 0), _groundCheckBox);
    }
    #endregion
}
