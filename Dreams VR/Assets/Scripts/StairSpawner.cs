using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairSpawner : MonoBehaviour {

	public GameObject stairFloorPrefab;
	public GameObject stairMiddlePrefab;
	public int maxFloors;
	
	private static float offsetAmount = 3.11f;
	private float verticalOffset;
	private int yRotation;
	private bool flipDirection;
	private int floorNum = 1;

	// Use this for initialization
	void Start () {
		Instantiate (stairFloorPrefab, transform.position, transform.rotation, transform);
	}
	
	// Update is called once per frame
	void Update () {
		if (floorNum <= maxFloors) {
			verticalOffset += offsetAmount;

			if (!flipDirection)
				yRotation = 180;
			else
				yRotation = 0;

			Instantiate (stairMiddlePrefab, new Vector3 (transform.position.x, verticalOffset, transform.position.y), Quaternion.Euler(transform.eulerAngles.x, yRotation, transform.eulerAngles.z), transform);
		
			flipDirection = !flipDirection;
			floorNum++;
		}
	}
}
