using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntLightScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ToggleLight()
	{
		gameObject.GetComponentInChildren<Light>().enabled = !gameObject.GetComponentInChildren<Light>().enabled;
	}


	public void EnableLight()
	{
		gameObject.GetComponentInChildren<Light>().enabled = true;
	}

	public void DisableLight()
	{
		gameObject.GetComponentInChildren<Light>().enabled = false;
	}
}
