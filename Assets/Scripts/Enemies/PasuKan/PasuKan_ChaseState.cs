using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PasuKan_ChaseState : StateMachineBehaviour
{
    NavMeshAgent _agent;
    ObstacleAgent _obstacleAgent;
    Enemy _enemy;
    Transform _player;
    Transform _decoy;

    private Vector3 _followPosition;

    private float _attackTimer;
    private float _jumpAttackTimer;

    private float oldSpeed;
    private float oldAcceleration;
    private bool rageMode = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent = animator.GetComponent<NavMeshAgent>();
        _obstacleAgent = animator.GetComponent<ObstacleAgent>();
        _enemy = animator.GetComponent<Enemy>();
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

        RaycastHit hit;
        // Debug.DrawRay(animator.transform.position + new Vector3(0, 0.5f, 0), ((_followPosition + new Vector3(0, 0.5f, 0))- (animator.transform.position + new Vector3(0, 0.5f, 0))));
        // Debug.DrawRay(animator.transform.position + new Vector3(0, 0.5f, 0), ((new Vector3(_followPosition.x, enemy.transform.position.y, _followPosition.z) + new Vector3(0, 0.5f, 0)) - (animator.transform.position + new Vector3(0, 0.5f, 0))), Color.green);

        /* if (!Physics.Raycast(enemy.transform.position + new Vector3(0, 0.5f, 0), ((new Vector3(_followPosition.x, enemy.transform.position.y, _followPosition.z) + new Vector3(0, 0.5f, 0)) - (animator.transform.position + new Vector3(0, 0.5f, 0))), out hit, distance, enemy.GroundLayer))
        {
            if (Physics.Raycast(enemy.transform.position + new Vector3(0, 0.5f, 0), Vector3.forward * distance, out hit, enemy.PlayerLayer))
            {
                if (distance < enemy.EnemyData._maxJumpAttackRange && distance > enemy.EnemyData._minJumpAttackRange && distance > enemy.EnemyData._attackRange && _jumpAttackTimer < 0)
                {
                    int random = Random.Range(0, 100);

                    if (random < enemy.EnemyData._jumpAttackChance)
                    {
                        // _jumpAttackTimer = enemy.EnemyData._jumpAttackCooldown;
                        // animator.transform.LookAt(_followPosition);
                        // animator.SetTrigger("jumpAttack");
                        // animator.SetBool("isChasing", false);                       

                        _jumpAttackTimer = enemy.EnemyData._jumpAttackCooldown;
                        animator.transform.LookAt(_followPosition);
                        animator.SetTrigger("rageMode");
                        rageMode = true;
                        oldSpeed = agent.speed;
                        agent.speed *= 2;
                    }
                    else
                    {
                        _jumpAttackTimer = enemy.EnemyData._jumpAttackCooldown;
                    }
                }
            }
        } */

        if (distance < _enemy.EnemyData._maxJumpAttackRange && distance > _enemy.EnemyData._minJumpAttackRange && distance > _enemy.EnemyData._attackRange && _jumpAttackTimer < 0 && !rageMode)
        {
            int random = Random.Range(0, 100);

            if (random < _enemy.EnemyData._jumpAttackChance)
            {
                _jumpAttackTimer = _enemy.EnemyData._jumpAttackCooldown;
                animator.transform.LookAt(_followPosition);
                animator.SetTrigger("rageMode");
                animator.SetBool("isRageMode", true);
                rageMode = true;
                oldSpeed = _agent.speed;
                oldAcceleration = _agent.acceleration;
                _agent.acceleration *= 1.5f;
                _agent.speed *= 1.5f;

                if(_enemy.GetComponent<HealthComponent>()._currentHealth > 0)
                {
                    _enemy.GetComponent<HealthComponent>()._maxHealth *= 2;
                    _enemy.GetComponent<HealthComponent>()._currentHealth = _enemy.GetComponent<HealthComponent>()._maxHealth;
                }
            }
            else
            {
                _jumpAttackTimer = _enemy.EnemyData._jumpAttackCooldown;
            }
        }

        if (_jumpAttackTimer >= 0)
        {
            _jumpAttackTimer -= Time.deltaTime;
        }
        else 
        {
            if (rageMode && _agent.speed != oldSpeed)
            {
                _agent.speed = oldSpeed;
                _agent.acceleration = oldAcceleration;
                _jumpAttackTimer = _enemy.EnemyData._jumpAttackCooldown;
                animator.SetBool("isRageMode", false);
            }
        }

        if (distance < _enemy.EnemyData._attackRange && _attackTimer < 0)
        {
            _attackTimer = _enemy.EnemyData._attackSpeed;
            animator.transform.LookAt(_followPosition);
            animator.SetTrigger("attack");
            animator.SetBool("isChasing", false);
        }
        else
        {
            _attackTimer -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_obstacleAgent.enabled)
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
}
