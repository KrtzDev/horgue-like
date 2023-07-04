using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedRobot_ChaseState : StateMachineBehaviour
{
    NavMeshAgent _agent;
    ObstacleAgent _obstacleAgent;
    Enemy _enemy;
    Transform _player;
    Transform _decoy;

    private Vector3 _followPosition;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent = animator.GetComponent<NavMeshAgent>();
        _enemy = animator.GetComponent<Enemy>();
        _obstacleAgent = animator.GetComponent<ObstacleAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _decoy = GameObject.FindGameObjectWithTag("Decoy").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_enemy.FollowDecoy)
        {
            _followPosition = new Vector3(_player.position.x, _player.position.y, _player.position.z);
            if (_obstacleAgent.enabled)
                _obstacleAgent.SetDestination(_followPosition);
        }
        else
        {
            _followPosition = new Vector3(_decoy.position.x, _decoy.position.y, _decoy.position.z);
            if (_obstacleAgent.enabled)
                _obstacleAgent.SetDestination(_followPosition);
        }

        float distance = Vector3.Distance(animator.transform.position, _followPosition);

        // CirclePlayer(animator);

        RaycastHit hit;
        Debug.DrawRay(_enemy.ProjectilePoint.transform.position, (_followPosition - _enemy.ProjectilePoint.transform.position));
        if (Physics.Raycast(_enemy.ProjectilePoint.transform.position, (_followPosition + new Vector3(0, 0.5f, 0) - _enemy.ProjectilePoint.transform.position), out hit, distance, _enemy.GroundLayer))
        {
            if (distance < _enemy.EnemyData._retreatRange)
            {
                animator.SetBool("isRetreating", true);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isChasing", false);
            }
        }
        else
        {
            if (distance < _enemy.EnemyData._attackRange && distance > _enemy.EnemyData._retreatRange)
            {
                animator.SetBool("isAttacking", true);
                animator.SetBool("isChasing", false);
                animator.SetBool("isRetreating", false);
            }
            else if (distance < _enemy.EnemyData._retreatRange)
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
        if (_obstacleAgent.enabled)
            _obstacleAgent.SetDestination(_agent.transform.position);
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
