using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManagerScript;

public class Look : MonoSingleton<Look>
{
    public float sensitivity = 10f;
    public float maxXAngle = 80f;
    public float maxYAngle = 80f;
    public float driftSpeed = 0.5f;
    public float maxAttentionSpan = 1.0f;
    public float movementThreshold = 5.0f;
    public Transform eyeContact;
    public Transform[] distractions;
    public LookAlert lookAlert;
    private Vector2 currentRotation;
    private Vector2 lastMousePosition;
    private Vector3 distractionPosition;
    private bool nodding = false;
    private bool shaking = false;
    //private Vector3 above;
    //private Vector3 below;
    //private Vector3 left;
    //private Vector3 right;
    private bool eyeContactLost = false;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        currentRotation = Input.mousePosition;
        lastMousePosition = currentRotation;
        //above = cam.transform.position + new Vector3(0f, 100f, 0f);
        //below = cam.transform.position + new Vector3(0f, -100f, 0f);
        //left = cam.transform.position + new Vector3(0f, 0f, -100f);
        //right = cam.transform.position + new Vector3(0f, 0f, 100f);
        StartCoroutine("Wander");
    }

    // Update is called once per frame
    void Update()
    {
		if(GameManagerScript.Instance.GameState == State.InGame)
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
    }

    private void ResetGaze()
    {
        eyeContactLost = false;
        AudioManager.Instance.mildAlarm.Stop();
        lookAlert.StopSignal();
        distractionPosition = eyeContact.position;
    }

    private void DriftMove()
    {
        Vector3 before = cam.transform.rotation.eulerAngles;
        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.LookRotation(distractionPosition - cam.transform.position), driftSpeed * Time.deltaTime);
        if (nodding)
        {
            Vector3 temp = cam.transform.rotation.eulerAngles;
            cam.transform.rotation = Quaternion.Euler(temp.x, before.y, before.z);
        }
        if(!eyeContactLost && distractionPosition != eyeContact.position && DistractionReached())
        {
            eyeContactLost = true;
            AudioManager.Instance.mildAlarm.Play();

            if (distractionPosition == distractions[1].position)
                lookAlert.SignalRight();
            else
                lookAlert.SignalUp();

            StartCoroutine("SubtractPointsForEyeContact");
        }
    }

    private bool DistractionReached()
    {
        float angle = Quaternion.Angle(cam.transform.rotation, Quaternion.LookRotation(distractionPosition - cam.transform.position));
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
        distractionPosition = -transform.up * 100;
        yield return new WaitForSeconds(0.05f);
        distractionPosition = transform.up * 100;
        yield return new WaitForSeconds(0.10f);
        distractionPosition = -transform.up * 100;
        yield return new WaitForSeconds(0.05f);
        distractionPosition = oldTarget;
        nodding = false;
	}

    IEnumerator Shake()
    {
		Vector3 oldTarget = distractionPosition;
        shaking = true;
        distractionPosition = -transform.right * 100;
		yield return new WaitForSeconds(0.05f);
        distractionPosition = transform.right * 100;
		yield return new WaitForSeconds(0.10f);
        distractionPosition = -transform.right * 100;
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
            GameManagerScript.Instance.ModifyScore(-1, false);
            yield return new WaitForSeconds(1f);
        }
    }
}
