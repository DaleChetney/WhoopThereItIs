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
			GameObject.Find("DaydreamPlayer").GetComponent<PlayerController2D>().ResetAfterDeath();
			gameObject.SetActive(false);
		}
	}
}
