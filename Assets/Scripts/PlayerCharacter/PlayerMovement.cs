using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [field: SerializeField, Header("Movement")]
    public float MovementSpeed { get; set; } = 10f;
    private PlayerCharacter _playerCharacter;

    [SerializeField]
    private AnimationCurve _acceleration;
    [SerializeField]
    private AnimationCurve _decceleration;

    [field: SerializeField, Header("Misc")]
    public Vector3 LastDirection { get; set; }
    [field: SerializeField]
    public bool CanMove { get; set; } = true;

    private PlayerCharacter _character;
    private PlayerInputMappings _inputActions;

    private Vector2 _moveDir;
    private bool _isMoving;
    private float _timeMoving;
    private float _timeStopping;

    private void Awake()
    {
        _playerCharacter = this.GetComponent<PlayerCharacter>();
        MovementSpeed = _playerCharacter._playerData._movementSpeed;
        _inputActions = new PlayerInputMappings();
        if (InputManager.Instance)
        {
            InputManager.Instance.CharacterInputActions = _inputActions;
        }
        _inputActions.Character.Movement.performed += Move;
        _inputActions.Character.Movement.canceled += StopMove;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void Start()
    {
        _character = GetComponent<PlayerCharacter>();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void FixedUpdate()
    {
        if (!CanMove) return;

        Vector3 cameraForward = _character.Camera.transform.forward;
        Vector3 cameraRight = _character.Camera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 relativeMoveDirection = _moveDir.y * cameraForward + _moveDir.x * cameraRight;
        Vector3 movement;

        if (_isMoving)
        {
            _timeMoving += Time.deltaTime;
            float acceleration = _acceleration.Evaluate(_timeMoving);
            movement = relativeMoveDirection * MovementSpeed * acceleration * Time.fixedDeltaTime;
            LastDirection = relativeMoveDirection;

            _character.transform.rotation =
                Quaternion.RotateTowards(_character.transform.rotation,
                Quaternion.LookRotation(relativeMoveDirection, Vector3.up),
                700f * Time.deltaTime);
        }
        else
        {
            _timeStopping += Time.deltaTime;
            float decceleration = _decceleration.Evaluate(_timeStopping);
            movement = LastDirection * MovementSpeed * decceleration * Time.fixedDeltaTime;
        }

        //_character.CharacterRigidbody.MovePosition(transform.position + movement);

        //_character.CharacterRigidbody.AddForce(movement * 3, ForceMode.VelocityChange);

        _character.CharacterRigidbody.velocity = new Vector3(movement.x * 50, _character.CharacterRigidbody.velocity.y, movement.z * 50);
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _moveDir = ctx.ReadValue<Vector2>();
            _moveDir.Normalize();

            //scewes the walking direction
            //TODO: rework movement or make Camera not othographic
            //_moveDir.x *= .66f;

            _isMoving = true;
            _timeStopping = 0;
        }
    }

    private void StopMove(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            _moveDir = Vector2.zero;
            _isMoving = false;
            _timeMoving = 0;
        }
    }

    #region Draw_Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(this.transform.position, this.transform.position + transform.TransformDirection(Vector3.forward));
    }
    #endregion
}
