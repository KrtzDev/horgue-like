using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private EnemySpawnerData _enemySpawnerData;

    public int EnemiesThatHaveSpawned { get; set; }

    [SerializeField] private Collider[] SpawnCollider;

    public Transform Player;

    public List<Enemy> EnemyPrefabs = new List<Enemy>();
    public Dictionary<int, ObjectPool> EnemyObjectPools = new Dictionary<int, ObjectPool>();

    public SpawnMethod EnemySpawnMethod = SpawnMethod.RoundRobin;
    public LocationMethod EnemyLocationMethod = LocationMethod.Collider;

    [HideInInspector]
    public int EnemyMaxAmount;

    private Bounds Bounds;
    private NavMeshTriangulation Triangulation;

    private void Start()
    {
        EnemyMaxAmount = _enemySpawnerData._enemyWavesToSpawn * _enemySpawnerData._enemyWaveSize;

        for (int i = 0; i < EnemyPrefabs.Count; i++)
        {
            EnemyObjectPools.Add(i, ObjectPool.CreateInstance(EnemyPrefabs[i], EnemyMaxAmount));
        }

        Triangulation = NavMesh.CalculateTriangulation();

        SetBounds();

        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        Debug.Log("EnemyMaxAmount = " + EnemyMaxAmount);
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds Wait = new WaitForSeconds(_enemySpawnerData._enemySpawnDelay);

        int SpawnedEnemies = 0;

        while (SpawnedEnemies < EnemyMaxAmount)
        {
            for(int i = 0; i < _enemySpawnerData._enemyWaveSize; i++)
            {
                if (EnemySpawnMethod == SpawnMethod.RoundRobin)
                {
                    SpawnRoundRobinEnemy(SpawnedEnemies);
                }
                else if (EnemySpawnMethod == SpawnMethod.Random)
                {
                    SpawnRandomEnemy();
                }

                SpawnedEnemies++;
            }

            yield return Wait;
        }
    }

    public void SpawnRoundRobinEnemy(int spawnedEnemies)
    {
        int SpawnIndex = spawnedEnemies % EnemyPrefabs.Count;

        if(EnemyLocationMethod == LocationMethod.Collider)
        {
            DoSpawnEnemy(SpawnIndex, GetRandomPositionInBounds());
        }
        else if(EnemyLocationMethod == LocationMethod.Random)
        {
            DoSpawnEnemy(SpawnIndex, ChooseRandomPositionOnNavMesh());
        }
    }

    public void SpawnRandomEnemy()
    {
        if (EnemyLocationMethod == LocationMethod.Collider)
        {
            DoSpawnEnemy(Random.Range(0, EnemyPrefabs.Count), GetRandomPositionInBounds());
        }
        else if (EnemyLocationMethod == LocationMethod.Random)
        {
            DoSpawnEnemy(Random.Range(0, EnemyPrefabs.Count), ChooseRandomPositionOnNavMesh());
        }
    }

    private Vector3 ChooseRandomPositionOnNavMesh()
    {
        int VertexIndex = Random.Range(0, Triangulation.vertices.Length);
        return Triangulation.vertices[VertexIndex];
    }

    private Vector3 GetRandomPositionInBounds()
    {
        return new Vector3(Random.Range(Bounds.min.x, Bounds.max.x), Bounds.min.y, Random.Range(Bounds.min.z, Bounds.max.z));
    }

    private void SetBounds()
    {
        if (EnemyLocationMethod == LocationMethod.Collider)
        {
            List<int> possibleSpawns = new List<int>();

            for (int i = 0; i < SpawnCollider.Length; i++)
            {
                possibleSpawns.Add(i);
            }

            int numberInList = 0;
            bool lowestDistEmpty = true;
            float lowestDist = 0;

            for (int i = 0; i < SpawnCollider.Length; i++)
            {
                float dist = Vector3.Distance(SpawnCollider[i].transform.position, Player.position);

                if (lowestDistEmpty)
                {
                    lowestDist = dist;
                    lowestDistEmpty = false;
                }

                if (dist < lowestDist)
                {
                    lowestDist = dist;
                    numberInList = i;
                }
            }

            possibleSpawns.RemoveAt(numberInList);

            int x = Random.Range(0, possibleSpawns.Count);
            Bounds = SpawnCollider[possibleSpawns[x]].bounds;
        }
    }

    private void DoSpawnEnemy(int spawnIndex, Vector3 spawnPosition)
    {
        PoolableObject poolableObject = EnemyObjectPools[spawnIndex].GetObject();

        SetBounds();

        if (poolableObject != null)
        {
            // Enemy enemy = poolableObject.GetComponent<Enemy>();
            Animator anim = poolableObject.GetComponent<Animator>();
            NavMeshAgent agent = poolableObject.GetComponent<NavMeshAgent>();

            NavMeshHit Hit;
            if (NavMesh.SamplePosition(spawnPosition, out Hit, 2f, -1))
            {
                /* enemy.Agent.Warp(Hit.position);
                    // enemy needs to get enabled and start chasing now.
                if(enemy.Movement.PlayerTarget == null)
                {
                    enemy.Movement.PlayerTarget = Player;
                }
                enemy.Agent.enabled = true;
                enemy.Movement.StartChasing(enemy.Movement.PlayerTarget.position);
                enemy.Movement.RetreatPosition = enemy.transform.position; */

                agent.Warp(Hit.position);
                agent.enabled = true;
                anim.SetBool("isChasing", true);

                EnemiesThatHaveSpawned += 1;
            }
            else
            {
                Debug.LogError($"Unable to place NavMeshAgent on NavMesh. Tried to use {spawnPosition}");
            }
        }
        else
        {
            Debug.LogError($"Unable to fetch enemy of type {spawnIndex} from object pool. Out of objects?");
        }
    }


    public enum SpawnMethod
    {
        RoundRobin,
        Random
        // Other spawn methods can be added here
    }

    public enum LocationMethod
    {
        Collider,
        Random
        // Other location methods can be added here
    }
}