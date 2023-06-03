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
    private bool _enableGizmos;
    [SerializeField]
    private float _spawnTick;
    [SerializeField]
    private int _spawnsPerTick;
    [SerializeField]
    private int _minEnemyCount;
    [SerializeField]
    private int _maxEnemyCount;
    private int _spawnedEnemies;
    private float _spawnTimer = 0;

    public List<EnemiesToSpawn> _EnemiesToSpawn = new List<EnemiesToSpawn>();

    // Variablen
    private float _boxHeight;
    [Header("Variables")] 
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
    private SphereCollider CloseZone;
    [SerializeField]
    private SphereCollider MidZone;
    [SerializeField]
    private SphereCollider FarZone;

    // Object Pooling
    public Dictionary<int, ObjectPool> EnemyObjectPools = new Dictionary<int, ObjectPool>();
    private NavMeshTriangulation Triangulation;


    private void Awake()
    {
        if(PlayerTransform == null)
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        SetColliderSizeCenter();
    }

    private void Start()
    {
        for (int i = 0; i < _EnemiesToSpawn.Count; i++)
        {
            EnemyObjectPools.Add(i, ObjectPool.CreateInstance(_EnemiesToSpawn[i].Enemy, _maxEnemyCount));
        }

        Triangulation = NavMesh.CalculateTriangulation();
    }

    private void Update()
    {
        transform.SetPositionAndRotation(PlayerTransform.position, PlayerTransform.rotation);

        if(_spawnTimer >= _spawnTick)
        {
            int currentEnemies = Mathf.RoundToInt((_minEnemyCount - GameManager.Instance._enemyCount));
            int spawnIndex = 0;

            foreach (EnemiesToSpawn enemy in _EnemiesToSpawn)
            {
                int enemiesToBeSpawned;
                enemiesToBeSpawned = 0;

                if(GameManager.Instance._enemyCount <= _minEnemyCount)
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
            DoSpawnEnemy(enemies, spawnIndex);
            GameManager.Instance._enemyCount++;
            _spawnedEnemies++;
            if (GameManager.Instance._enemyCount >= _minEnemyCount)
            {
                break;
            }
        }
    }

    private void DoSpawnEnemy(EnemiesToSpawn enemies, int spawnIndex)
    {
        PoolableObject poolableObject = EnemyObjectPools[spawnIndex].GetObject();

        // Determine Position
        int VertexIndex = Random.Range(0, Triangulation.vertices.Length);
        Vector3 spawnPosition = Triangulation.vertices[VertexIndex];

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
