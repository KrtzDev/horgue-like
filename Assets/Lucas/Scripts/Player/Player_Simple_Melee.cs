using System.Collections;
using UnityEngine;

public class Player_Simple_Melee : MonoBehaviour
{
    [SerializeField]
    private GameObject Weapon1;
    [SerializeField]
    private GameObject Weapon2;
    [SerializeField]
    private bool dualAttack;

    private Vector3 startWeapon1Pos;
    private Vector3 startWeapon2Pos;

    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _attackDelay;
    [SerializeField]
    private float _attackRetreat;
    public bool canAttack = true;
    public bool didDamage = false;

    private float _currentAttackDelay;

    private void Awake()
    {
        if (Weapon1 != null && Weapon2 != null)
        {
            startWeapon1Pos = Weapon1.transform.position;
            startWeapon2Pos = Weapon2.transform.position;
        }
    }

    private void FixedUpdate()
    {
        _currentAttackDelay -= Time.deltaTime;
        if (_currentAttackDelay <= 0 && canAttack)
        {
            float currentclosestdistance = Mathf.Infinity;
            Enemy closestEnemy = null;

            Collider[] enemies = Physics.OverlapSphere(transform.position, _attackRange, _enemyLayer);
            foreach (var enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < currentclosestdistance)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, (enemy.transform.position - transform.position), out hit, distanceToEnemy, _groundLayer))
                    {
                    }
                    else
                    {
                        closestEnemy = enemy.GetComponent<Enemy>();
                        currentclosestdistance = distanceToEnemy;
                    }
                }
            }

            if (closestEnemy)
            {
                Attack(closestEnemy);
                _currentAttackDelay = _attackDelay;
            }
        }
    }
    private void Attack(Enemy enemy)
    {
        if (dualAttack)
        {
            if (Weapon1 != null && Weapon2 != null)
            {
                canAttack = false;

                Weapon1.transform.localPosition += new Vector3(0,0, _attackRange);
                Weapon2.transform.localPosition += new Vector3(0,0, _attackRange);

                StartCoroutine(StartAttackReset());
            }
            else
            {
                Debug.Log("Weapon 1 or 2 is not equipped");
            }
        }
        else
        {
            if(Weapon1 != null)
            {
                Weapon1.transform.localPosition += new Vector3(0, 0, _attackRange);
            }
            else
            {
                Debug.Log("No Weapon 1 equipped");
            }
        }
    }

    private IEnumerator StartAttackReset()
    {
        yield return new WaitForSeconds(_attackRetreat);

        AttackReset();
    }

    private void AttackReset()
    {
        if(Weapon1 != null)
        {
            Weapon1.transform.localPosition = startWeapon1Pos;
        }

        if(Weapon2 != null)
        {
            Weapon2.transform.localPosition = startWeapon2Pos;
        }

        canAttack = true;
        didDamage = false;
    }

    #region Draw_Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
    #endregion
}
