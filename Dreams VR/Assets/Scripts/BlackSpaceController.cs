using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSpaceController : MonoBehaviour {

	public GameObject[] SweetSpots;

	private Director director;
	private GameObject player;

	// Use this for initialization
	void Start () {
		//director = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Director> ();
		//player = director.getPlayer ();

		generateSweetSpot ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void generateSweetSpot(){
		GameObject sweetSpot = SweetSpots [Random.Range (0, SweetSpots.Length)];
		Instantiate (sweetSpot, new Vector3(sweetSpot.transform.position.x - 15, sweetSpot.transform.position.y, sweetSpot.transform.position.z), sweetSpot.transform.rotation, transform);
	}
}
