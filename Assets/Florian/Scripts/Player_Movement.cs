using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _acceleration;
    [SerializeField]
    private AnimationCurve _decceleration;
    [SerializeField]
    private float _movementSpeed = 10f;

    private Player_Character _character;
    private Player_Input_Mappings _inputActions;

    private Vector2 _moveDir;
    private Vector3 _lastDirection;
    private bool _isMoving;
    private float _timeMoving;
    private float _timeStopping;

    private void Awake()
    {
        _inputActions = new Player_Input_Mappings();
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
                500f * Time.deltaTime);
        }
        else
        {
            _timeStopping += Time.deltaTime;
            float decceleration = _decceleration.Evaluate(_timeStopping);
            movement = _lastDirection * _movementSpeed * decceleration * Time.fixedDeltaTime;
        }
        _character.CharacterRigidbody.MovePosition(transform.position + movement);
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _moveDir = ctx.ReadValue<Vector2>();
            _moveDir.Normalize();
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
}
