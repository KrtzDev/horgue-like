using System.Collections.Generic;
using UnityEngine;
public class ObjectPool
{
    private GameObject _parent;
    private PoolableObject _prefab;
    private int _size;
    private List<PoolableObject> _availableObjectsPool;

    public static ObjectPool CreateInstance(PoolableObject prefab, int size)
    {
        ObjectPool pool = new ObjectPool(prefab, size);
        
        pool._parent = new GameObject(prefab + " Pool");
        pool._parent.transform.parent = GameObject.Find("EnemySpawner").transform;
        pool.CreateObjects();

        return pool;
    }

    private ObjectPool(PoolableObject prefab, int size)
    {
        this._prefab = prefab;
        this._size = size;
        _availableObjectsPool = new List<PoolableObject>(size);
    }

    public PoolableObject GetObject()
    {
        if (_availableObjectsPool.Count == 0)
        {
            CreateObject();
        }

        PoolableObject instance = _availableObjectsPool[0];

        _availableObjectsPool.RemoveAt(0);

        instance.gameObject.SetActive(true);

        return instance;
    }

    private void CreateObjects()
    {
        for (int i = 0; i < _size; i++)
        {
            CreateObject();
        }
    }

    private void CreateObject()
    {
        PoolableObject poolableObject = GameObject.Instantiate(_prefab, Vector3.zero, Quaternion.identity, _parent.transform);
        poolableObject.Parent = this;
        poolableObject.gameObject.SetActive(false); // PoolableObject handles re-adding the object to the AvailableObjects
    }

    public void ReturnObjectToPool(PoolableObject returnObject)
    {
        _availableObjectsPool.Add(returnObject);
    }
}