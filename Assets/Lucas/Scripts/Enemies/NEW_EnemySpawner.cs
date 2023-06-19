using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemiesToSpawn
{
    public Enemy Enemy;
    public int SpawnChance;
    public SpawnBias SpawnBias;

    public EnemiesToSpawn (Enemy enemy, int spawnChance, SpawnBias spawnBias)
    {
        Enemy = enemy;
        SpawnChance = spawnChance;
        SpawnBias = spawnBias;
    }
}

public enum SpawnBias { Close, Mid, Far, Level };

public class NEW_EnemySpawner : MonoBehaviour
{
    public GameObject EnemySpawnIndicator;
    private Transform PlayerTransform;

    [Header("Settings")]
    [SerializeField]
    public EnemySpawnerData _enemySpawnerData;
    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private LayerMask _groundLayer;
    private float _spawnTimer = 0;
    private bool _canSpawnEnemies = true;
    private float _boxHeight;

    public List<EnemiesToSpawn> _EnemiesToSpawn = new List<EnemiesToSpawn>();

    [Header("Box Colliders")]
    [SerializeField]
    private BoxCollider SafeZone;
    [SerializeField]
    private List<BoxCollider> CloseZones = new List<BoxCollider>();
    [SerializeField]
    private List<BoxCollider> MidZones = new List<BoxCollider>();
    [SerializeField]
    private List<BoxCollider> FarZones = new List<BoxCollider>();
    [SerializeField]
    private List<BoxCollider> LevelZone = new List<BoxCollider>();


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
            EnemyObjectPools.Add(i, ObjectPool<Enemy>.CreatePool(_EnemiesToSpawn[i].Enemy, _enemySpawnerData._maxEnemyCount,transform.parent));
        }
    }

    private void Update()
    {
        transform.SetPositionAndRotation(PlayerTransform.position, PlayerTransform.rotation); // performance heavy?

        Debug.DrawLine(new Vector3(FarZones[0].center.x + FarZones[0].size.x / 2, FarZones[0].center.y + FarZones[0].size.y / 2, FarZones[0].center.z - FarZones[0].size.z / 2), new Vector3(FarZones[0].center.x - FarZones[0].size.x / 2, FarZones[0].center.y + FarZones[0].size.y / 2, FarZones[0].center.z - FarZones[0].size.z / 2), Color.blue, 10f);
        Debug.DrawLine(new Vector3(FarZones[1].center.x + FarZones[1].size.x / 2, FarZones[1].center.y + FarZones[1].size.y / 2, FarZones[1].center.z + FarZones[1].size.z / 2), new Vector3(FarZones[1].center.x + FarZones[1].size.x / 2, FarZones[1].center.y + FarZones[1].size.y / 2, FarZones[1].center.z - FarZones[1].size.z / 2), Color.blue, 10f);
        Debug.DrawLine(new Vector3(FarZones[2].center.x - FarZones[2].size.x / 2, FarZones[2].center.y + FarZones[2].size.y / 2, FarZones[2].center.z + FarZones[2].size.z / 2), new Vector3(FarZones[2].center.x + FarZones[2].size.x / 2, FarZones[2].center.y + FarZones[2].size.y / 2, FarZones[2].center.z + FarZones[2].size.z / 2), Color.blue, 10f);
        Debug.DrawLine(new Vector3(FarZones[3].center.x - FarZones[3].size.x / 2, FarZones[3].center.y + FarZones[3].size.y / 2, FarZones[3].center.z - FarZones[3].size.z / 2), new Vector3(FarZones[3].center.x - FarZones[3].size.x / 2, FarZones[3].center.y + FarZones[3].size.y / 2, FarZones[3].center.z + FarZones[3].size.z / 2), Color.blue, 10f);

        if (_spawnTimer >= _enemySpawnerData._spawnTick)
        {
            int currentEnemiesFromMin = Mathf.RoundToInt((_enemySpawnerData._minEnemyCount - GameManager.Instance._enemyCount));
            int spawnIndex = 0;

            foreach (EnemiesToSpawn enemy in _EnemiesToSpawn)
            {
                int enemiesToBeSpawned;
                enemiesToBeSpawned = 0;

                if (GameManager.Instance._enemyCount + _enemySpawnerData._spawnsPerTick > _enemySpawnerData._minEnemyCount)
                {
                    enemiesToBeSpawned = Mathf.RoundToInt(_enemySpawnerData._spawnsPerTick * (enemy.SpawnChance * 0.01f) + 0.4f);
                }
                else if (GameManager.Instance._enemyCount < _enemySpawnerData._minEnemyCount)
                {
                    enemiesToBeSpawned = Mathf.RoundToInt(currentEnemiesFromMin * (enemy.SpawnChance * 0.01f) + 0.4f);
                }
                else if (GameManager.Instance._enemyCount < _enemySpawnerData._maxEnemyCount)
                {
                    enemiesToBeSpawned = Mathf.RoundToInt(_enemySpawnerData._spawnsPerTick * (enemy.SpawnChance * 0.01f) + 0.4f);
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
        Debug.Log(enemiesToBeSpawned);

        int zoneNumber = Random.Range(0,3);

        for (int i = 0; i < enemiesToBeSpawned; i++)
        {
            float spawnDelay = Random.Range(_enemySpawnerData._minSpawnDelay, _enemySpawnerData._maxSpawnDelay);

            SetBounds(enemies.SpawnBias, zoneNumber);
            StartCoroutine(DoSpawnEnemy(enemies, spawnIndex, GetRandomPositionInBounds(Bounds), spawnDelay));

            zoneNumber++;
            if (zoneNumber > 3)
                zoneNumber = 0;

            GameManager.Instance._enemyCount++;
            if (GameManager.Instance._enemyCount >= _enemySpawnerData._maxEnemyCount)
            {
                break;
            }
        }
    }
    private void SetBounds(SpawnBias spawnBias, int zoneNumber)
    {
        switch (spawnBias) // Add Detection if Zones are full
        {
            case SpawnBias.Close:
                Bounds = CloseZones[zoneNumber].bounds;
                DeterminePossibleBound(spawnBias, zoneNumber, 0);
                break;
            case SpawnBias.Mid:
                Bounds = MidZones[zoneNumber].bounds;
                DeterminePossibleBound(spawnBias, zoneNumber, 0);
                break;
            case SpawnBias.Far:
                Bounds = FarZones[zoneNumber].bounds;
                DeterminePossibleBound(spawnBias, zoneNumber, 0);
                break;
            case SpawnBias.Level: // Spawns the Enemies at the furthest away LevelSpawnPoint
                float distanceToBounds = 0;              
                for(int i = 0; i < LevelZone.Count; i++)
                {
                    if(distanceToBounds < Vector3.Distance(PlayerTransform.position, LevelZone[i].center))
                    {
                        distanceToBounds = Vector3.Distance(PlayerTransform.position, LevelZone[i].center);
                        zoneNumber = i;
                    }
                }
                Bounds = LevelZone[zoneNumber].bounds;
                break;
        }
    }

    private void DeterminePossibleBound(SpawnBias spawnBias, int zoneNumber, int attempt)
    {
        // if (Physics.CheckBox(Bounds.center, Bounds.extents / 2, Quaternion.identity, _mapConstraintsLayer)) 
        // check if 4 Ecken over Ground

        Vector3 max1 = Vector3.zero;
        Vector3 max2 = Vector3.zero;
        Vector3 min1 = Vector3.zero;
        Vector3 min2 = Vector3.zero;

        switch (zoneNumber)
        {
            case 0:
                max1 = new Vector3(Bounds.center.x - Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z + Bounds.size.z / 2);
                max2 = new Vector3(Bounds.center.x + Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z + Bounds.size.z / 2);
                min1 = new Vector3(Bounds.center.x + Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z - Bounds.size.z / 2);
                min2 = new Vector3(Bounds.center.x - Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z - Bounds.size.z / 2);
                break;
            case 1:
                max1 = new Vector3(Bounds.center.x - Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z - Bounds.size.z / 2);
                max2 = new Vector3(Bounds.center.x - Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z + Bounds.size.z / 2);
                min1 = new Vector3(Bounds.center.x + Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z + Bounds.size.z / 2);
                min2 = new Vector3(Bounds.center.x + Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z - Bounds.size.z / 2);
                break;
            case 2:
                max1 = new Vector3(Bounds.center.x + Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z - Bounds.size.z / 2);
                max2 = new Vector3(Bounds.center.x - Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z - Bounds.size.z / 2);
                min1 = new Vector3(Bounds.center.x - Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z + Bounds.size.z / 2);
                min2 = new Vector3(Bounds.center.x + Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z + Bounds.size.z / 2);
                break;
            case 3:
                max1 = new Vector3(Bounds.center.x + Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z + Bounds.size.z / 2);
                max2 = new Vector3(Bounds.center.x + Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z - Bounds.size.z / 2);
                min1 = new Vector3(Bounds.center.x - Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z - Bounds.size.z / 2);
                min2 = new Vector3(Bounds.center.x - Bounds.size.x / 2, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z + Bounds.size.z / 2);
                break;
        }

        if (
            !(Physics.Raycast(max1, Vector3.down, Mathf.Infinity, _groundLayer)) ||
            !(Physics.Raycast(max2, Vector3.down, Mathf.Infinity, _groundLayer)) ||
            !(Physics.Raycast(min1, Vector3.down, Mathf.Infinity, _groundLayer)) ||
            !(Physics.Raycast(min2, Vector3.down, Mathf.Infinity, _groundLayer))
            // !(Physics.Raycast(new Vector3(Bounds.center.x, Bounds.center.y + Bounds.size.y / 2, Bounds.center.z), Vector3.down, Mathf.Infinity, _groundLayer))
            // test this more
           )
        {
            if(attempt <= 4)
            {
                switch (zoneNumber)
                {
                    case 0:
                        DeterminePossibleBound(spawnBias, 1, attempt + 1);
                        break;
                    case 1:
                        DeterminePossibleBound(spawnBias, 2, attempt + 1);
                        break;
                    case 2:
                        DeterminePossibleBound(spawnBias, 3, attempt + 1);
                        break;
                    case 3:
                        DeterminePossibleBound(spawnBias, 0, attempt + 1);
                        break;
                }
            }
            else
            {
                switch (spawnBias)
                {
                    case SpawnBias.Close:
                        DeterminePossibleBound(SpawnBias.Mid, 0, 0);
                        break;
                    case SpawnBias.Mid:
                        DeterminePossibleBound(SpawnBias.Far, 0, 0);
                        break;
                    case SpawnBias.Far:
                        Debug.Log("Spawn Random");
                        SetBounds(SpawnBias.Level, 0);
                        break;
                }
            }
        }
        else
        {
            int _enemyCountInCollider = 0;

            switch (spawnBias)
            {
                case SpawnBias.Close:
                    for (int i = 0; i < CloseZones.Count; i++)
                    {
                        Collider[] enemyHitColliders = Physics.OverlapBox(CloseZones[i].center, CloseZones[i].size / 2, Quaternion.identity, _enemyLayer);
                        _enemyCountInCollider += enemyHitColliders.Length;
                    }

                    if(_enemyCountInCollider < _enemySpawnerData._maxCloseZoneOcc)
                    {
                        Bounds = CloseZones[zoneNumber].bounds;
                    }
                    else
                    {
                        DeterminePossibleBound(SpawnBias.Mid, 0, 0);
                    }
                    break;
                case SpawnBias.Mid:
                    for (int i = 0; i < MidZones.Count; i++)
                    {
                        Collider[] enemyHitColliders = Physics.OverlapBox(MidZones[i].center, MidZones[i].size / 2, Quaternion.identity, _enemyLayer);
                        _enemyCountInCollider += enemyHitColliders.Length;
                    }

                    if (_enemyCountInCollider < _enemySpawnerData._maxMidZoneOcc)
                    {
                        Bounds = MidZones[zoneNumber].bounds;
                    }
                    else
                    {
                        DeterminePossibleBound(SpawnBias.Far, 0, 0);
                    }
                    break;
                case SpawnBias.Far:
                    for (int i = 0; i < FarZones.Count; i++)
                    {
                        Collider[] enemyHitColliders = Physics.OverlapBox(FarZones[i].center, FarZones[i].size / 2, Quaternion.identity, _enemyLayer);
                        _enemyCountInCollider += enemyHitColliders.Length;
                    }

                    if (_enemyCountInCollider < _enemySpawnerData._maxFarZoneOcc)
                    {
                        Bounds = FarZones[zoneNumber].bounds;
                    }
                    else
                    {
                        DeterminePossibleBound(SpawnBias.Level, 0, 0);
                    }
                    break;
            }
        }
    }

    private Vector3 GetRandomPositionInBounds(Bounds bounds)
    {
        _canSpawnEnemies = false;
        Vector3 possibleSpawnPosition = Vector3.zero;

        float xValueInBounds = Random.Range(Bounds.min.x, Bounds.max.x);
        float zValueInBounds = Random.Range(Bounds.min.z, Bounds.max.z);

        float DistanceToPlayer = Vector3.Distance(new Vector3(xValueInBounds, PlayerTransform.position.y, zValueInBounds), PlayerTransform.position);

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

        return possibleSpawnPosition;
    }

    private IEnumerator DoSpawnEnemy(EnemiesToSpawn enemies, int spawnIndex, Vector3 spawnPosition, float spawnDelay)
    {
        if (_canSpawnEnemies)
        {
            yield return new WaitForSeconds(spawnDelay);

            NavMeshHit Hit;
            if (NavMesh.SamplePosition(spawnPosition, out Hit, 2f, -1))
            {
                Instantiate(EnemySpawnIndicator, Hit.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(_enemySpawnerData._spawnAnimDelay);

            Enemy poolableObject = EnemyObjectPools[spawnIndex].GetObject();

            // Determine Position

            if (poolableObject != null)
            {
                Animator anim = poolableObject.GetComponent<Animator>();
                NavMeshAgent agent = poolableObject.GetComponent<NavMeshAgent>();

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
        _boxHeight = _enemySpawnerData._farZoneSize;

        Vector3 size;
        Vector3 center;

        float size_x;
        float size_y;
        float center_x;
        float center_y;

        // Box Colliders

        // Safe Zone

        size = new Vector3(_enemySpawnerData._safeZoneSize, _boxHeight, _enemySpawnerData._safeZoneSize);
        center = PlayerTransform.position + new Vector3(0, 0, 0);
        SafeZone.size = size;
        SafeZone.center = center;

        // Close Zone

        size_x = _enemySpawnerData._closeZoneSize - (_enemySpawnerData._closeZoneSize - _enemySpawnerData._safeZoneSize) / 2;
        size_y = (_enemySpawnerData._closeZoneSize - _enemySpawnerData._safeZoneSize) / 2;
        center_x = -(_enemySpawnerData._safeZoneSize / 2 + ((_enemySpawnerData._closeZoneSize - _enemySpawnerData._safeZoneSize) / 4) - _enemySpawnerData._safeZoneSize / 2);
        center_y = _enemySpawnerData._safeZoneSize / 2 + (_enemySpawnerData._closeZoneSize - _enemySpawnerData._safeZoneSize) / 4;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        CloseZones[0].size = size;
        CloseZones[0].center = center;

        size = new Vector3(size_y, _boxHeight, size_x);
        center = PlayerTransform.position + new Vector3(-center_y, 0, center_x);
        CloseZones[1].size = size;
        CloseZones[1].center = center;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(-center_x, 0, -center_y);
        CloseZones[2].size = size;
        CloseZones[2].center = center;

        size = new Vector3(size_y, _boxHeight, size_x);
        center = PlayerTransform.position + new Vector3(center_y, 0, -center_x);
        CloseZones[3].size = size;
        CloseZones[3].center = center;

        // Mid Zone

        size_x = _enemySpawnerData._midZoneSize - (_enemySpawnerData._midZoneSize - _enemySpawnerData._closeZoneSize) / 2;
        size_y = (_enemySpawnerData._midZoneSize - _enemySpawnerData._closeZoneSize) / 2;
        center_x = -(_enemySpawnerData._closeZoneSize / 2 + ((_enemySpawnerData._midZoneSize - _enemySpawnerData._closeZoneSize) / 4) - _enemySpawnerData._closeZoneSize / 2);
        center_y = _enemySpawnerData._closeZoneSize / 2 + (_enemySpawnerData._midZoneSize - _enemySpawnerData._closeZoneSize) / 4;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        MidZones[0].size = size;
        MidZones[0].center = center;

        size = new Vector3(size_y, _boxHeight, size_x);
        center = PlayerTransform.position + new Vector3(-center_y, 0, center_x);
        MidZones[1].size = size;
        MidZones[1].center = center;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(-center_x, 0, -center_y);
        MidZones[2].size = size;
        MidZones[2].center = center;

        size = new Vector3(size_y, _boxHeight, size_x);
        center = PlayerTransform.position + new Vector3(center_y, 0, -center_x);
        MidZones[3].size = size;
        MidZones[3].center = center;

        // Far Zone

        size_x = _enemySpawnerData._farZoneSize - (_enemySpawnerData._farZoneSize - _enemySpawnerData._midZoneSize) / 2;
        size_y = (_enemySpawnerData._farZoneSize - _enemySpawnerData._midZoneSize) / 2;
        center_x = -(_enemySpawnerData._midZoneSize / 2 + ((_enemySpawnerData._farZoneSize - _enemySpawnerData._midZoneSize) / 4) - _enemySpawnerData._midZoneSize / 2);
        center_y = _enemySpawnerData._midZoneSize / 2 + (_enemySpawnerData._farZoneSize - _enemySpawnerData._midZoneSize) / 4;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(center_x, 0, center_y);
        FarZones[0].size = size;
        FarZones[0].center = center;

        size = new Vector3(size_y, _boxHeight, size_x);
        center = PlayerTransform.position + new Vector3(-center_y, 0, center_x);
        FarZones[1].size = size;
        FarZones[1].center = center;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = PlayerTransform.position + new Vector3(-center_x, 0, -center_y);
        FarZones[2].size = size;
        FarZones[2].center = center;

        size = new Vector3(size_y, _boxHeight, size_x);
        center = PlayerTransform.position + new Vector3(center_y, 0, -center_x);
        FarZones[3].size = size;
        FarZones[3].center = center;
    }
}
