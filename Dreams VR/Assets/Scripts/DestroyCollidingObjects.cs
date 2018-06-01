using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollidingObjects : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "EnvironmentObject"){
			//Debug.Log ("Removed object: " + other.name);
			Destroy (other.gameObject);
		}
	}
}
