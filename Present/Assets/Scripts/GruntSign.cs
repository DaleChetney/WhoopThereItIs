using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GruntSign : MonoSingleton<GruntSign>
{
    public GruntLight greenLight;
    public GruntLight redLight;
    public Text gruntText;
	public bool wasLastGruntSuccessful;

	private bool gruntAvailable;
    private DateTime failTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (gruntAvailable)
		{
			Debug.Log("GET YOUR GRUNT ON");
			if (Input.GetMouseButton(0))
            {
                Grunt(GruntType.Green);
				Look.Instance.GruntEffects(GruntType.Green);
			}
            else if (Input.GetMouseButton(1))
            {
				Grunt(GruntType.Red);
				Look.Instance.GruntEffects(GruntType.Red);
			}
            else if (DateTime.UtcNow > failTime)
            {
                EndGruntOpportunity();
                TriggerGruntFailure();
            }
        }
    }

    private void Grunt(GruntType gruntType)
    {
        EndGruntOpportunity();
        if ((gruntType == GruntType.Green) == greenLight.isOn)
            TriggerGruntSuccess(gruntType);
        else
            TriggerGruntFailure();
    }

    public void TriggerGruntFailure()
    {
		wasLastGruntSuccessful = false;
        AudioManager.Instance.gruntFail.Play();
    }

    public void TriggerGruntSuccess(GruntType gruntType)
    {
		wasLastGruntSuccessful = true;
        AudioManager.Instance.gruntPass.Play();
	}

    public void TriggerGruntOpportunity(GruntType gruntType, string promptText, int availabilityMillis)
    {
        failTime = DateTime.UtcNow.AddMilliseconds(availabilityMillis);
        gruntAvailable = true;
		wasLastGruntSuccessful = false;

        if (gruntType == GruntType.Green)
            greenLight.isOn = true;
        else
            redLight.isOn = true;

        gruntText.text = promptText;
        AudioManager.Instance.gruntAlert.Play();
    }

	private void EndGruntOpportunity()
	{
		gruntAvailable = false;
		greenLight.isOn = false;
		redLight.isOn = false;
		gruntText.text = "";
	}
}

public enum GruntType
{
    Green,
    Red
}