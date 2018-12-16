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
        {ObstacleType.ShortJump, -0.514f },
        {ObstacleType.LongJump, -0.667f },
        {ObstacleType.ShortDuck, -0.179f },
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
            nextSpawnTime = GetNextSpawnTime();
            int test= UnityEngine.Random.Range(0, obstaclePrefabs.Length);
            Debug.Log(test);
            ObstacleType obstacleType = (ObstacleType)UnityEngine.Random.Range(0, obstaclePrefabs.Length);
            Debug.Log(obstacleType);
            SpawnObstacle(obstacleType);
        }

        if (scrollSpeed < defaultScrollSpeed)
        {
            scrollSpeed += defaultScrollSpeed * speedRecovery * Time.deltaTime;
            nextSpawnTime.AddSeconds(Time.deltaTime);
        }
        else
        {
            scrollSpeed = defaultScrollSpeed;
        }
    }

    public void SpawnObstacle(ObstacleType obstacleType)
    {
        ObstacleScript obstacle;
        switch (obstacleType)
        {
            case ObstacleType.ShortJump:
                obstacle = ObjectPoolService.Instance.AcquireInstance<ShortJumpObstacle>(obstaclePrefabs[(int)obstacleType]);
                break;
            case ObstacleType.LongJump:
                obstacle = ObjectPoolService.Instance.AcquireInstance<LongJumpObstacle>(obstaclePrefabs[(int)obstacleType]);
                break;
            case ObstacleType.ShortDuck:
                obstacle = ObjectPoolService.Instance.AcquireInstance<ShortDuckObstacle>(obstaclePrefabs[(int)obstacleType]);
                break;
            default:
                obstacle = ObjectPoolService.Instance.AcquireInstance<ShortJumpObstacle>(obstaclePrefabs[(int)obstacleType]);
                break;
        }
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
