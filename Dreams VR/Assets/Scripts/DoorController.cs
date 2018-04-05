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
		
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.transform.parent.tag == "MainCamera"){
		//if (other.tag == "Player") {
			doorAnimation.Play ("Door_open");
		} else if(other.tag == "EnvironmentObject"){
			Destroy (other.gameObject);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Player") {
			doorAnimation.Play ("Door_Close");
		}
	}

}
