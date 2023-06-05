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
    private bool _enableGizmos;
    [SerializeField]
    private float _spawnTick;
    [SerializeField]
    private int _spawnsPerTick;
    [SerializeField]
    private int _minEnemyCount;
    [SerializeField]
    private int _maxEnemyCount;
    private float _spawnTimer = 0;
    private bool _canSpawnEnemies = true;

    public List<EnemiesToSpawn> _EnemiesToSpawn = new List<EnemiesToSpawn>();

    // Variablen
    private float _boxHeight;
    [Header("Variables")]
    [SerializeField]
    private float _safeZoneRadius = 3f;
    [SerializeField]
    private float _closeZoneRadius = 8f;
    [SerializeField]
    private float _midZoneRadius = 16f;
    [SerializeField]
    private float _farZoneRadius = 24f;

    [Header("Box Colliders")]
    [SerializeField]
    private BoxCollider FrontalZone;
    [SerializeField]
    private BoxCollider RearZone;
    [SerializeField]
    private BoxCollider LeftLateralZone;
    [SerializeField]
    private BoxCollider RightLateralZone;
    [SerializeField]
    private BoxCollider LeftUpPeripheralZone;
    [SerializeField]
    private BoxCollider RightUpPeripheralZone;
    [SerializeField]
    private BoxCollider LeftDownPeripheralZone;
    [SerializeField]
    private BoxCollider RightDownPeripheralZone;

    [Header("Sphere Colliders")]
    [SerializeField]
    private SphereCollider SafeZone;
    [SerializeField]
    private SphereCollider CloseZone;
    [SerializeField]
    private SphereCollider MidZone;
    [SerializeField]
    private SphereCollider FarZone;

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
            EnemyObjectPools.Add(i, ObjectPool<Enemy>.CreatePool(_EnemiesToSpawn[i].Enemy, _maxEnemyCount,transform));
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
        for (int i = 0; i < enemiesToBeSpawned; i++)
        {
            SetBounds();
            if(BoundRadius(RearZone.bounds, _farZoneRadius, _midZoneRadius))
            {
                DoSpawnEnemy(enemies, spawnIndex, GetRandomPositionInBounds(RearZone.bounds, _farZoneRadius, _midZoneRadius));
            }
            GameManager.Instance._enemyCount++;
            if (GameManager.Instance._enemyCount >= _maxEnemyCount)
            {
                break;
            }
        }
    }
    private void SetBounds()
    {
        Bounds = RearZone.bounds;
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
        _boxHeight = _farZoneRadius;

        Vector3 size;
        Vector3 center;

        // Box Colliders

        size = new Vector3(_closeZoneRadius * 2, _boxHeight, _farZoneRadius);
        center = PlayerTransform.position + new Vector3(0, 0, _farZoneRadius / 2);
        FrontalZone.size = size;
        FrontalZone.center = center;

        size = new Vector3(_closeZoneRadius * 2, _boxHeight, _farZoneRadius);
        center = PlayerTransform.position - new Vector3(0, 0, _farZoneRadius / 2);
        RearZone.size = size;
        RearZone.center = center;

        size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        center = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, 0);
        LeftLateralZone.size = size;
        LeftLateralZone.center = center;

        size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        center = PlayerTransform.position - new Vector3(_closeZoneRadius * 2, 0, 0);
        RightLateralZone.size = size;
        RightLateralZone.center = center;

        size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        center = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, _closeZoneRadius * 2);
        LeftUpPeripheralZone.size = size;
        LeftUpPeripheralZone.center = center;

        size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        center = PlayerTransform.position + new Vector3(-_closeZoneRadius * 2, 0, _closeZoneRadius * 2);
        RightUpPeripheralZone.size = size;
        RightUpPeripheralZone.center = center;

        size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        center = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, -_closeZoneRadius * 2);
        LeftDownPeripheralZone.size = size;
        LeftDownPeripheralZone.center = center;

        size = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
        center = PlayerTransform.position + new Vector3(-_closeZoneRadius * 2, 0, -_closeZoneRadius * 2);
        RightDownPeripheralZone.size = size;
        RightDownPeripheralZone.center = center;

        // Collider[] hitColliders = Physics.OverlapBox(center, size / 2, Quaternion.identity, _enemyLayer);

        // Sphere Colliders

        SafeZone.radius = _safeZoneRadius;
        CloseZone.radius = _closeZoneRadius;
        // Collider[] hitColliders = Physics.OverlapSphere(transform.position, _closeZoneRadius, _enemyLayer);
        MidZone.radius = _midZoneRadius;
        // Collider[] hitColliders = Physics.OverlapSphere(transform.position, _midZoneRadius, _enemyLayer);
        FarZone.radius = _farZoneRadius;
        // Collider[] hitColliders = Physics.OverlapSphere(transform.position, _farZoneRadius, _enemyLayer);

    }

    // GIZMOS

    private void OnDrawGizmos()
    {
        if (_enableGizmos)
        {
            Vector3 _startPos;

            // Settings speichern
            Color prevColor = Gizmos.color;
            Matrix4x4 prevMatrix = Gizmos.matrix;

            // Frontal
            Gizmos.color = Color.cyan;
            Vector3 FrontalVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _farZoneRadius);
            _startPos = PlayerTransform.position + new Vector3(0, 0, _farZoneRadius / 2);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, FrontalVector);

            // Rear
            Gizmos.color = Color.magenta;
            Vector3 RearVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _farZoneRadius);
            _startPos = PlayerTransform.position - new Vector3(0, 0, _farZoneRadius / 2);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, RearVector);

            // Left Lateral
            Gizmos.color = Color.green;
            Vector3 LeftLateralVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
            _startPos = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, 0);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, LeftLateralVector);

            // Right Lateral
            Vector3 RightLateralVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
            _startPos = PlayerTransform.position - new Vector3(_closeZoneRadius * 2, 0, 0);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, RightLateralVector);

            // Left Up Peripheral
            Gizmos.color = Color.yellow;
            Vector3 LeftUpPeripheralVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
            _startPos = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, _closeZoneRadius * 2);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, LeftUpPeripheralVector);

            // Right Up Peripheral
            Vector3 RightUpPeripheralVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
            _startPos = PlayerTransform.position + new Vector3(-_closeZoneRadius * 2, 0, _closeZoneRadius * 2);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, RightUpPeripheralVector);

            // Left Down Peripheral
            Vector3 LeftDownPeripheralVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
            _startPos = PlayerTransform.position + new Vector3(_closeZoneRadius * 2, 0, -_closeZoneRadius * 2);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, LeftDownPeripheralVector);

            // Right Down Peripheral
            Vector3 RightDownPeripheralVector = new Vector3(_closeZoneRadius * 2, _boxHeight, _closeZoneRadius * 2);
            _startPos = PlayerTransform.position + new Vector3(-_closeZoneRadius * 2, 0, -_closeZoneRadius * 2);
            _startPos = transform.InverseTransformPoint(_startPos);
            Gizmos.DrawCube(_startPos, RightDownPeripheralVector);

            // Settings resetten
            Gizmos.color = prevColor;
            Gizmos.matrix = prevMatrix;
        }
    }

}
