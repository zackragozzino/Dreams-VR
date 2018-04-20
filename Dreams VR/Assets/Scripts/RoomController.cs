using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {

	public Animator doorAnimation;

	// Use this for initialization
	void Start () {
		StartCoroutine (waitAndOpenDoor ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator waitAndOpenDoor(){
		yield return new WaitForSeconds (10f);
		doorAnimation.Play ("Door_open");
	}
}
