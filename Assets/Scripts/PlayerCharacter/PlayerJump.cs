using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    private float _jumpCDTimer;

    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _groundCheckValueY;
    [SerializeField] private Vector3 _groundCheckBox;

    private PlayerCharacter _character;
    private PlayerInputMappings _inputActions;

    private bool _isJumping;

    [SerializeField] private float _jumpCD;
    [SerializeField] private float _fallMultiplier = 7;
    [SerializeField] private float _jumpVelocityFalloff = 8;
    [SerializeField] private float _jumpForce = 15;

    private bool _hasJumped;

    private void Awake()
    {
        _inputActions = new PlayerInputMappings();
        _inputActions.Character.Jump.performed += Jump;

        _jumpCDTimer = -1;
    }

    private void Start()
    {
        _character = GetComponent<PlayerCharacter>();
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

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !_isJumping && GameManager.Instance._playerCanUseAbilities)
        {
            if (_jumpCDTimer <= 0)
            {
                JumpAbility();
            }
        }
    }

    private void TrackTimer()
    {
        if (_jumpCDTimer > 0) _jumpCDTimer -= Time.deltaTime;
    }

    private void CheckIfGrounded()
    {
        Collider[] hitColliders = (Physics.OverlapBox(this.transform.position + new Vector3(0, _groundCheckValueY, 0), _groundCheckBox, Quaternion.identity, _character.WalkLayer));

        if (hitColliders.Length > 0)
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
        _jumpCDTimer = cd;
    }

    private void JumpAbility()
    {
        _isJumping = true;
        _hasJumped = true;

        Vector2 jumpDir = new Vector2(_character.CharacterRigidbody.velocity.x, _jumpForce);
        _character.CharacterRigidbody.AddForce(jumpDir, ForceMode.Impulse);
    }

    private void FallPhysics()
    {
        if ((_character.CharacterRigidbody.velocity.y < _jumpVelocityFalloff || _character.CharacterRigidbody.velocity.y > 0 && !_inputActions.Character.Jump.triggered) && !_isGrounded)
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
        _isJumping = false;
        _hasJumped = false;

        ResetAbilityTimer(_jumpCD);
    }

    #region Draw_Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position + new Vector3(0, _groundCheckValueY, 0), _groundCheckBox);
    }
    #endregion
}
