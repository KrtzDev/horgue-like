using UnityEngine;

public class StabDamage : MonoBehaviour
{
    private SimpleStabAttack _SimpleStabAttack;

    [SerializeField]
    private int baseDamage;

    private void Awake()
    {
        _SimpleStabAttack = this.gameObject.GetComponentInParent<SimpleStabAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && _SimpleStabAttack._isAttacking)
        {
            if (other.GetComponent<HealthComponent>() != null)
            {
                other.GetComponent<HealthComponent>().TakeDamage(baseDamage);
            }
        }
    }
}
