using UnityEngine;

public class RadiusDamage : MonoBehaviour
{
    private Simple_Radius_Attack _SimpleRadiusAttack;

    [SerializeField]
    private int baseDamage;

    private void Awake()
    {
        _SimpleRadiusAttack = this.gameObject.GetComponentInParent<Simple_Radius_Attack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && _SimpleRadiusAttack._isAttacking)
        {
            if (other.GetComponent<HealthComponent>() != null)
            {
                other.GetComponent<HealthComponent>().TakeDamage(baseDamage);
            }
        }
    }
}
