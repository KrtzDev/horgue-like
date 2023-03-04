using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEW_EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform PlayerTransform;

    private void Start()
    {
        if(PlayerTransform == null)
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); ;
    }

    // GIZMOS

    private void OnDrawGizmos()
    {
        // Variablen
        float BoxHeight = 0.25f;

        float CloseZoneRadius = 8f;
        float MidZoneRadius = 16f;
        float FarZoneRadius = 24f;

        Vector3 CenterVector;

        // Settings speichern
        Color prevColor = Gizmos.color;
        Matrix4x4 prevMatrix = Gizmos.matrix;

        // Frontal
        Gizmos.color = Color.cyan;
        Vector3 FrontalVector = new Vector3(FarZoneRadius, BoxHeight, CloseZoneRadius * 2);
        CenterVector = PlayerTransform.position + new Vector3(FarZoneRadius / 2, 0, 0);
        CenterVector = transform.InverseTransformPoint(CenterVector);
        Gizmos.DrawCube(CenterVector, FrontalVector);

        // Rear
        Gizmos.color = Color.magenta;
        Vector3 RearVector = new Vector3(FarZoneRadius, BoxHeight, CloseZoneRadius * 2);
        CenterVector = PlayerTransform.position - new Vector3(FarZoneRadius / 2, 0, 0);
        CenterVector = transform.InverseTransformPoint(CenterVector);
        Gizmos.DrawCube(CenterVector, RearVector);

        // Left Lateral
        Gizmos.color = Color.green;
        Vector3 LeftLateralVector = new Vector3(CloseZoneRadius * 2, BoxHeight, CloseZoneRadius * 2);
        CenterVector = PlayerTransform.position + new Vector3(0, 0, CloseZoneRadius * 2);
        CenterVector = transform.InverseTransformPoint(CenterVector);
        Gizmos.DrawCube(CenterVector, LeftLateralVector);

        // Right Lateral
        Vector3 RightLateralVector = new Vector3(CloseZoneRadius * 2, BoxHeight, CloseZoneRadius * 2);
        CenterVector = PlayerTransform.position - new Vector3(0, 0, CloseZoneRadius * 2);
        CenterVector = transform.InverseTransformPoint(CenterVector);
        Gizmos.DrawCube(CenterVector, RightLateralVector);

        // Left Up Peripheral
        Gizmos.color = Color.yellow;
        Vector3 LeftUpPeripheralVector = new Vector3(CloseZoneRadius * 2, BoxHeight, CloseZoneRadius * 2);
        CenterVector = PlayerTransform.position + new Vector3(CloseZoneRadius * 2, 0, CloseZoneRadius * 2);
        CenterVector = transform.InverseTransformPoint(CenterVector);
        Gizmos.DrawCube(CenterVector, LeftUpPeripheralVector);

        // Right Up Peripheral
        Vector3 RightUpPeripheralVector = new Vector3(CloseZoneRadius * 2, BoxHeight, CloseZoneRadius * 2);
        CenterVector = PlayerTransform.position + new Vector3(-CloseZoneRadius * 2, 0, CloseZoneRadius * 2);
        CenterVector = transform.InverseTransformPoint(CenterVector);
        Gizmos.DrawCube(CenterVector, RightUpPeripheralVector);

        // Left Down Peripheral
        Vector3 LeftDownPeripheralVector = new Vector3(CloseZoneRadius * 2, BoxHeight, CloseZoneRadius * 2);
        CenterVector = PlayerTransform.position + new Vector3(CloseZoneRadius * 2, 0, -CloseZoneRadius * 2);
        CenterVector = transform.InverseTransformPoint(CenterVector);
        Gizmos.DrawCube(CenterVector, LeftDownPeripheralVector);

        // Right Down Peripheral
        Vector3 RightDownPeripheralVector = new Vector3(CloseZoneRadius * 2, BoxHeight, CloseZoneRadius * 2);
        CenterVector = PlayerTransform.position + new Vector3(-CloseZoneRadius * 2, 0, -CloseZoneRadius * 2);
        CenterVector = transform.InverseTransformPoint(CenterVector);
        Gizmos.DrawCube(CenterVector, RightDownPeripheralVector);

        // Settings resetten
        Gizmos.color = prevColor;
        Gizmos.matrix = prevMatrix;
    }

}
