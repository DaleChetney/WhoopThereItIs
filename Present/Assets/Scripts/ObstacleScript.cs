using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player")
		{
            ResponseManager.Instance.RemoveRandomCollectedResponse();
			// TODO: Move player backwards 
			gameObject.SetActive(false);
		}
	}
}
