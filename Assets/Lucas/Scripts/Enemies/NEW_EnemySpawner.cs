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
    private EnemySpawnerData[] _enemySpawnerData;
    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private LayerMask _mapConstraintsLayer;
    [SerializeField]
    private float _spawnTick;
    [SerializeField]
    private int _spawnsPerTick;
    [SerializeField]
    private int _minEnemyCount;
    [SerializeField]
    public int _maxEnemyCount;
    private float _spawnTimer = 0;
    private bool _canSpawnEnemies = true;

    [Header("Spawn Delay")]
    [SerializeField]
    private float _spawnAnimDelay;
    [SerializeField]
    private float _minSpawnDelay;
    [SerializeField]
    private float _maxSpawnDelay;

    public List<EnemiesToSpawn> _EnemiesToSpawn = new List<EnemiesToSpawn>();

    // Variablen
    private float _boxHeight;
    [Header("Variables")]
    [SerializeField]
    private float _safeZoneSquareSize;
    [SerializeField]
    private float _closeZoneSquareSize;
    [SerializeField]
    private float _midZoneSquareSize;
    [SerializeField]
    private float _farZoneSquareSize;

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
    private NavMeshTriangulation Triangulation;

    private void Awake()
    {
        if (PlayerTransform == null)
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        Triangulation = NavMesh.CalculateTriangulation();

        SetColliderSizeCenter();
    }

    private void Start()
    {
        for (int i = 0; i < _EnemiesToSpawn.Count; i++)
        {
            EnemyObjectPools.Add(i, ObjectPool<Enemy>.CreatePool(_EnemiesToSpawn[i].Enemy, _maxEnemyCount,transform.parent));
        }
    }

    private void Update()
    {
        transform.SetPositionAndRotation(PlayerTransform.position, PlayerTransform.rotation); // performance heavy?

        if (_spawnTimer >= _spawnTick)
        {
            int currentEnemiesFromMin = Mathf.RoundToInt((_minEnemyCount - GameManager.Instance._enemyCount));
            int spawnIndex = 0;

            foreach (EnemiesToSpawn enemy in _EnemiesToSpawn)
            {
                int enemiesToBeSpawned;
                enemiesToBeSpawned = 0;

                if (GameManager.Instance._enemyCount + _spawnsPerTick > _minEnemyCount)
                {
                    enemiesToBeSpawned = Mathf.RoundToInt(_spawnsPerTick * (enemy.SpawnChance * 0.01f) + 0.4f);
                }
                else if (GameManager.Instance._enemyCount < _minEnemyCount)
                {
                    enemiesToBeSpawned = Mathf.RoundToInt(currentEnemiesFromMin * (enemy.SpawnChance * 0.01f) + 0.4f);
                }
                else if (GameManager.Instance._enemyCount < _maxEnemyCount)
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
        Debug.Log(enemiesToBeSpawned);

        int zoneNumber = Random.Range(0,3);

        for (int i = 0; i < enemiesToBeSpawned; i++)
        {
            float spawnDelay = Random.Range(_minSpawnDelay, _maxSpawnDelay);

            SetBounds(enemies.SpawnBias, zoneNumber);
            StartCoroutine(DoSpawnEnemy(enemies, spawnIndex, GetRandomPositionInBounds(Bounds), spawnDelay));

            zoneNumber++;
            if (zoneNumber > 3)
                zoneNumber = 0;

            GameManager.Instance._enemyCount++;
            if (GameManager.Instance._enemyCount >= _maxEnemyCount)
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
            case SpawnBias.Level:             
                Bounds = LevelZone[Random.Range(0, LevelZone.Count)].bounds;
                break;
        }
    }

    private void DeterminePossibleBound(SpawnBias spawnBias, int zoneNumber, int attempt)
    {
        if (Physics.CheckBox(Bounds.center, Bounds.extents / 2, Quaternion.identity, _mapConstraintsLayer))
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
            switch (spawnBias)
            {
                case SpawnBias.Close:
                    Bounds = CloseZones[zoneNumber].bounds;
                    break;
                case SpawnBias.Mid:
                    Bounds = MidZones[zoneNumber].bounds;
                    break;
                case SpawnBias.Far:
                    Bounds = FarZones[zoneNumber].bounds;
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

            yield return new WaitForSeconds(_spawnAnimDelay);

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
