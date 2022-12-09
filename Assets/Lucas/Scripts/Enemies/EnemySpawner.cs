using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Transform Player;
    public int EnemyWavesToSpawn = 5;
    public int EnemyWaveSize = 2;
    public int EnemiesThatHaveSpawned = 0;
    private int EnemyMaxAmount;
    public float SpawnDelay = 1f;
    public List<Enemy> EnemyPrefabs = new List<Enemy>();
    public SpawnMethod EnemySpawnMethod = SpawnMethod.RoundRobin;

    [SerializeField]
    private Collider[] SpawnCollider;
    private Bounds Bounds;
    public LocationMethod EnemyLocationMethod = LocationMethod.Collider;

    private NavMeshTriangulation Triangulation;
    public Dictionary<int, ObjectPool> EnemyObjectPools = new Dictionary<int, ObjectPool>();

    private void Awake()
    {
        EnemyMaxAmount = EnemyWavesToSpawn * EnemyWaveSize;

        for (int i = 0; i < EnemyPrefabs.Count; i++)
        {
            EnemyObjectPools.Add(i, ObjectPool.CreateInstance(EnemyPrefabs[i], EnemyMaxAmount));
        }
    }

    private void Start()
    {
        Triangulation = NavMesh.CalculateTriangulation();

        SetBounds();

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds Wait = new WaitForSeconds(SpawnDelay);

        int SpawnedEnemies = 0;

        while (SpawnedEnemies < EnemyMaxAmount)
        {
            for(int i = 0; i < EnemyWaveSize; i++)
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

    private void SpawnRoundRobinEnemy(int SpawnedEnemies)
    {
        int SpawnIndex = SpawnedEnemies % EnemyPrefabs.Count;

        if(EnemyLocationMethod == LocationMethod.Collider)
        {
            DoSpawnEnemy(SpawnIndex, GetRandomPositionInBounds());
        }
        else if(EnemyLocationMethod == LocationMethod.Random)
        {
            DoSpawnEnemy(SpawnIndex, ChooseRandomPositionOnNavMesh());
        }
    }

    private void SpawnRandomEnemy()
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

    private void DoSpawnEnemy(int SpawnIndex, Vector3 SpawnPosition)
    {
        PoolableObject poolableObject = EnemyObjectPools[SpawnIndex].GetObject();

        SetBounds();

        if (poolableObject != null)
        {
            Enemy enemy = poolableObject.GetComponent<Enemy>();

            NavMeshHit Hit;
            if (NavMesh.SamplePosition(SpawnPosition, out Hit, 2f, -1))
            {
                enemy.Agent.Warp(Hit.position);
                // enemy needs to get enabled and start chasing now.
                if(enemy.Movement.playerTarget == null)
                {
                    enemy.Movement.playerTarget = Player;
                }
                enemy.Agent.enabled = true;
                enemy.Movement.StartChasing(enemy.Movement.playerTarget.position);
                enemy.Movement.RetreatPosition = enemy.transform.position;

                EnemiesThatHaveSpawned += 1;
            }
            else
            {
                Debug.LogError($"Unable to place NavMeshAgent on NavMesh. Tried to use {SpawnPosition}");
            }
        }
        else
        {
            Debug.LogError($"Unable to fetch enemy of type {SpawnIndex} from object pool. Out of objects?");
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