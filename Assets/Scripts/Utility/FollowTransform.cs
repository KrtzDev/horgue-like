using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField]
    private Transform _transform;

    void Update()
    {
        if(_transform)
            transform.position = _transform.position;
    }
}
