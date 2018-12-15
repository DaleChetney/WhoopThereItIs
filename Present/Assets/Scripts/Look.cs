using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    public float sensitivity = 10f;
    public float maxXAngle = 80f;
    public float maxYAngle = 80f;
    public float driftSpeed = 1.0f;
    public float maxAttentionSpan = 1.0f;
    public float movementThreshold = 5.0f;
    public Transform eyeContact;
    public Transform[] distractions;
    private Vector2 currentRotation;
    private Vector2 lastMousePosition;
    private Vector3 distractionPosition;


    // Start is called before the first frame update
    void Start()
    {
        currentRotation = Input.mousePosition;
        lastMousePosition = currentRotation;
        StartCoroutine("Wander");

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(Input.mousePosition, lastMousePosition) > movementThreshold)
        {
            //UserInputMove();
            ResetGaze();
        }
      
        DriftMove();
        lastMousePosition = Input.mousePosition;
    }

    private void ResetGaze()
    {
        distractionPosition = eyeContact.position;
    }

    private void UserInputMove()
    {
        
        currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
        currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        //currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxXAngle, maxXAngle);
        currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
        Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(distractionPosition - Camera.main.transform.position), driftSpeed * Time.deltaTime);


    }

    private void DriftMove()
    {
        Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(distractionPosition - Camera.main.transform.position), driftSpeed * Time.deltaTime);
    }

    IEnumerator Wander()
    {
        for (; ; )
        {
            MoveGaze();
            yield return new WaitForSeconds(Random.Range(0, maxAttentionSpan));
        }
    }

    private void MoveGaze()
    {
        int index = Random.Range(0, distractions.Length);
        distractionPosition = distractions[index].position;
    }
}
