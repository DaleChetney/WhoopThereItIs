using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerField : MonoBehaviour
{
    [SerializeField]
    public Transform[] groundSprites;
    public float leftBoundary;
    public float rightBoundary;
    public float scrollSpeed;

    // Start is called before the first frame update
    void Start()
    {
        float spriteSpacing = (rightBoundary - leftBoundary) / groundSprites.Length;
        for(int i = 0; i<groundSprites.Length; i++)
        {
            var spawnPosition = groundSprites[i].position;
            spawnPosition.x = transform.position.x + rightBoundary - i * spriteSpacing;
            spawnPosition.z = transform.position.z - i * 0.01f;
            groundSprites[i].position = spawnPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var grnd in groundSprites)
        {
            grnd.position += new Vector3(-scrollSpeed * Time.deltaTime,0,-0.001f);
            if(grnd.position.x < transform.position.x + leftBoundary)
            {
                var newVec = grnd.position;
                newVec.x = transform.position.x + rightBoundary;
                newVec.z = transform.position.z;
                grnd.position = newVec;
            }
        }
    }
}
