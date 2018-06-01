using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseTrigger : MonoBehaviour {
	HouseController houseController;
	// Use this for initialization
	void Start () {
		houseController = this.GetComponentInParent<HouseController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			houseController.spawnTransition ();
		}
	}
}
