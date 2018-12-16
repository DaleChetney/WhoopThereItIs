using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    public float sensitivity = 10f;
    public float maxXAngle = 80f;
    public float maxYAngle = 80f;
    public float driftSpeed = 0.5f;
    public float maxAttentionSpan = 1.0f;
    public float movementThreshold = 5.0f;
    public Transform eyeContact;
    public Transform[] distractions;
    private Vector2 currentRotation;
    private Vector2 lastMousePosition;
    private Vector3 distractionPosition;
    private bool nodding = false;
    private bool shaking = false;
    private Vector3 above;
    private Vector3 below;
    private Vector3 left;
    private Vector3 right;
    private bool eyeContactLost = false;


    // Start is called before the first frame update
    void Start()
    {
        currentRotation = Input.mousePosition;
        lastMousePosition = currentRotation;
        above = Camera.main.transform.position + new Vector3(0f, 100f, 0f);
        below = Camera.main.transform.position + new Vector3(0f, -100f, 0f);
        left = Camera.main.transform.position + new Vector3(-100f, 0f, 0f);
        right = Camera.main.transform.position + new Vector3(100f, 0f, 0f);
        StartCoroutine("Wander");

    }

    // Update is called once per frame
    void Update()
    {

        if (!nodding && !shaking)
        {
            if (Vector2.Distance(Input.mousePosition, lastMousePosition) > movementThreshold)
            {
                ResetGaze();
            }
        }

        DriftMove();

        lastMousePosition = Input.mousePosition;
    }

    private void ResetGaze()
    {
        eyeContactLost = false;
        distractionPosition = eyeContact.position;
    }

    private void DriftMove()
    {
        Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(distractionPosition - Camera.main.transform.position), driftSpeed * Time.deltaTime);
        if(!eyeContactLost && distractionPosition != eyeContact.position && DistractionReached())
        {
            eyeContactLost = true;
            StartCoroutine("SubtractPointsForEyeContact");
        }
    }

    private bool DistractionReached()
    {
        float angle = Quaternion.Angle(Camera.main.transform.rotation, Quaternion.LookRotation(distractionPosition - Camera.main.transform.position));
        return angle < 5f;
    }

    IEnumerator Wander()
    {
        for (; ; )
        {
            if(!nodding && !shaking)
            {
                Distract();
            }
            yield return new WaitForSeconds(Random.Range(0, maxAttentionSpan));
        }
    }

    IEnumerator Nod()
    {
        Vector3 oldTarget = distractionPosition;
        nodding = true;
        distractionPosition = above;
        yield return new WaitForSeconds(0.05f);
        distractionPosition = below;
        yield return new WaitForSeconds(0.10f);
        distractionPosition = above;
        yield return new WaitForSeconds(0.05f);
        distractionPosition = oldTarget;
        nodding = false;
    }

    IEnumerator Shake()
    {
        Vector3 oldTarget = distractionPosition;
        shaking = true;
        distractionPosition = left;
        yield return new WaitForSeconds(0.05f);
        distractionPosition = right;
        yield return new WaitForSeconds(0.10f);
        distractionPosition = left;
        yield return new WaitForSeconds(0.05f);
        distractionPosition = oldTarget;
        shaking = false;
    }


    private void Distract()
    {
        int index = Random.Range(0, distractions.Length);
        distractionPosition = distractions[index].position;
    }

    public void GruntEffects(GruntType gruntType)
    {
        if (!nodding && !shaking)
        {
            if (gruntType == GruntType.Green)
            {
                StartCoroutine("Nod");
            }
            else if (gruntType == GruntType.Red)
            {
                StartCoroutine("Shake");
            }
        }
    }

    IEnumerator SubtractPointsForEyeContact()
    {
        yield return new WaitForSeconds(1f);
        while (eyeContactLost)
        {
            GameManagerScript.Instance.ModifyScore(-1);
            yield return new WaitForSeconds(1f);
        }
    }
}
