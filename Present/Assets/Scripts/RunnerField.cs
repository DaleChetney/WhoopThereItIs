using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerField : MonoBehaviour
{

    [SerializeField]
    public GameObject groundPrefab;
    public int groundPieces;

    private List<Transform> groundSprites = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {

        float spriteSpacing = (RunnerManager.Instance.rightBoundary - RunnerManager.Instance.leftBoundary) / groundPieces;
        for(int i = 0; i< groundPieces; i++)
        {
            var groundSprite = ObjectPoolService.Instance.AcquireInstance<FieldGround>(groundPrefab);
            var spawnPosition = transform.position;
            spawnPosition.x = transform.position.x + RunnerManager.Instance.rightBoundary - i * spriteSpacing;
            spawnPosition.z = transform.position.z - i * 0.01f;
            groundSprite.transform.position = spawnPosition;
            groundSprites.Add(groundSprite.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var grnd in groundSprites)
        {
            grnd.position += new Vector3(-RunnerManager.Instance.scrollSpeed * Time.deltaTime, 0, -0.001f);
            if (grnd.position.x < transform.position.x + RunnerManager.Instance.leftBoundary)
            {
                var newVec = grnd.position;
                newVec.x = transform.position.x + RunnerManager.Instance.rightBoundary;
                newVec.z = transform.position.z;
                grnd.position = newVec;
            }
        }
    }
}
