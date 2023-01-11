using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class RangedRobot_AttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Enemy enemy;
    Transform player;
    Transform decoy;

    private Vector3 _followPosition;

    private float _attackTimer;

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
        }
        else
        {
            _followPosition = new Vector3(decoy.position.x, decoy.position.y, decoy.position.z);
        }

        animator.transform.LookAt(_followPosition);

        if (_attackTimer < 0)
        {
            _attackTimer = enemy.enemyData._attackSpeed;
            animator.SetTrigger("shoot");
        }
        else
        {
            _attackTimer -= Time.deltaTime;

            float distance = Vector3.Distance(animator.transform.position, _followPosition);

            if (distance > enemy.enemyData._attackRange)
            {
                animator.SetBool("isAttacking", false);
                animator.SetBool("isChasing", false);
            }
            else if (distance < enemy.enemyData._retreatRange)
            {
                animator.SetBool("isAttacking", false);
                animator.SetBool("isChasing", false);
                animator.SetBool("isRetreating", true);
            }
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
