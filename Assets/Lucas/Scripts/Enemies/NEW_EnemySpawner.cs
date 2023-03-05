using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEW_EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform PlayerTransform;

    [SerializeField]
    private bool _enableGizmos;

    // Variablen
    [SerializeField]
    private float _boxHeight = 0.25f;
    [SerializeField]
    private float _closeZoneRadius = 8f;
    [SerializeField]
    private float _midZoneRadius = 16f;
    [SerializeField]
    private float _farZoneRadius = 24f;

    [SerializeField]
    private BoxCollider Frontal;
    [SerializeField]
    private BoxCollider Rear;
    [SerializeField]
    private BoxCollider LeftLateral;
    [SerializeField]
    private BoxCollider RightLateral;
    [SerializeField]
    private BoxCollider LeftUpPeripheral;
    [SerializeField]
    private BoxCollider RightUpPeripheral;
    [SerializeField]
    private BoxCollider LeftDownPeripheral;
    [SerializeField]
    private BoxCollider RightDownPeripheral;

    private void Start()
    {
        if(PlayerTransform == null)
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        SetColliderSizeCenter();
    }

    private void Update()
    {
        transform.SetPositionAndRotation(PlayerTransform.position, PlayerTransform.rotation);
    }

    private void SetColliderSizeCenter()
    {
        Frontal.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _farZoneRadius);
        Frontal.center = PlayerTransform.position + new Vector3(0, 0, _farZoneRadius / 2);

        Rear.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _farZoneRadius);
        Rear.center = PlayerTransform.position - new Vector3(0, 0, _farZoneRadius / 2);

        LeftLateral.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        LeftLateral.center = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, 0);

        RightLateral.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        RightLateral.center = PlayerTransform.position - new Vector3(_closeZoneRadius * 2, 0, 0);

        LeftUpPeripheral.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        LeftUpPeripheral.center = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, _closeZoneRadius * 2);

        RightUpPeripheral.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        RightUpPeripheral.center = PlayerTransform.position + new Vector3(-_closeZoneRadius * 2, 0, _closeZoneRadius * 2);

        LeftDownPeripheral.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        LeftDownPeripheral.center = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, -_closeZoneRadius * 2);

        RightDownPeripheral.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        RightDownPeripheral.center = PlayerTransform.position + new Vector3(-_closeZoneRadius * 2, 0, -_closeZoneRadius * 2);
    }

    // GIZMOS

    private void OnDrawGizmos()
    {
        if (_enableGizmos)
        {
            Vector3 _startPos;

            // Settings speichern
            Color prevColor = Gizmos.color;
            Matrix4x4 prevMatrix = Gizmos.matrix;

            // Frontal
            Gizmos.color = Color.cyan;
            Vector3 FrontalVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _farZoneRadius);
            _startPos = PlayerTransform.position + new Vector3(0, 0, _farZoneRadius / 2);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, FrontalVector);

            // Rear
            Gizmos.color = Color.magenta;
            Vector3 RearVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _farZoneRadius);
            _startPos = PlayerTransform.position - new Vector3(0, 0, _farZoneRadius / 2);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, RearVector);

            // Left Lateral
            Gizmos.color = Color.green;
            Vector3 LeftLateralVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
            _startPos = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, 0);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, LeftLateralVector);

            // Right Lateral
            Vector3 RightLateralVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
            _startPos = PlayerTransform.position - new Vector3(_closeZoneRadius * 2, 0, 0);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, RightLateralVector);

            // Left Up Peripheral
            Gizmos.color = Color.yellow;
            Vector3 LeftUpPeripheralVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
            _startPos = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, _closeZoneRadius * 2);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, LeftUpPeripheralVector);

            // Right Up Peripheral
            Vector3 RightUpPeripheralVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
            _startPos = PlayerTransform.position + new Vector3(-_closeZoneRadius * 2, 0, _closeZoneRadius * 2);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, RightUpPeripheralVector);

            // Left Down Peripheral
            Vector3 LeftDownPeripheralVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
            _startPos = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, -_closeZoneRadius * 2);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, LeftDownPeripheralVector);

            // Right Down Peripheral
            Vector3 RightDownPeripheralVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
            _startPos = PlayerTransform.position + new Vector3(-_closeZoneRadius * 2, 0, -_closeZoneRadius * 2);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, RightDownPeripheralVector);

            // Settings resetten
            Gizmos.color = prevColor;
            Gizmos.matrix = prevMatrix;
        }
    }

}
