using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

	Animator doorAnimation;

	// Use this for initialization
	void Start () {
		doorAnimation = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.R))
			doorAnimation.Play ("Door_open");

		if (Input.GetKeyDown (KeyCode.T))
			doorAnimation.Play ("Door_Close");
		
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			doorAnimation.Play ("Door_open");
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Player") {
			doorAnimation.Play ("Door_Close");
		}
	}

}
