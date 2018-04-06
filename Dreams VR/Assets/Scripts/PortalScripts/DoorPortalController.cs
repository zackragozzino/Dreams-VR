using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPortalController : MonoBehaviour {

	private Director director;

	// Use this for initialization
	void Start () {
		director = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Director>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		Debug.Log (other.gameObject.name);
		if (other.tag == "Player") {
			Debug.Log ("Entering....");
			director.GenerateNewWorld ();
		}
	}
}
