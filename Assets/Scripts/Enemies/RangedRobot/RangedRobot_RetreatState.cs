using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedRobot_RetreatState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Enemy enemy;
    Transform player;
    Transform decoy;
    ObstacleAgent _obstacleAgent;

    private Vector3 _followPosition;

    float retreatDistance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        enemy = animator.GetComponent<Enemy>();
        _obstacleAgent = animator.GetComponent<ObstacleAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        decoy = GameObject.FindGameObjectWithTag("Decoy").transform;

        retreatDistance = Random.Range(enemy.EnemyData._retreatRange + 1, enemy.EnemyData._attackRange - 1);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!enemy.FollowDecoy)
        {
            _followPosition = new Vector3(player.position.x, player.position.y, player.position.z);
        }
        else
        {
            _followPosition = new Vector3(decoy.position.x, decoy.position.y, decoy.position.z);
        }

        Vector3 dirToPlayer = animator.transform.position - _followPosition;
        Vector3 newPos = animator.transform.position + dirToPlayer;

        animator.transform.LookAt(newPos);
        if (_obstacleAgent.enabled)
            _obstacleAgent.SetDestination(newPos);

        float distance = Vector3.Distance(animator.transform.position, _followPosition);

        if (distance > retreatDistance)
        {
            animator.SetBool("isRetreating", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
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
}
