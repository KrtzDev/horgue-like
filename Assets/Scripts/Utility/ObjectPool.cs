using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private GameObject _parent;
    private T _prefab;
    private int _size;
    private List<T> _availableObjectsPool;

	public int Count => _availableObjectsPool.Count;

    public static ObjectPool<T> CreatePool(T prefab, int size, Transform parentTransform)
    {
        ObjectPool<T> pool = new ObjectPool<T>(prefab, size);
        
        pool._parent = new GameObject(prefab + " Pool");
        pool._parent.transform.parent = parentTransform;
        pool.CreateObjects();

        return pool;
    }

    private ObjectPool(T prefab, int size)
    {
        this._prefab = prefab;
        this._size = size;
        _availableObjectsPool = new List<T>(size);
    }

    public T GetObject()
    {
        if (_availableObjectsPool.Count == 0)
        {
            CreateObject();
        }

        T instance = _availableObjectsPool[0];

        _availableObjectsPool.RemoveAt(0);

        instance.gameObject.SetActive(true);

        return instance;
    }

    public T GetObjectAtIndex(int index)
    {
        return _parent.transform.GetChild(index).GetComponent<T>();
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
        T poolableObject = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity, _parent.transform);
        poolableObject.gameObject.transform.SetParent(_parent.transform);
        poolableObject.gameObject.SetActive(false);
		_availableObjectsPool.Add(poolableObject);
	}

    public void ReturnObjectToPool(T returnObject)
    {
		returnObject.transform.SetParent(_parent.transform);
        _availableObjectsPool.Add(returnObject);
		returnObject.gameObject.SetActive(false);
	}

    public int ActiveCount()
    {
        int count = 0;

        for(int i = 0; i < _parent.transform.childCount; i++)
        {
            if(_parent.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                count++;
            }
        }

        return count;
    }
}