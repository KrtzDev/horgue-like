using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{

    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _groundCheckValueY;
    [SerializeField] private Vector3 _groundCheckBox;

    private PlayerCharacter _character;
    private PlayerInputMappings _inputActions;

    private bool _isJumping;
    private bool _jumpAttempt;

    [SerializeField] private float _fallMultiplier = 7;
    [SerializeField] private float _jumpVelocityFalloff = 8;
    [SerializeField] private float _jumpForce = 15;

    private void Awake()
    {
        _inputActions = new PlayerInputMappings();
        _inputActions.Character.Jump.performed += Jump;
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
        FallPhysics();
        CheckIfGrounded();
    }

    // General

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !_isJumping && _isGrounded)
        {
            JumpAbility();
        }
        else if (ctx.performed && _isJumping && !_isGrounded)
        {
            Debug.Log("Jump Input while Jumping");

            if(Physics.Raycast(this.transform.position + new Vector3(0, _groundCheckValueY, 0), Vector3.down, 2 * _groundCheckValueY, _character.WalkLayer))
            {
                Debug.Log("Buffer Jump?");

                _jumpAttempt = true;
            }
        }
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

    private void JumpAbility()
    {
        _isJumping = true;
        _jumpAttempt = false;

        Vector2 jumpDir = new Vector2(_character.CharacterRigidbody.velocity.x, _jumpForce);
        _character.CharacterRigidbody.AddForce(jumpDir, ForceMode.Impulse);
    }

    private void FallPhysics()
    {
        if ((_character.CharacterRigidbody.velocity.y < _jumpVelocityFalloff || _character.CharacterRigidbody.velocity.y > 0 && !_inputActions.Character.Jump.triggered) && !_isGrounded)
        {
            _character.CharacterRigidbody.velocity += _fallMultiplier * Physics.gravity.y * Vector3.up * Time.deltaTime;
        }

        if (_isJumping)
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
        Debug.Log("Reset Jump!");

        if (_jumpAttempt)
        {
            Debug.Log("Jump Attempt?");

            JumpAbility();
        }
    }

    #region Draw_Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position + new Vector3(0, _groundCheckValueY, 0), _groundCheckBox);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position + new Vector3(0, _groundCheckValueY, 0), this.transform.position - new Vector3(0, 2 * _groundCheckValueY, 0));
    }
    #endregion
}
