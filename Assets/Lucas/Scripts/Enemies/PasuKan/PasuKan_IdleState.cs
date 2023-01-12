using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasuKan_IdleState : StateMachineBehaviour
{
    Enemy enemy;
    Transform player;
    Transform decoy;

    private Vector3 _followPosition;

    private float _attackTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        decoy = GameObject.FindGameObjectWithTag("Decoy").transform;

        _attackTimer = enemy.EnemyData._attackSpeed;
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

        float distance = Vector3.Distance(animator.transform.position, _followPosition);

        if (distance > enemy.EnemyData._attackRange)
        {
            animator.SetBool("isChasing", true);
        }
        else if (_attackTimer < 0)
        {
            _attackTimer = enemy.EnemyData._attackSpeed;
            animator.SetTrigger("attack");
        }
        else
        {
            _attackTimer -= Time.deltaTime;
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
