using System;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool
{
    void ReleaseInstance<T>(T instance) where T : MonoBehaviour, IPoolable;
    T AcquireInstance<T>() where T : MonoBehaviour, IPoolable;
}

public class ObjectPool<T> : IObjectPool where T : MonoBehaviour, IPoolable
{
    private GameObject _prefabSource;
    private Queue<T> _pooledInstances = new Queue<T>();

    public ObjectPool(GameObject prefab)
    {
        _prefabSource = prefab;
    }

    public void ReleaseInstance(T instance)
    {
        instance.Deactivate();
        _pooledInstances.Enqueue(instance);
    }

    public T AcquireInstance()
    {
        T instance;

        if (_pooledInstances.Count == 0)
        {
            var instanceObject = GameObject.Instantiate(_prefabSource);
            instance = instanceObject.GetComponent<T>();
            instance.Initialize();
        }
        else
        {
            instance = _pooledInstances.Dequeue();
        }

        instance.Activate();

        return instance;
    }

    public void ReleaseInstance<T1>(T1 instance) where T1 : MonoBehaviour, IPoolable
    {
        ReleaseInstance(instance);
    }

    public T1 AcquireInstance<T1>() where T1 : MonoBehaviour, IPoolable
    {
        T instance = AcquireInstance();

        T1 castInstance = instance as T1;
        if (castInstance == null)
        {
            throw new InvalidCastException($"Cannot acquire {nameof(T1)} from pool of {nameof(T)}");
        }

        return castInstance;
    }
}