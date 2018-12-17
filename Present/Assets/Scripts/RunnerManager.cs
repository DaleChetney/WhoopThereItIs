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

    private DateTime nextObstacleSpawnTime;
    private DateTime nextPacketSpawnTime;
    private float defaultScrollSpeed;

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
        if (DateTime.UtcNow > nextObstacleSpawnTime)
        {
            SetNextSpawnTime();

            RunnerObjectType objectType;
            if (UnityEngine.Random.Range(0, obstacleSpawnRarity) > 0)
            {
                objectType = (RunnerObjectType)UnityEngine.Random.Range(obstaclePrefabs.Length, obstaclePrefabs.Length + 2);
                Debug.Log("A packet has spawned");
            }
            else
            {
                objectType = (RunnerObjectType)UnityEngine.Random.Range(0, obstaclePrefabs.Length);
                Debug.Log("An obstacle has spawned");
            }

            SpawnObject(objectType);
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
        nextObstacleSpawnTime = DateTime.UtcNow.AddSeconds(obstacleSpawnIntervalSec + spawnVariance);
        nextPacketSpawnTime = DateTime.UtcNow.AddSeconds((obstacleSpawnIntervalSec + spawnVariance) * 0.5f);
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