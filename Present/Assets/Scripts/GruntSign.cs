using UnityEngine;
using UnityEngine.UI;

public class GruntSign : MonoSingleton<GruntSign>
{
    public GruntLight greenLight;
    public GruntLight redLight;
    public Text gruntText;
	public bool wasLastGruntSuccessful;

	private bool gruntAvailable;
    private float failTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (gruntAvailable)
		{
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
            else if (Time.time > failTime)
            {
                EndGruntOpportunity();
                TriggerGruntFailure();
            }
        }
    }

    private void Grunt(GruntType gruntType)
    {
        bool success = (gruntType == GruntType.Green) == greenLight.isOn;
        EndGruntOpportunity();
        if (success)
            TriggerGruntSuccess(gruntType);
        else
            TriggerGruntFailure();
    }

    public void TriggerGruntFailure()
    {
		wasLastGruntSuccessful = false;
        GameManagerScript.Instance.ShortSubmitTime();
    }

    public void TriggerGruntSuccess(GruntType gruntType)
    {
		wasLastGruntSuccessful = true;
        GameManagerScript.Instance.ShortSubmitTime();
    }

    public void TriggerGruntOpportunity(GruntType gruntType, string promptText, float availabilitySecs)
    {
        failTime = Time.time + availabilitySecs;
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