using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

	Animator doorAnimation;
	private AudioManager audm;

	// Use this for initialization
	void Start () {
		doorAnimation = this.GetComponent<Animator> ();
		audm = FindObjectOfType<AudioManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.transform.parent.tag == "MainCamera"){
		//if (other.tag == "Player") {
			doorAnimation.Play ("Door_open");
			audm.Play ("DoorOpen");
		} else if(other.tag == "EnvironmentObject"){
			//Destroy (other.gameObject);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Player") {
			audm.Play ("DoorClose");
			doorAnimation.Play ("Door_Close");
		}
	}

}
