using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{

    private bool _isGrounded;
    [SerializeField] private float _bufferJumpCheckValueY;
    [SerializeField] private float _groundCheckY;
    [SerializeField] private Vector3 _groundCheckBox;
    
    private PlayerCharacter _character;
    private PlayerInputMappings _inputActions;

    private bool _isJumping;
    private bool _jumpAttempt = false;

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
        CheckIfGrounded();
        FallPhysics();

        Debug.DrawRay(this.transform.position, new Vector3(0, -_bufferJumpCheckValueY, 0), Color.red);
        Debug.DrawRay(this.transform.position, new Vector3(0, -_groundCheckY, 0), Color.yellow);
    }

    // General

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !_isJumping && _isGrounded && !_jumpAttempt)
        {
            JumpAbility();
        }
        else if (ctx.performed && _isJumping && !_isGrounded && !_jumpAttempt)
        {
            if(Physics.Raycast(this.transform.position, Vector3.down, _bufferJumpCheckValueY, _character.WalkLayer))
            {
                _jumpAttempt = true;
            }
        }
    }

    private void CheckIfGrounded()
    {
        Collider[] hitColliders = (Physics.OverlapBox(this.transform.position, _groundCheckBox / 2, Quaternion.identity, _character.WalkLayer));

        if (hitColliders.Length > 0 || Physics.Raycast(this.transform.position, Vector2.down, _groundCheckY, _character.WalkLayer))
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
        Debug.Log("Jump " + Time.time);

        _isJumping = true;
        _jumpAttempt = false;

        _character.CharacterRigidbody.velocity = new Vector3(_character.CharacterRigidbody.velocity.x, 0, _character.CharacterRigidbody.velocity.z);

        Vector2 jumpDir = new Vector2(_character.CharacterRigidbody.velocity.x, _jumpForce);
        _character.CharacterRigidbody.AddForce(jumpDir, ForceMode.Impulse);
    }

    private void FallPhysics()
    {
        if ((_character.CharacterRigidbody.velocity.y < _jumpVelocityFalloff || (_character.CharacterRigidbody.velocity.y > 0 && !_inputActions.Character.Jump.triggered)) && !_isGrounded)
        {
            _character.CharacterRigidbody.velocity += _fallMultiplier * Physics.gravity.y * Vector3.up * Time.deltaTime;
            _isJumping = true;
        }

        if (_isGrounded && _character.CharacterRigidbody.velocity.y <= 0 && _isJumping)
        {
            ResetJumpAbility();
        }
    }

    private void ResetJumpAbility()
    {
        if (_jumpAttempt)
        {
            BufferJump();
            return;
        }

        _isJumping = false;
    }

    private void BufferJump()
    {
        Debug.Log("B Jump " + Time.time);

        _isJumping = true;
        _jumpAttempt = false;

        _character.CharacterRigidbody.velocity = new Vector3(_character.CharacterRigidbody.velocity.x, 0, _character.CharacterRigidbody.velocity.z);

        Vector2 jumpDir = new Vector2(_character.CharacterRigidbody.velocity.x, _jumpForce);
        _character.CharacterRigidbody.AddForce(jumpDir, ForceMode.Impulse);
    }

    #region Draw_Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position,  _groundCheckBox);
    }
    #endregion
}
