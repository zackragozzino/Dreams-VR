using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackEntranceToggle : MonoBehaviour {

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
			pt.canTeleport = true;
		}
	}
}
