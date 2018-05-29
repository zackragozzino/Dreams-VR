using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteStairTrigger : MonoBehaviour {

	private bool entered = false;
	private StairSpawner stairSpawner;

	// Use this for initialization
	void Start () {
		stairSpawner = this.GetComponentInParent<StairSpawner> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player" && !entered) {
			stairSpawner.newFloor ();
			stairSpawner.newFloor ();
			stairSpawner.newFloor ();
			entered = true;
		}
	}
}
