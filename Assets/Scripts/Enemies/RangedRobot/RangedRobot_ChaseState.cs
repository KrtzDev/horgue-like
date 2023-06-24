using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedRobot_ChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Enemy enemy;
    Transform player;
    Transform decoy;

    private Vector3 _followPosition;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        enemy = animator.GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        decoy = GameObject.FindGameObjectWithTag("Decoy").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!enemy.FollowDecoy)
        {
            _followPosition = new Vector3(player.position.x, player.position.y, player.position.z);
            if (agent.enabled)
                agent.SetDestination(_followPosition);
        }
        else
        {
            _followPosition = new Vector3(decoy.position.x, decoy.position.y, decoy.position.z);
            if (agent.enabled)
                agent.SetDestination(_followPosition);
        }

        float distance = Vector3.Distance(animator.transform.position, _followPosition);

        // CirclePlayer(animator);

        RaycastHit hit;
        Debug.DrawRay(enemy.ProjectilePoint.transform.position, (_followPosition - enemy.ProjectilePoint.transform.position));
        if (Physics.Raycast(enemy.ProjectilePoint.transform.position, (_followPosition + new Vector3(0, 0.5f, 0) - enemy.ProjectilePoint.transform.position), out hit, distance, enemy.GroundLayer))
        {
            if (distance < enemy.EnemyData._retreatRange)
            {
                animator.SetBool("isRetreating", true);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isChasing", false);
            }
        }
        else
        {
            if (distance < enemy.EnemyData._attackRange && distance > enemy.EnemyData._retreatRange)
            {
                animator.SetBool("isAttacking", true);
                animator.SetBool("isChasing", false);
                animator.SetBool("isRetreating", false);
            }
            else if (distance < enemy.EnemyData._retreatRange)
            {
                animator.SetBool("isRetreating", true);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isChasing", false);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.enabled)
            agent.SetDestination(agent.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    /* private void CirclePlayer(Animator animator)
    {
        float currentclosestdistance = Mathf.Infinity;
        Enemy closestEnemy = null;

        Collider[] enemies = Physics.OverlapSphere(enemy.transform.position, enemy.enemyData._attackRange, enemy.EnemyLayer);
        foreach (var rangedRobot in enemies)
        {
            if (rangedRobot.gameObject.name.Contains("RangedRobot") && rangedRobot.gameObject != enemy.gameObject)
            {
                Debug.Log("found Name");

                float distanceToEnemy = Vector3.Distance(enemy.transform.position, rangedRobot.transform.position);
                if (distanceToEnemy < currentclosestdistance)
                {
                    closestEnemy = rangedRobot.GetComponent<Enemy>();
                    currentclosestdistance = distanceToEnemy;
                }
            }
        }

        if (closestEnemy)
        {
           
            Vector3 dirToClosestEnemy = _followPosition - animator.transform.position;
            Vector3 newPos = _followPosition - dirToClosestEnemy;

            animator.transform.LookAt(newPos);
            agent.SetDestination(newPos);
        }
    }
    */
}
