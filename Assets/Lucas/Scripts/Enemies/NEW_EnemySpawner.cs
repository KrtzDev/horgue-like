using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEW_EnemySpawner : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField]
    private Transform PlayerTransform;

    [SerializeField]
    private bool _enableGizmos;

    // Variablen
    [Header("Variables")]
    [SerializeField]
    private float _boxHeight = 0.25f;
    [SerializeField]
    private float _closeZoneRadius = 8f;
    [SerializeField]
    private float _midZoneRadius = 16f;
    [SerializeField]
    private float _farZoneRadius = 24f;


    [Header("Box Colliders")]
    [SerializeField]
    private BoxCollider FrontalZone;
    [SerializeField]
    private BoxCollider RearZone;
    [SerializeField]
    private BoxCollider LeftLateralZone;
    [SerializeField]
    private BoxCollider RightLateralZone;
    [SerializeField]
    private BoxCollider LeftUpPeripheralZone;
    [SerializeField]
    private BoxCollider RightUpPeripheralZone;
    [SerializeField]
    private BoxCollider LeftDownPeripheralZone;
    [SerializeField]
    private BoxCollider RightDownPeripheralZone;

    [Header("Sphere Colliders")]
    [SerializeField]
    private SphereCollider CloseZone;
    [SerializeField]
    private SphereCollider MidZone;
    [SerializeField]
    private SphereCollider FarZone;


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
        // Box Colliders

        FrontalZone.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _farZoneRadius);
        FrontalZone.center = PlayerTransform.position + new Vector3(0, 0, _farZoneRadius / 2);

        RearZone.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _farZoneRadius);
        RearZone.center = PlayerTransform.position - new Vector3(0, 0, _farZoneRadius / 2);

        LeftLateralZone.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        LeftLateralZone.center = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, 0);

        RightLateralZone.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        RightLateralZone.center = PlayerTransform.position - new Vector3(_closeZoneRadius * 2, 0, 0);

        LeftUpPeripheralZone.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        LeftUpPeripheralZone.center = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, _closeZoneRadius * 2);

        RightUpPeripheralZone.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        RightUpPeripheralZone.center = PlayerTransform.position + new Vector3(-_closeZoneRadius * 2, 0, _closeZoneRadius * 2);

        LeftDownPeripheralZone.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        LeftDownPeripheralZone.center = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, -_closeZoneRadius * 2);

        RightDownPeripheralZone.size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        RightDownPeripheralZone.center = PlayerTransform.position + new Vector3(-_closeZoneRadius * 2, 0, -_closeZoneRadius * 2);

        // Sphere Colliders

        CloseZone.radius = _closeZoneRadius;
        MidZone.radius = _midZoneRadius;
        FarZone.radius = _farZoneRadius;
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
