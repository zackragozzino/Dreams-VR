using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour {

	private bool alreadySpawned;
	public GameObject snakeGroup;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void spawnTransition(){
		if (!alreadySpawned) {
			snakeGroup.SetActive (true);
			alreadySpawned = true;
		}
	}
}
