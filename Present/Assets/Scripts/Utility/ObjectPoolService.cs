using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolService : MonoSingleton<ObjectPoolService>
{
    private Dictionary<Type, IObjectPool> _objectPools = new Dictionary<Type, IObjectPool>();

    public T AcquireInstance<T>(GameObject prefab) where T : MonoBehaviour, IPoolable
    {
        if(!_objectPools.ContainsKey(typeof(T)))
        {
            ObjectPool<T> pool = new ObjectPool<T>(prefab);
            _objectPools.Add(typeof(T), pool);
        }

        return _objectPools[typeof(T)].AcquireInstance<T>();
    }

    public void ReleaseInstance<T>(T instance) where T : MonoBehaviour, IPoolable
    {
        if (!_objectPools.ContainsKey(typeof(T)))
        {
            Debug.LogError("Tried to release instance to a pool that does not exist!");
            return;
        }

        _objectPools[typeof(T)].ReleaseInstance(instance);
    }
}