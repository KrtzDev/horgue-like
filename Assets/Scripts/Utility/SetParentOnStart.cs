using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParentOnStart : MonoBehaviour
{
    [SerializeField]
    private Transform _parent;

    void Start()
    {
        transform.SetParent(_parent);
    }
}
