using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> _cameraPresets;

    private int _presetIndex = 0;

    private CinemachineVirtualCamera _cm;
    private CinemachineTransposer _tr;

    private void Start()
    {
        _cm = GetComponent<CinemachineVirtualCamera>();
        _tr = _cm.GetCinemachineComponent<CinemachineTransposer>();
    }


    private void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard.qKey.wasPressedThisFrame)
        {
            _presetIndex -= 1;
        }

        if (keyboard.eKey.wasPressedThisFrame)
        {
            _presetIndex += 1;
        }

        if (_presetIndex < 0)
        {
            _presetIndex = _cameraPresets.Count - 1;
        }
        else if (_presetIndex >= _cameraPresets.Count)
        {
            _presetIndex = 0;
        }

        _tr.m_FollowOffset = _cameraPresets[_presetIndex];
    }
}
