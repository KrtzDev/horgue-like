using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private int _rotation = 90;

    private CinemachineVirtualCamera _cm;
    private CinemachineOrbitalTransposer _tr;

    private PlayerInputMappings _inputActions;
    private float _moveDir;

    private void Start()
    {
        _cm = GetComponent<CinemachineVirtualCamera>();
        _tr = _cm.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        _inputActions = new PlayerInputMappings();

        _inputActions.Character.Enable();

        _inputActions.Character.CameraMovement.performed += MoveCamera;
        _inputActions.Character.CameraMovement.canceled += ReleasedButton;
    }

    private void Update()
    {
        /*
        var keyboard = Keyboard.current;

        if (keyboard.qKey.wasPressedThisFrame)
        {
            _tr.m_XAxis.Value -= _rotation;
        }
        else if (keyboard.eKey.wasPressedThisFrame)
        {
            _tr.m_XAxis.Value += _rotation;
        }
        else if (keyboard.rKey.wasPressedThisFrame)
        {
            _tr.m_XAxis.Value = 0.0f;
        }
        */

        if(_moveDir < 0)
        {
            _tr.m_XAxis.Value -= Time.deltaTime * _rotation;
        }
        else if (_moveDir > 0)
        {
            _tr.m_XAxis.Value += Time.deltaTime * _rotation;
        }
    }

    private void MoveCamera(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            _moveDir = ctx.ReadValue<float>();
        }
    }

    private void ReleasedButton(InputAction.CallbackContext ctx)
    {
        if(ctx.canceled)
        {
            _moveDir = 0;
        }
    }
}
