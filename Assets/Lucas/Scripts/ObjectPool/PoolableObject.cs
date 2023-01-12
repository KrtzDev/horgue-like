using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public ObjectPool Parent { get; set; }

    public virtual void OnDisable()
    {
        Parent.ReturnObjectToPool(this);
    }
}