using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour, IPoolable
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

    void Update()
    {
        transform.position += new Vector3(-RunnerManager.Instance.scrollSpeed * Time.deltaTime, 0, -0.001f);
        if (transform.position.x < RunnerManager.Instance.transform.position.x + RunnerManager.Instance.leftBoundary)
            ReleaseSelf();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.name == "DaydreamPlayer")
		{
            //ResponseManager.Instance.RemoveRandomCollectedResponse();
			GameObject.Find("DaydreamPlayer").GetComponent<PlayerController2D>().Knockback();
            RunnerManager.Instance.InterruptScrolling();
            ReleaseSelf();
        }
    }

    internal virtual void ReleaseSelf()
    {
        ObjectPoolService.Instance.ReleaseInstance<ObstacleScript>(this);
    }
}

public enum ObstacleType
{
    ShortJump,
    LongJump,
    ShortDuck,
    LongDuck
}
