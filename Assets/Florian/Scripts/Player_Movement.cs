using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    private Player_Character _character;
    private Player_Input_Mappings _inputActions;

    private float _movementSpeed = 10f;
    private Vector2 _moveDir;

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

        _character.CharacterRigidbody.MovePosition(transform.position + (relativeMoveDirection * _movementSpeed * Time.fixedDeltaTime));
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
            _moveDir = ctx.ReadValue<Vector2>();
       
    }

    private void StopMove(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
            _moveDir = Vector2.zero;
    }
}
