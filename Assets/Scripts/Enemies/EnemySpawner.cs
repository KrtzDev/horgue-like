using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

public class EnemySpawner : MonoBehaviour
{
    public GameObject _enemySpawnIndicator;
    public GameObject _enemyObjectPoolParent;
    private Transform _playerTransform;

    [Header("Settings")]
    [SerializeField] public EnemySpawnerData _enemySpawnerData;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _groundLayer;
    private float _spawnTimer = 0;
    private bool _canSpawnEnemies = true;
    private float _boxHeight;

    public List<EnemiesToSpawn> _enemiesToSpawn = new List<EnemiesToSpawn>();

    [Header("Box Colliders")]
    [SerializeField] private BoxCollider _safeZone;
    [SerializeField] private List<BoxCollider> _closeZones = new List<BoxCollider>();
    [SerializeField] private List<BoxCollider> _midZones = new List<BoxCollider>();
    [SerializeField] private List<BoxCollider> _farZones = new List<BoxCollider>();
    [SerializeField] private List<BoxCollider> _levelZone = new List<BoxCollider>();

    // Object Pooling
    public Dictionary<int, ObjectPool<Enemy>> _enemyObjectPool = new Dictionary<int, ObjectPool<Enemy>>();
    private Bounds _bounds;

    private void Awake()
    {
        if (_playerTransform == null)
            _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        SetColliderSizeCenter();
    }

    private void Start()
    {
        for (int i = 0; i < _enemiesToSpawn.Count; i++)
        {
            _enemyObjectPool.Add(i, ObjectPool<Enemy>.CreatePool(_enemiesToSpawn[i].Enemy, _enemySpawnerData._maxEnemyCount, _enemyObjectPoolParent.transform));
        }
    }

    private void Update()
    {
        transform.SetPositionAndRotation(_playerTransform.position, _playerTransform.rotation); // performance heavy?

        if (_spawnTimer >= _enemySpawnerData._spawnTick)
        {
            int currentEnemiesFromMin = Mathf.RoundToInt((_enemySpawnerData._minEnemyCount - GameManager.Instance._enemyCount));
            int spawnIndex = 0;

            foreach (EnemiesToSpawn enemy in _enemiesToSpawn)
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
        int zoneNumber = Random.Range(0,3);

        for (int i = 0; i < enemiesToBeSpawned; i++)
        {
            float spawnDelay = Random.Range(_enemySpawnerData._minSpawnDelay, _enemySpawnerData._maxSpawnDelay);

            SetBounds(enemies.SpawnBias, zoneNumber);
            StartCoroutine(DoSpawnEnemy(enemies, spawnIndex, GetRandomPositionInBounds(_bounds), spawnDelay));

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
                _bounds = _closeZones[zoneNumber].bounds;
                DeterminePossibleBound(spawnBias, zoneNumber, 0);
                break;
            case SpawnBias.Mid:
                _bounds = _midZones[zoneNumber].bounds;
                DeterminePossibleBound(spawnBias, zoneNumber, 0);
                break;
            case SpawnBias.Far:
                _bounds = _farZones[zoneNumber].bounds;
                DeterminePossibleBound(spawnBias, zoneNumber, 0);
                break;
            case SpawnBias.Level: // Spawns the Enemies at the furthest away LevelSpawnPoint
                DetermineRandomSpawnLocation();
                break;
        }
    }

    public class RandomSpawnLocation{
        public int Number;
        public float DistanceToSpawnPosition;

        public RandomSpawnLocation (int number, float distanceToSpawnPosition)
        {
            Number = number;
            DistanceToSpawnPosition = distanceToSpawnPosition;
        }
    }

    private void DetermineRandomSpawnLocation()
    {
        List<RandomSpawnLocation> randomSpawnLocation = new List<RandomSpawnLocation>();
        int zoneNumber = 0;
        float distanceToBounds = 0;
        float distanceToMidZone = _enemySpawnerData._midZoneSize * Mathf.Sqrt(2);

        for (int i = 0; i < _levelZone.Count; i++)
        {
            distanceToBounds = Vector3.Distance(new Vector3(_levelZone[i].transform.position.x, _playerTransform.position.y, _levelZone[i].transform.position.z), _playerTransform.position);

            if (distanceToBounds > distanceToMidZone)
            {
                randomSpawnLocation.Add(new RandomSpawnLocation(i, distanceToBounds));
            }
        }

        if (randomSpawnLocation.Count > 0)
        {
            int removeNumber = -1;
            float lowestDistance = Mathf.Infinity;
            List<int> bestSpawnNumbers = new List<int>();

            zoneNumber = Random.Range(0, _levelZone.Count);

            for (int i = 0; i < Mathf.RoundToInt(Mathf.RoundToInt(randomSpawnLocation.Count * 0.575f)); i++)
            {
                for (int j = 0; j < randomSpawnLocation.Count; j++)
                {
                    if (lowestDistance > randomSpawnLocation[j].DistanceToSpawnPosition)
                    {
                        zoneNumber = randomSpawnLocation[j].Number;
                        removeNumber = j;
                    }
                }

                bestSpawnNumbers.Add(zoneNumber);
                if(removeNumber >= 0)
                {
                    randomSpawnLocation.RemoveAt(removeNumber);
                }
            }

            if(bestSpawnNumbers.Count > 0)
            {
                zoneNumber = bestSpawnNumbers[Random.Range(0, bestSpawnNumbers.Count)];
            }

            _bounds = _levelZone[zoneNumber].bounds;  
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
                max1 = new Vector3(_bounds.center.x - _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z + _bounds.size.z / 2);
                max2 = new Vector3(_bounds.center.x + _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z + _bounds.size.z / 2);
                min1 = new Vector3(_bounds.center.x + _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z - _bounds.size.z / 2);
                min2 = new Vector3(_bounds.center.x - _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z - _bounds.size.z / 2);
                break;
            case 1:
                max1 = new Vector3(_bounds.center.x - _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z - _bounds.size.z / 2);
                max2 = new Vector3(_bounds.center.x - _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z + _bounds.size.z / 2);
                min1 = new Vector3(_bounds.center.x + _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z + _bounds.size.z / 2);
                min2 = new Vector3(_bounds.center.x + _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z - _bounds.size.z / 2);
                break;
            case 2:
                max1 = new Vector3(_bounds.center.x + _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z - _bounds.size.z / 2);
                max2 = new Vector3(_bounds.center.x - _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z - _bounds.size.z / 2);
                min1 = new Vector3(_bounds.center.x - _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z + _bounds.size.z / 2);
                min2 = new Vector3(_bounds.center.x + _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z + _bounds.size.z / 2);
                break;
            case 3:
                max1 = new Vector3(_bounds.center.x + _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z + _bounds.size.z / 2);
                max2 = new Vector3(_bounds.center.x + _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z - _bounds.size.z / 2);
                min1 = new Vector3(_bounds.center.x - _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z - _bounds.size.z / 2);
                min2 = new Vector3(_bounds.center.x - _bounds.size.x / 2, _bounds.center.y + _bounds.size.y / 2, _bounds.center.z + _bounds.size.z / 2);
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
                    for (int i = 0; i < _closeZones.Count; i++)
                    {
                        Collider[] enemyHitColliders = Physics.OverlapBox(_closeZones[i].center, _closeZones[i].size / 2, Quaternion.identity, _enemyLayer);
                        _enemyCountInCollider += enemyHitColliders.Length;
                    }

                    if(_enemyCountInCollider < _enemySpawnerData._maxCloseZoneOcc)
                    {
                        _bounds = _closeZones[zoneNumber].bounds;
                    }
                    else
                    {
                        DeterminePossibleBound(SpawnBias.Mid, 0, 0);
                    }
                    break;
                case SpawnBias.Mid:
                    for (int i = 0; i < _midZones.Count; i++)
                    {
                        Collider[] enemyHitColliders = Physics.OverlapBox(_midZones[i].center, _midZones[i].size / 2, Quaternion.identity, _enemyLayer);
                        _enemyCountInCollider += enemyHitColliders.Length;
                    }

                    if (_enemyCountInCollider < _enemySpawnerData._maxMidZoneOcc)
                    {
                        _bounds = _midZones[zoneNumber].bounds;
                    }
                    else
                    {
                        DeterminePossibleBound(SpawnBias.Far, 0, 0);
                    }
                    break;
                case SpawnBias.Far:
                    for (int i = 0; i < _farZones.Count; i++)
                    {
                        Collider[] enemyHitColliders = Physics.OverlapBox(_farZones[i].center, _farZones[i].size / 2, Quaternion.identity, _enemyLayer);
                        _enemyCountInCollider += enemyHitColliders.Length;
                    }

                    if (_enemyCountInCollider < _enemySpawnerData._maxFarZoneOcc)
                    {
                        _bounds = _farZones[zoneNumber].bounds;
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

        float xValueInBounds = Random.Range(_bounds.min.x, _bounds.max.x);
        float zValueInBounds = Random.Range(_bounds.min.z, _bounds.max.z);

        float DistanceToPlayer = Vector3.Distance(new Vector3(xValueInBounds, _playerTransform.position.y, zValueInBounds), _playerTransform.position);

        RaycastHit rc_hit;
        Physics.Raycast(new Vector3(xValueInBounds, _bounds.max.y, zValueInBounds), Vector3.down, out rc_hit, _boxHeight, _groundLayer);

        float yValue = _bounds.max.y - rc_hit.distance;
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
                Instantiate(_enemySpawnIndicator, Hit.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(_enemySpawnerData._spawnAnimDelay);

            Enemy poolableObject = _enemyObjectPool[spawnIndex].GetObject();

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
        center = _playerTransform.position + new Vector3(0, 0, 0);
        _safeZone.size = size;
        _safeZone.center = center;

        // Close Zone

        size_x = _enemySpawnerData._closeZoneSize - (_enemySpawnerData._closeZoneSize - _enemySpawnerData._safeZoneSize) / 2;
        size_y = (_enemySpawnerData._closeZoneSize - _enemySpawnerData._safeZoneSize) / 2;
        center_x = -(_enemySpawnerData._safeZoneSize / 2 + ((_enemySpawnerData._closeZoneSize - _enemySpawnerData._safeZoneSize) / 4) - _enemySpawnerData._safeZoneSize / 2);
        center_y = _enemySpawnerData._safeZoneSize / 2 + (_enemySpawnerData._closeZoneSize - _enemySpawnerData._safeZoneSize) / 4;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = _playerTransform.position + new Vector3(center_x, 0, center_y);
        _closeZones[0].size = size;
        _closeZones[0].center = center;

        size = new Vector3(size_y, _boxHeight, size_x);
        center = _playerTransform.position + new Vector3(-center_y, 0, center_x);
        _closeZones[1].size = size;
        _closeZones[1].center = center;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = _playerTransform.position + new Vector3(-center_x, 0, -center_y);
        _closeZones[2].size = size;
        _closeZones[2].center = center;

        size = new Vector3(size_y, _boxHeight, size_x);
        center = _playerTransform.position + new Vector3(center_y, 0, -center_x);
        _closeZones[3].size = size;
        _closeZones[3].center = center;

        // Mid Zone

        size_x = _enemySpawnerData._midZoneSize - (_enemySpawnerData._midZoneSize - _enemySpawnerData._closeZoneSize) / 2;
        size_y = (_enemySpawnerData._midZoneSize - _enemySpawnerData._closeZoneSize) / 2;
        center_x = -(_enemySpawnerData._closeZoneSize / 2 + ((_enemySpawnerData._midZoneSize - _enemySpawnerData._closeZoneSize) / 4) - _enemySpawnerData._closeZoneSize / 2);
        center_y = _enemySpawnerData._closeZoneSize / 2 + (_enemySpawnerData._midZoneSize - _enemySpawnerData._closeZoneSize) / 4;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = _playerTransform.position + new Vector3(center_x, 0, center_y);
        _midZones[0].size = size;
        _midZones[0].center = center;

        size = new Vector3(size_y, _boxHeight, size_x);
        center = _playerTransform.position + new Vector3(-center_y, 0, center_x);
        _midZones[1].size = size;
        _midZones[1].center = center;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = _playerTransform.position + new Vector3(-center_x, 0, -center_y);
        _midZones[2].size = size;
        _midZones[2].center = center;

        size = new Vector3(size_y, _boxHeight, size_x);
        center = _playerTransform.position + new Vector3(center_y, 0, -center_x);
        _midZones[3].size = size;
        _midZones[3].center = center;

        // Far Zone

        size_x = _enemySpawnerData._farZoneSize - (_enemySpawnerData._farZoneSize - _enemySpawnerData._midZoneSize) / 2;
        size_y = (_enemySpawnerData._farZoneSize - _enemySpawnerData._midZoneSize) / 2;
        center_x = -(_enemySpawnerData._midZoneSize / 2 + ((_enemySpawnerData._farZoneSize - _enemySpawnerData._midZoneSize) / 4) - _enemySpawnerData._midZoneSize / 2);
        center_y = _enemySpawnerData._midZoneSize / 2 + (_enemySpawnerData._farZoneSize - _enemySpawnerData._midZoneSize) / 4;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = _playerTransform.position + new Vector3(center_x, 0, center_y);
        _farZones[0].size = size;
        _farZones[0].center = center;

        size = new Vector3(size_y, _boxHeight, size_x);
        center = _playerTransform.position + new Vector3(-center_y, 0, center_x);
        _farZones[1].size = size;
        _farZones[1].center = center;

        size = new Vector3(size_x, _boxHeight, size_y);
        center = _playerTransform.position + new Vector3(-center_x, 0, -center_y);
        _farZones[2].size = size;
        _farZones[2].center = center;

        size = new Vector3(size_y, _boxHeight, size_x);
        center = _playerTransform.position + new Vector3(center_y, 0, -center_x);
        _farZones[3].size = size;
        _farZones[3].center = center;
    }
}