using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{

    public bool IsGrounded;
    [SerializeField] private float _bufferJumpCheckValueY;
    [SerializeField] private float _groundCheckY;
    [SerializeField] private Vector3 _groundCheckBox;
    
    private PlayerCharacter _character;
    private PlayerInputMappings _inputActions;

    private bool _isJumping;
    private bool _jumpAttempt = false;

    public float _fallMultiplier = 7;
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
        if (ctx.performed && !_isJumping && IsGrounded && !_jumpAttempt && Time.deltaTime != 0)
        {
            JumpAbility();
        }
        else if (ctx.performed && _isJumping && !IsGrounded && !_jumpAttempt)
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
            if(GetComponent<PlayerAbilities>().IsUsingJetpack)
            {
                if(hitColliders.Length <= 0)
                {
                    return;
                }
            }

            if(IsGrounded == false)
            {
                // AudioManager.Instance.PlaySound("Landing");
            }

            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
    }

    // Jump

    private void JumpAbility()
    {
        StatsTracker.Instance.jumpsUsedLevel++;

        _isJumping = true;
        _jumpAttempt = false;

        _character.CharacterRigidbody.velocity = new Vector3(_character.CharacterRigidbody.velocity.x, 0, _character.CharacterRigidbody.velocity.z);

        Vector2 jumpDir = new Vector2(_character.CharacterRigidbody.velocity.x, _jumpForce);
        _character.CharacterRigidbody.AddForce(jumpDir, ForceMode.Impulse);

        AudioManager.Instance.PlaySound("Jump");
    }

    private void FallPhysics()
    {
        if(!_character.GetComponent<PlayerAbilities>().IsUsingJetpack)
        {
            if ((_character.CharacterRigidbody.velocity.y < _jumpVelocityFalloff || (_character.CharacterRigidbody.velocity.y > 0 && !_inputActions.Character.Jump.triggered)) && !IsGrounded)
            {
                _character.CharacterRigidbody.velocity += _fallMultiplier * Physics.gravity.y * Vector3.up * Time.deltaTime;
                _isJumping = true;
            }

            if (IsGrounded && _character.CharacterRigidbody.velocity.y <= 0 && _isJumping)
            {
                ResetJumpAbility();
            }
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
        StatsTracker.Instance.jumpsUsedLevel++;

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
