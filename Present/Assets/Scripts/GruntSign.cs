using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GruntSign : MonoBehaviour
{
    public GruntLight greenLight;
    public GruntLight redLight;
    public Text gruntText;

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
            if (Input.GetMouseButton(1))
            {
                Grunt(true);
            }
            else if (Input.GetMouseButton(2))
            {
                Grunt(false);
            }
            else if (DateTime.UtcNow > failTime)
            {
                EndGruntOpportunity();
                TriggerGruntFailure();
            }
        }
    }

    private void Grunt(bool greenGrunt)
    {
        EndGruntOpportunity();
        if (greenGrunt == greenLight.isOn)
            TriggerGruntSuccess();
        else
            TriggerGruntFailure();
    }

    public void TriggerGruntFailure()
    {
    }

    public void TriggerGruntSuccess()
    {
    }

    public void TriggerGruntOpportunity(bool greenGrunt, string promptText, int availabilityMillis)
    {
        failTime = DateTime.UtcNow.AddMilliseconds(availabilityMillis);
        gruntAvailable = true;

        if (greenGrunt)
            greenLight.isOn = true;
        else
            redLight.isOn = true;

        gruntText.text = promptText;
    }

    private void EndGruntOpportunity()
    {
        gruntAvailable = false;
        greenLight.isOn = false;
        redLight.isOn = false;
        gruntText.text = "";
    }
}
