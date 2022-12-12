using UnityEngine;

public class RadiusDamage : MonoBehaviour
{
    private Simple_Radius_Attack SimpleRadiusAttack;

    [SerializeField]
    private int baseDamage;

    private void Awake()
    {
        SimpleRadiusAttack = this.gameObject.GetComponentInParent<Simple_Radius_Attack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && SimpleRadiusAttack._isAttacking)
        {
            if (other.GetComponent<HealthComponent>() != null)
            {
                other.GetComponent<HealthComponent>().TakeDamage(baseDamage);
            }
        }
    }
}
