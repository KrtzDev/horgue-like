using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _acceleration;
    [SerializeField]
    private AnimationCurve _decceleration;
    public float _movementSpeed = 10f;

    private Player_Character _character;
    private Player_Input_Mappings _inputActions;

    private Vector2 _moveDir;
    public Vector3 _lastDirection;
    private bool _isMoving;
    public bool _canMove = true;
    private float _timeMoving;
    private float _timeStopping;

    private void Awake()
    {
        _inputActions = new Player_Input_Mappings();
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
        _character = GetComponent<Player_Character>();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void FixedUpdate()
    {
        if (!_canMove) return;
        else
        {
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
                movement = relativeMoveDirection * _movementSpeed * acceleration * Time.fixedDeltaTime;
                _lastDirection = relativeMoveDirection;

                _character.transform.rotation =
                    Quaternion.RotateTowards(_character.transform.rotation,
                    Quaternion.LookRotation(relativeMoveDirection, Vector3.up),
                    700f * Time.deltaTime);
            }
            else
            {
                _timeStopping += Time.deltaTime;
                float decceleration = _decceleration.Evaluate(_timeStopping);
                movement = _lastDirection * _movementSpeed * decceleration * Time.fixedDeltaTime;
            }

            //_character.CharacterRigidbody.MovePosition(transform.position + movement);

            //_character.CharacterRigidbody.AddForce(movement * 3, ForceMode.VelocityChange);

            _character.CharacterRigidbody.velocity = new Vector3(movement.x * 50, _character.CharacterRigidbody.velocity.y, movement.z * 50);
        }
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _moveDir = ctx.ReadValue<Vector2>();
            _moveDir.Normalize();

            //scewes the walking direction
            //TODO: rework movement 
            _moveDir.x *= .66f;

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
