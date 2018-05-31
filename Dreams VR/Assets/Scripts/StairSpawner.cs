using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairSpawner : MonoBehaviour {

	public GameObject stairFloorPrefab;
	public GameObject stairMiddlePrefab;
	public GameObject stairTopPrefab;
	public int maxFloors;
	
	private static float offsetAmount = 3.11f;
	private static float transitionHeight = 5f;
	private float verticalOffset;
	private int yRotation;
	private bool flipDirection;
	private int floorNum = 1;
	private Director director;
	private bool playerAtTop;
	private GameObject player;


	// Use this for initialization
	void Start () {
		Instantiate (stairFloorPrefab, transform.position, transform.rotation, transform);
		newFloor ();
		//newFloor ();
		//newFloor ();
		director = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Director>();
		player = director.getPlayer ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.y >= verticalOffset && floorNum >= maxFloors)
			playerAtTop = true;

		if (playerAtTop)
			checkFinalTransition ();
	}

	public void checkFinalTransition(){
		if (player.transform.position.y < transitionHeight)
			director.GenerateNewWorld ();
	}

	public void newFloor(){
		if (floorNum < maxFloors) {
			verticalOffset += offsetAmount;

			if (flipDirection)
				yRotation = 180;
			else
				yRotation = 0;

			Instantiate (stairMiddlePrefab, new Vector3 (transform.position.x, verticalOffset, transform.position.y), Quaternion.Euler (transform.eulerAngles.x, yRotation, transform.eulerAngles.z), transform);

			flipDirection = !flipDirection;
			floorNum++;
		} else if (floorNum == maxFloors) {
			verticalOffset += offsetAmount;

			if (flipDirection)
				yRotation = 180;
			else
				yRotation = 0;

			Instantiate (stairTopPrefab, new Vector3 (transform.position.x, verticalOffset, transform.position.y), Quaternion.Euler (transform.eulerAngles.x, yRotation, transform.eulerAngles.z), transform);

			floorNum++;
		}
	}
}
