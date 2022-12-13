using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPush : MonoBehaviour
{
    private Simple_Radius_Attack _SimpleRadiusAttack;

    private GameObject _Player;

    [SerializeField]
    private float _pushForce;

    private void Awake()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");

        _SimpleRadiusAttack = this.gameObject.GetComponentInParent<Simple_Radius_Attack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered collider");

        if (other.CompareTag("Enemy") && _SimpleRadiusAttack._isAttacking)
        {
            Debug.Log("is enemey & isattacking");

            if (other.GetComponent<Rigidbody>() != null)
            {
                Debug.Log("push");
                Vector3 pushDir = (other.transform.position - _Player.GetComponent<Transform>().position).normalized;
                other.GetComponent<Rigidbody>().AddForce(_pushForce * pushDir, ForceMode.Impulse);
            }
        }
    }
}
