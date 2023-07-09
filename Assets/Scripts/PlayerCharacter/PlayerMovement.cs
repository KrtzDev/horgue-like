using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;

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

    // Record Player Path History for Enemy Movement Prediction
    [SerializeField] [Range(0.1f, 5f)] private float _historicalPositionDuration = 1f;
    [SerializeField] [Range(0.001f, 1f)] private float _historicalPositionInterval = 0.1f;
    private Queue<Vector3> _historicalVelocities;
    private float _lastPositinTime;
    private int _maxQueueSize;

    public Vector3 AverageVelocity
    {
        get
        {
            Vector3 average = Vector3.zero;
            foreach(Vector3 velocity in _historicalVelocities)
            {
                average += velocity;
            }
            average.y = 0;

            return average / _historicalVelocities.Count;
        }
    }


    private void Awake()
    {
        _playerCharacter = GetComponent<PlayerCharacter>();

        _maxQueueSize = Mathf.CeilToInt(1f / _historicalPositionInterval * _historicalPositionDuration);  // Calculate Queue Size
        _historicalVelocities = new Queue<Vector3>(_maxQueueSize);

        MovementSpeed = _playerCharacter._playerData._movementSpeed;
		_inputActions = InputManager.Instance?.CharacterInputActions;
    }

	private void Start()
	{
        _character = GetComponent<PlayerCharacter>();
	}

    private void OnEnable()
    {
		if (_inputActions == null)
			return;
        _inputActions.Character.Movement.performed += Move;
        _inputActions.Character.Movement.canceled += StopMove;

        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions?.Disable();
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

        _character.CharacterRigidbody.velocity = new Vector3(movement.x * 50, _character.CharacterRigidbody.velocity.y, movement.z * 50);

        if(_lastPositinTime + _historicalPositionInterval <= Time.time)
        {
            if(_historicalVelocities.Count == _maxQueueSize)
            {
                _historicalVelocities.Dequeue();
            }

            _historicalVelocities.Enqueue(this.GetComponent<Rigidbody>().velocity);
            _lastPositinTime = Time.time;
        }
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
