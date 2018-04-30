using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {

	public Animator doorAnimation;
	private GameObject compass;
	private Vector3 compassStartPos;
	private bool doorOpened;

	// Use this for initialization
	void Start () {
		//StartCoroutine (waitAndOpenDoor ());
		compass = GameObject.Find ("Compass 1");
		compassStartPos = compass.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if(compass.transform.position != compassStartPos && !doorOpened){
			doorAnimation.Play ("Door_open");
			doorOpened = true;
		}
		
	}

	IEnumerator waitAndOpenDoor(){
		yield return new WaitForSeconds (10f);
		doorAnimation.Play ("Door_open");
	}
}
