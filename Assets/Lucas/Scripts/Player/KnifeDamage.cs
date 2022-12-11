using UnityEngine;

public class KnifeDamage : MonoBehaviour
{
    private Simple_Knife_Attack _SimpleKnifeAttack;

    [SerializeField]
    private int baseDamage;
    private bool _didDamage = false;

    private void Awake()
    {
        _SimpleKnifeAttack = this.gameObject.GetComponent<Simple_Knife_Attack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !_didDamage)
        {
            if (other.GetComponent<HealthComponent>() != null)
            {
                other.GetComponent<HealthComponent>().TakeDamage(baseDamage);
                _didDamage = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && _didDamage)
        {
            _didDamage = false;
        }
    }
}
