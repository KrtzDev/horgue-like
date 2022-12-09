using UnityEngine;

public class KnifeDamage : MonoBehaviour
{
    private Player_Simple_Melee _PlayerSimpleMelee;

    [SerializeField]
    private int baseDamage;

    private void Awake()
    {
        _PlayerSimpleMelee = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Simple_Melee>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && _PlayerSimpleMelee.didDamage == false)
        {
            if (other.GetComponent<HealthComponent>() != null)
            {
                other.GetComponent<HealthComponent>().TakeDamage(baseDamage);
                _PlayerSimpleMelee.didDamage = true;
            }
        }
    }
}
