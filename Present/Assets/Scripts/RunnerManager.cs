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

    private DateTime nextObstacleSpawnTime;
    private DateTime nextPacketSpawnTime;
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
        SetNextSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (DateTime.UtcNow > nextObstacleSpawnTime)
        {
            SetNextSpawnTime();
            ObstacleType obstacleType = (ObstacleType)UnityEngine.Random.Range(0, obstaclePrefabs.Length);
            SpawnObstacle(obstacleType);
        }

        if (DateTime.UtcNow > nextPacketSpawnTime)
        {
            nextPacketSpawnTime = DateTime.MaxValue;
            ObstacleType obstacleType = (ObstacleType)UnityEngine.Random.Range(0, obstaclePrefabs.Length);
            SpawnPacket(obstacleType);
        }

        if (scrollSpeed < defaultScrollSpeed)
        {
            scrollSpeed += defaultScrollSpeed * speedRecovery * Time.deltaTime;
            nextObstacleSpawnTime.AddSeconds(Time.deltaTime);
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

    public void SpawnPacket(ObstacleType obstacleType)
    {
        ResponsePacket packet = ResponseManager.Instance.GetRandomAvailableResponse();
        var spawnPosition = transform.position;
        spawnPosition.x += RunnerManager.Instance.rightBoundary;
        spawnPosition.y = spawnPositions[obstacleType];
        spawnPosition.z = 1;
        packet.transform.position = spawnPosition;
    }

    private void SetNextSpawnTime()
    {
        float spawnVariance = UnityEngine.Random.Range(-spawnTimeVariance * obstacleSpawnIntervalSec, spawnTimeVariance * obstacleSpawnIntervalSec);
        nextObstacleSpawnTime = DateTime.UtcNow.AddSeconds(obstacleSpawnIntervalSec + spawnVariance);
        nextPacketSpawnTime = DateTime.UtcNow.AddSeconds((obstacleSpawnIntervalSec + spawnVariance) * 0.5f);
    }

    public void InterruptScrolling()
    {
        scrollSpeed = defaultScrollSpeed * -knockbackForce;
    }
}
