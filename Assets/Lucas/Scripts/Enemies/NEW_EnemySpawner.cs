using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemiesToSpawn
{
    public Enemy Enemy;
    public int SpawnChance;

    public EnemiesToSpawn (Enemy enemy, int spawnChance)
    {
        Enemy = enemy;
        SpawnChance = spawnChance;
    }
}

public class NEW_EnemySpawner : MonoBehaviour
{
    private Transform PlayerTransform;

    [Header("Settings")]
    private EnemySpawnerData[] _enemySpawnerData;
    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private float _spawnTick;
    [SerializeField]
    private int _spawnsPerTick;
    [SerializeField]
    private int _minEnemyCount;
    [SerializeField]
    public int _enemyMaxAmount;
    private float _spawnTimer = 0;
    private bool _canSpawnEnemies = true;

    public List<EnemiesToSpawn> _EnemiesToSpawn = new List<EnemiesToSpawn>();

    // Variablen
    private float _boxHeight;
    [Header("Variables")]
    [SerializeField]
    private float _safeZoneSquareSize = 3f;
    [SerializeField]
    private float _closeZoneSquareSize = 8f;
    [SerializeField]
    private float _midZoneSquareSize = 16f;
    [SerializeField]
    private float _farZoneSquareSize = 24f;

    [Header("Box Colliders")]
    [SerializeField]
    private BoxCollider SafeZone;
    [SerializeField]
    private List<BoxCollider> CloseZones = new List<BoxCollider>(4);
    [SerializeField]
    private List<BoxCollider> MidZones = new List<BoxCollider>(4);
    [SerializeField]
    private List<BoxCollider> FarZones = new List<BoxCollider>(4);


    // Object Pooling
    public Dictionary<int, ObjectPool<Enemy>> EnemyObjectPools = new Dictionary<int, ObjectPool<Enemy>>();
    private Bounds Bounds;

    private void Awake()
    {
        if (PlayerTransform == null)
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        SetColliderSizeCenter();
    }

    private void Start()
    {
        for (int i = 0; i < _EnemiesToSpawn.Count; i++)
        {
            EnemyObjectPools.Add(i, ObjectPool<Enemy>.CreatePool(_EnemiesToSpawn[i].Enemy, _enemyMaxAmount,transform.parent));
        }
    }

    private void Update()
    {
        transform.SetPositionAndRotation(PlayerTransform.position, PlayerTransform.rotation); // performance heavy?

        if (_spawnTimer >= _spawnTick)
        {
            int currentEnemies = Mathf.RoundToInt((_minEnemyCount - GameManager.Instance._enemyCount));
            int spawnIndex = 0;

            foreach (EnemiesToSpawn enemy in _EnemiesToSpawn)
            {
                int enemiesToBeSpawned;
                enemiesToBeSpawned = 0;

                if (GameManager.Instance._enemyCount < _minEnemyCount) // if enemies are slightly below minEnemyCount, there are only a few enemies spawned
                {
                    enemiesToBeSpawned = Mathf.RoundToInt(currentEnemies * (enemy.SpawnChance * 0.01f) + 0.4f);
                }
                else if (GameManager.Instance._enemyCount < _enemyMaxAmount)
                {
                    enemiesToBeSpawned = Mathf.RoundToInt(_spawnsPerTick * (enemy.SpawnChance * 0.01f) + 0.4f);
                }
                SpawnEnemies(enemy, enemiesToBeSpawned, spawnIndex);
                spawnIndex++;
            }
            _spawnTimer = 0;
        }
        _spawnTimer += Time.deltaTime;
    }

    private void SpawnEnemies(EnemiesToSpawn enemies, int enemiesToBeSpawned, int spawnIndex) // GameManager.Instance._enemyCount has to be subtracted on Enemy Death in Enemy Script
    {
        for (int i = 0; i < enemiesToBeSpawned; i++)
        {
            /* SetBounds();
            if(BoundRadius(RearZone.bounds, _farZoneRadius, _midZoneRadius))
            {
                DoSpawnEnemy(enemies, spawnIndex, GetRandomPositionInBounds(RearZone.bounds, _farZoneRadius, _midZoneRadius));
            }
            */
            GameManager.Instance._enemyCount++;
            if (GameManager.Instance._enemyCount >= _enemyMaxAmount)
            {
                break;
            }
        }
    }
    private void SetBounds()
    {
        // Bounds = RearZone.bounds;
    }

    private bool BoundRadius(Bounds bounds, float _maxZoneRadius, float _minZoneRadius)
    {
        float DistanceToPlayerMinMin = Vector3.Distance(new Vector3(bounds.min.x, PlayerTransform.position.y, bounds.min.z), PlayerTransform.position);
        float DistanceToPlayerMinMax = Vector3.Distance(new Vector3(bounds.min.x, PlayerTransform.position.y, bounds.max.z), PlayerTransform.position);
        float DistanceToPlayerMaxMin = Vector3.Distance(new Vector3(bounds.max.x, PlayerTransform.position.y, bounds.min.z), PlayerTransform.position);
        float DistanceToPlayerMaxMax = Vector3.Distance(new Vector3(bounds.max.x, PlayerTransform.position.y, bounds.max.z), PlayerTransform.position);

        List<float> Distances = new List<float>();
        Distances.Add(DistanceToPlayerMinMin);
        Distances.Add(DistanceToPlayerMinMax);
        Distances.Add(DistanceToPlayerMaxMin);
        Distances.Add(DistanceToPlayerMaxMax);

        bool inRange = false;

        foreach (float distance in Distances)
        {
            if (distance <= _maxZoneRadius && distance >= _minZoneRadius)
            {
                inRange = true;
                break;
            }
        }

        return inRange;
    }

    private Vector3 GetRandomPositionInBounds(Bounds bounds, float _maxZoneRadius, float _minZoneRadius)
    {
        _canSpawnEnemies = false;
        Vector3 possibleSpawnPosition = Vector3.zero;
        int spawnPositionAttempts = 0;

        while (!_canSpawnEnemies)
        {
            /*
             * x and z have to be in radius from the the player
             * spawn always greater than safe zone
             */

            float xValueInBounds = Random.Range(Bounds.min.x, Bounds.max.x);
            float zValueInBounds = Random.Range(Bounds.min.z, Bounds.max.z);

            float DistanceToPlayer = Vector3.Distance(new Vector3(xValueInBounds, PlayerTransform.position.y, zValueInBounds), PlayerTransform.position);

            if(DistanceToPlayer <= _maxZoneRadius && DistanceToPlayer >= _minZoneRadius)
            {
                RaycastHit rc_hit;
                Physics.Raycast(new Vector3(xValueInBounds, Bounds.max.y, zValueInBounds), Vector3.down, out rc_hit, _boxHeight, _groundLayer);

                float yValue = Bounds.max.y - rc_hit.distance;
                possibleSpawnPosition = new Vector3(xValueInBounds, yValue, zValueInBounds);

                NavMeshHit nv_hit;
                if (NavMesh.SamplePosition(possibleSpawnPosition, out nv_hit, 1.0f, NavMesh.AllAreas))
                {
                    possibleSpawnPosition = nv_hit.position;
                    _canSpawnEnemies = true;
                }
            }

            spawnPositionAttempts++;

            if(spawnPositionAttempts > 10)
            {
                break;
            }
        } 

        return possibleSpawnPosition;
    }

    private void DoSpawnEnemy(EnemiesToSpawn enemies, int spawnIndex, Vector3 spawnPosition)
    {
        if(_canSpawnEnemies)
        {
            Enemy poolableObject = EnemyObjectPools[spawnIndex].GetObject();

            // Determine Position

            if (poolableObject != null)
            {
                Animator anim = poolableObject.GetComponent<Animator>();
                NavMeshAgent agent = poolableObject.GetComponent<NavMeshAgent>();

                NavMeshHit Hit;
                if (NavMesh.SamplePosition(spawnPosition, out Hit, 2f, -1))
                {
                    agent.Warp(Hit.position);
                    agent.enabled = true;
                    anim.SetBool("isChasing", true);
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
    }

    private void SetColliderSizeCenter()
    {
        _boxHeight = _farZoneSquareSize;

        Vector3 size;
        Vector3 center;

        float size_x;
        float size_y;
        float center_x;
        float center_y;

        // Box Colliders

        // Safe Zone

        size = new Vector3(_safeZoneSquareSize, _boxHeight, _safeZoneSquareSize);
        center = PlayerTransform.position + new Vector3(0, 0, 0);
        SafeZone.size = size;
        SafeZone.center = center;

        // Close Zone

        size_x = _closeZoneSquareSize - (_closeZoneSquareSize - _safeZoneSquareSize) / 2;
        size_y = (_closeZoneSquareSize - _safeZoneSquareSize) / 2;
        center_x = -(_safeZoneSquareSize / 2 + ((_closeZoneSquareSize - _safeZoneSquareSize) / 4) - _safeZoneSquareSize / 2);
        center_y = _safeZoneSquareSize / 2 + (_closeZoneSquareSize - _safeZoneSquareSize) / 4;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        CloseZones[0].size = size;
        CloseZones[0].center = center;
        CloseZones[0].transform.rotation = Quaternion.Euler(0, 0, 0);

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        CloseZones[1].size = size;
        CloseZones[1].center = center;
        CloseZones[1].transform.rotation = Quaternion.Euler(0, 90, 0);

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        CloseZones[2].size = size;
        CloseZones[2].center = center;
        CloseZones[2].transform.rotation = Quaternion.Euler(0, 180, 0);

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        CloseZones[3].size = size;
        CloseZones[3].center = center;
        CloseZones[3].transform.rotation = Quaternion.Euler(0, 270, 0);

        // Mid Zone

        size_x = _midZoneSquareSize - (_midZoneSquareSize - _closeZoneSquareSize) / 2;
        size_y = (_midZoneSquareSize - _closeZoneSquareSize) / 2;
        center_x = -(_closeZoneSquareSize / 2 + ((_midZoneSquareSize - _closeZoneSquareSize) / 4) - _closeZoneSquareSize / 2);
        center_y = _closeZoneSquareSize / 2 + (_midZoneSquareSize - _closeZoneSquareSize) / 4;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        MidZones[0].size = size;
        MidZones[0].center = center;
        MidZones[0].transform.rotation = Quaternion.Euler(0, 0, 0);

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        MidZones[1].size = size;
        MidZones[1].center = center;
        MidZones[1].transform.rotation = Quaternion.Euler(0, 90, 0);

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        MidZones[2].size = size;
        MidZones[2].center = center;
        MidZones[2].transform.rotation = Quaternion.Euler(0, 180, 0);

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        MidZones[3].size = size;
        MidZones[3].center = center;
        MidZones[3].transform.rotation = Quaternion.Euler(0, 270, 0);

        // Far Zone

        size_x = _farZoneSquareSize - (_farZoneSquareSize - _midZoneSquareSize) / 2;
        size_y = (_farZoneSquareSize - _midZoneSquareSize) / 2;
        center_x = -(_midZoneSquareSize / 2 + ((_farZoneSquareSize - _midZoneSquareSize) / 4) - _midZoneSquareSize / 2);
        center_y = _midZoneSquareSize / 2 + (_farZoneSquareSize - _midZoneSquareSize) / 4;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        FarZones[0].size = size;
        FarZones[0].center = center;
        FarZones[0].transform.rotation = Quaternion.Euler(0, 0, 0);

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        FarZones[1].size = size;
        FarZones[1].center = center;
        FarZones[1].transform.rotation = Quaternion.Euler(0, 90, 0);

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        FarZones[2].size = size;
        FarZones[2].center = center;
        FarZones[2].transform.rotation = Quaternion.Euler(0, 180, 0);

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        FarZones[3].size = size;
        FarZones[3].center = center;
        FarZones[3].transform.rotation = Quaternion.Euler(0, 270, 0);

    }
}
