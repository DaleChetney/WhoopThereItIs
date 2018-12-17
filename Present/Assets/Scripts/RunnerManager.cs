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
    public int obstacleSpawnRarity;
    public int maxConsecutiveObstacles;
    public int maxConsecutivePackets;

    private DateTime nextSpawnTime;
    private float defaultScrollSpeed;
    private int obstaclesSincePacket = 0;
    private int packetsSinceObstacle = 0;

    private Dictionary<RunnerObjectType, float> spawnPositions = new Dictionary<RunnerObjectType, float>()
    {
        {RunnerObjectType.ShortJump, -0.514f },
        {RunnerObjectType.LongJump, -0.667f },
        {RunnerObjectType.ShortDuck, -0.179f },
        {RunnerObjectType.JumpPacket, -0.159f },
        {RunnerObjectType.WalkPacket, -0.52f },
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
        if (DateTime.UtcNow > nextSpawnTime)
        {
            SetNextSpawnTime();
            
            if (UnityEngine.Random.Range(0, obstacleSpawnRarity) == 0 && obstaclesSincePacket <= maxConsecutiveObstacles)
                SpawnObstacle();
            else if (packetsSinceObstacle <= maxConsecutivePackets)
                SpawnPacket();
            else
                SpawnObstacle();
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

    public void SpawnObstacle()
    {
        RunnerObjectType objectType = (RunnerObjectType)UnityEngine.Random.Range(0, obstaclePrefabs.Length);
        packetsSinceObstacle = 0;
        obstaclesSincePacket++;

        SpawnObject(objectType);
    }

    public void SpawnPacket()
    {
        RunnerObjectType objectType = (RunnerObjectType)UnityEngine.Random.Range(obstaclePrefabs.Length, obstaclePrefabs.Length + 2);
        obstaclesSincePacket = 0;
        packetsSinceObstacle++;

        SpawnObject(objectType);
    }

    private void SpawnObject(RunnerObjectType objectType)
    {
        RunnerObject item;
        switch (objectType)
        {
            case RunnerObjectType.ShortJump:
                item = ObjectPoolService.Instance.AcquireInstance<ShortJumpObstacle>(obstaclePrefabs[(int)objectType]);
                break;
            case RunnerObjectType.LongJump:
                item = ObjectPoolService.Instance.AcquireInstance<LongJumpObstacle>(obstaclePrefabs[(int)objectType]);
                break;
            case RunnerObjectType.ShortDuck:
                item = ObjectPoolService.Instance.AcquireInstance<ShortDuckObstacle>(obstaclePrefabs[(int)objectType]);
                break;
            case RunnerObjectType.JumpPacket:
            case RunnerObjectType.WalkPacket:
            default:
                item = ResponseManager.Instance.GetRandomAvailableResponse();
                break;
        }

        if(item != null)
        {
            var spawnPosition = transform.position;
            spawnPosition.x += RunnerManager.Instance.rightBoundary;
            spawnPosition.y = spawnPositions[objectType];
            spawnPosition.z = 1;
            item.transform.position = spawnPosition;
        }
    }

    private void SetNextSpawnTime()
    {
        float spawnVariance = UnityEngine.Random.Range(-spawnTimeVariance * obstacleSpawnIntervalSec, spawnTimeVariance * obstacleSpawnIntervalSec);
        nextSpawnTime = DateTime.UtcNow.AddSeconds(obstacleSpawnIntervalSec + spawnVariance);
    }

    public void InterruptScrolling()
    {
        scrollSpeed = defaultScrollSpeed * -knockbackForce;
    }
}

public enum RunnerObjectType
{
    ShortJump,
    LongJump,
    ShortDuck,
    JumpPacket,
    WalkPacket,
}