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
    private LayerMask _groundLayerMask;

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
    private bool _isUsingJumpAbility;
    [SerializeField] private float _fallMultiplier = 7;
    [SerializeField] private float _jumpVelocityFalloff = 8;
    [SerializeField] private float _jumpForce = 15;

    [Header("Player Dash Ability")]
    [SerializeField]
    private float _dashCD;
    private bool _isUsingDashAbility;
    [SerializeField]
    private float _dashForce = 30;
    [SerializeField]
    private float _dashTime = 0.25f;

    [Header("Player Stealth Ability")]
    [SerializeField]
    private float _stealthCD;
    private bool i_sUsingStealthAbility;

    [Header("Player Flicker Strike Ability")]
    [SerializeField]
    private float _flickerStrikeCD;
    private bool _isUsingFlickerStrikeAbility;

    private void Awake()
    {
        _inputActions = new Player_Input_Mappings();
        _inputActions.Character.MovementAction.performed += UseAbility;
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
        Collider[] hitColliders = (Physics.OverlapBox(this.transform.position + new Vector3(0, _groundCheckValueY, 0), _groundCheckBox, Quaternion.identity, _groundLayerMask));

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
        _isUsingJumpAbility = true;

        Vector2 jumpDir = new Vector2(_character.CharacterRigidbody.velocity.x, _jumpForce);
        _character.CharacterRigidbody.AddForce(jumpDir, ForceMode.Impulse);
    }

    private void FallPhysics()
    {
        if ((_character.CharacterRigidbody.velocity.y < _jumpVelocityFalloff || _character.CharacterRigidbody.velocity.y > 0 && !_inputActions.Character.MovementAction.triggered) && !_isGrounded)
        {
            _character.CharacterRigidbody.velocity += _fallMultiplier * Physics.gravity.y * Vector3.up * Time.deltaTime;
        }

        if (_isUsingJumpAbility)
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
        _isUsingJumpAbility = false;

        ResetAbilityTimer(_jumpCD);
    }

    // Dash

    private void DashAbility()
    {
        _playerMovement._canMove = false;
        _isUsingAbility = true;
        _isUsingDashAbility = true;

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
        _isUsingDashAbility = false;

        ResetAbilityTimer(_dashCD);
    }

    // Stealth

    private void StealthAbility()
    {
        _isUsingAbility = true;
        i_sUsingStealthAbility = true;
    }

    private void ResetStealthbility()
    {
        _isUsingAbility = false;
        i_sUsingStealthAbility = false;

        ResetAbilityTimer(_stealthCD);
    }

    // Flicker Strike

    private void FlickerStrikeAbility()
    {
        _isUsingAbility = true;
        _isUsingFlickerStrikeAbility = true;
    }

    private void ResetFlickerStrikeAbility()
    {
        _isUsingAbility = false;
        _isUsingFlickerStrikeAbility = false;

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
