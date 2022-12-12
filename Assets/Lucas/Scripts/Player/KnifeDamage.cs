using UnityEngine;

public class KnifeDamage : MonoBehaviour
{
    private Simple_Knife_Attack _SimpleKnifeAttack;

    [SerializeField]
    private int baseDamage;

    private void Awake()
    {
        _SimpleKnifeAttack = this.gameObject.GetComponent<Simple_Knife_Attack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && _SimpleKnifeAttack._isAttacking)
        {
            if (other.GetComponent<HealthComponent>() != null)
            {
                other.GetComponent<HealthComponent>().TakeDamage(baseDamage);
            }
        }
    }
}
