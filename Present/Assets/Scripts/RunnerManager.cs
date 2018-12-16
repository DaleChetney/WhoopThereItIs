using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerManager : MonoSingleton<RunnerManager>
{
    [SerializeField]
    public GameObject[] obstaclePrefabs;

    public float leftBoundary;
    public float rightBoundary;
    public float scrollSpeed;
    public float obstacleSpawnIntervalSec;
    public float spawnTimeVariance;
    public float knockbackForce;
    public float speedRecovery;

    private DateTime nextSpawnTime;
    private float defaultScrollSpeed;

    private Dictionary<ObstacleType, float> spawnPositions = new Dictionary<ObstacleType, float>()
    {
        {ObstacleType.ShortJump, -0.35f },
        {ObstacleType.LongJump, 1f },
        {ObstacleType.ShortDuck, 1f },
        {ObstacleType.LongDuck, 1f }
    };

    // Start is called before the first frame update
    void Start()
    {
        defaultScrollSpeed = scrollSpeed;
        nextSpawnTime = GetNextSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (DateTime.UtcNow > nextSpawnTime)
        {
            SpawnObstacle(ObstacleType.ShortJump);
            nextSpawnTime = GetNextSpawnTime();
        }

        if(scrollSpeed < defaultScrollSpeed)
        {
            scrollSpeed += defaultScrollSpeed * speedRecovery * Time.deltaTime;
        }
    }

    public void SpawnObstacle(ObstacleType obstacleType)
    {
        var obstacle = ObjectPoolService.Instance.AcquireInstance<ObstacleScript>(obstaclePrefabs[(int)obstacleType]);
        var spawnPosition = transform.position;
        spawnPosition.x += RunnerManager.Instance.rightBoundary;
        spawnPosition.y = spawnPositions[obstacleType];
        spawnPosition.z = 1;
        obstacle.transform.position = spawnPosition;
    }

    private DateTime GetNextSpawnTime()
    {
        var spawnVariance = UnityEngine.Random.Range(-spawnTimeVariance * obstacleSpawnIntervalSec, spawnTimeVariance * obstacleSpawnIntervalSec);
        return DateTime.UtcNow.AddSeconds(obstacleSpawnIntervalSec + spawnVariance);
    }

    public void InterruptScrolling()
    {
        scrollSpeed = defaultScrollSpeed * -knockbackForce;
    }
}
