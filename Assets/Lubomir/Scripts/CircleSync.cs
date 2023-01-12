using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSync : MonoBehaviour
{
    public static int posID = Shader.PropertyToID("_Position");
    public static int sizeID = Shader.PropertyToID("_Size");

    [SerializeField]
    private Material wallMaterial;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private LayerMask mask;

    // Update is called once per frame
    void Update()
    {
        var dir = camera.transform.position - transform.position;
        var ray = new Ray (transform.position, dir.normalized);

        if (Physics.Raycast(ray, 3000, mask))
        {
            wallMaterial.SetFloat(sizeID, 1);
        }
        else
        {
            wallMaterial.SetFloat(sizeID, 0);
        }

        var view = camera.WorldToViewportPoint(transform.position);
        wallMaterial.SetVector(posID, view);
    }
}
