using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTeleport : MonoBehaviour {

	public PortalTeleporter pt;

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			pt.canTeleport = false;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player")
		{
			//pt.canTeleport = true;
			StartCoroutine("entranceDelay");
		}
	}

	private IEnumerator entranceDelay(){
		yield return new WaitForSeconds (0.2f);
		pt.canTeleport = true;
	}
}
