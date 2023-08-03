using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class AI_Manager : MonoBehaviour
{
    private static AI_Manager _instance;
    public static AI_Manager Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public List<AI_Agent_Enemy> PasuKan = new List<AI_Agent_Enemy>();
    public List<AI_Agent_Enemy> RangedRobot = new List<AI_Agent_Enemy>();
    public List<AI_Agent_Enemy> Sniper = new List<AI_Agent_Enemy>();
    public List<AI_Agent_Enemy> Drone = new List<AI_Agent_Enemy>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(gameObject);
    }

    public void MakeAgentCircleTarget(List<AI_Agent_Enemy> EnemyType, Transform target, float radiusAroundTarget)
    {
        for (int i = 0; i < EnemyType.Count; i++)
        {
            float xPos = target.position.x + radiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / PasuKan.Count);
            float yPos = target.position.y;
            float zPos = target.position.z + radiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / PasuKan.Count);
            EnemyType[i]._navMeshAgent.destination = new Vector3(xPos, yPos, zPos);
        }
    }

    public IEnumerator LookAtTarget(AI_Agent agent, Vector3 followPosition, float maxTime)
    {
        Quaternion lookRotation = Quaternion.LookRotation(followPosition - agent.transform.position);

        float time = 0;

        while (time < maxTime)
        {
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, time);
            time += Time.deltaTime * agent._lookRotationSpeed;

            yield return null;
        }
    }
}
