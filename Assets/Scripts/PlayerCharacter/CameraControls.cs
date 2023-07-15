using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private int _rotation = 90;

    private CinemachineVirtualCamera _cm;
    private CinemachineOrbitalTransposer _tr;

    private void Start()
    {
        _cm = GetComponent<CinemachineVirtualCamera>();
        _tr = _cm.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }


    private void Update()
    {
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
    }
}
