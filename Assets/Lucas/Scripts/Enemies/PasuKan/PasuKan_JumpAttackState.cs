using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PasuKan_JumpAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Enemy enemy;
    Transform player;
    Transform decoy;

    private Vector3 _followPosition;

    private float _jumpPrepTime;
    private float _jumpTime;
    private float _jumpFactor;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        enemy = animator.GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        decoy = GameObject.FindGameObjectWithTag("Decoy").transform;

        _jumpPrepTime = 0;
        _jumpTime = 0;
        _jumpFactor = 0;

        animator.SetFloat("jumpTime", -1);

        agent.enabled = false;

        if (!enemy.FollowDecoy)
        {
            _followPosition = new Vector3(player.position.x, player.position.y, player.position.z);
        }
        else
        {
            _followPosition = new Vector3(decoy.position.x, decoy.position.y, decoy.position.z);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_jumpPrepTime <= enemy.EnemyData._jumpPrepTime)
        {
            _jumpPrepTime += Time.deltaTime;
        }
        else
        {
            Vector3 startingPosition = animator.transform.position;

            /* if (_jumpTime <= 0.75 * enemy.EnemyData._jumpTime )
            {
                if (!enemy.FollowDecoy)
                {
                    _followPosition = new Vector3(player.position.x, player.position.y, player.position.z);
                }
                else
                {
                    _followPosition = new Vector3(decoy.position.x, decoy.position.y, decoy.position.z);
                }
            }

            */

            // Vector3 dirToPlayer = _followPosition - animator.transform.position;
            // Vector3 newPos = _followPosition - dirToPlayer.normalized * 2.5f;
            // _followPosition = newPos;

            if (_jumpTime <= enemy.EnemyData._jumpTime)
            {
                _jumpTime += Time.deltaTime;
                _jumpFactor += Time.deltaTime * enemy.EnemyData._jumpForce;

                animator.transform.position = Vector3.Lerp(startingPosition, _followPosition, _jumpFactor) + Vector3.up * enemy.EnemyData.HeightCurve.Evaluate(_jumpTime) * enemy.EnemyData._jumpForce;
                animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, Quaternion.LookRotation(_followPosition - animator.transform.position), _jumpFactor);
                animator.SetFloat("jumpTime", _jumpTime);

                if(animator.transform.position == _followPosition)
                {
                    animator.SetTrigger("jumpAttackEnded");
                }
            }
            else
            {
                animator.SetTrigger("jumpAttackEnded");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.enabled = true;

        if (NavMesh.SamplePosition(_followPosition, out NavMeshHit hit, 1f, agent.areaMask))
        {
            agent.Warp(hit.position);
            animator.SetBool("isChasing", true);
        }
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
