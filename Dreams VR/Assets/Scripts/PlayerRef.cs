using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerRef : MonoBehaviour {

	public GameObject VR_Player;
	public GameObject Simulator_Player;

	public bool enableVR = false;

	// Use this for initialization
	void Start () {

		if (!enableVR)
			XRSettings.enabled = false;
	}

	void Update(){
	}

	public GameObject getPlayerReference(){

		if (XRSettings.enabled)
			return VR_Player;
		else
			return Simulator_Player;
	}
}
