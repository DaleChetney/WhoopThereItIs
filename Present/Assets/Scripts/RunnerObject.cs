using UnityEngine;
using System.Collections;

public class RunnerObject : MonoBehaviour, IPoolable
{
    public void Activate()
    {
    }

    public void Deactivate()
    {
    }

    public void Initialize()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "DaydreamPlayer")
        {
            CollideEffects();
            Despawn();
        }
    }

    void Update()
    {
        transform.position += new Vector3(-RunnerManager.Instance.scrollSpeed * Time.deltaTime, 0, -0.001f);
        if (transform.position.x < RunnerManager.Instance.transform.position.x + RunnerManager.Instance.leftBoundary)
            Despawn();
    }

    internal virtual void CollideEffects() { }
    internal virtual void Despawn() { }
}
