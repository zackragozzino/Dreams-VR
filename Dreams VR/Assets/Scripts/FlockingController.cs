using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingController : MonoBehaviour {

	public GameObject flockObject;
	public GameObject goalPosObject;
	static int numObjects = 25;
	public int swarmSize = 5;
	private GameObject[] allObjects = new GameObject[numObjects];

	public Vector3 goalPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < numObjects; i++) {
			Vector3 pos = new Vector3 (Random.Range (-swarmSize, swarmSize), Random.Range (-swarmSize, swarmSize), Random.Range (-swarmSize, swarmSize));
			allObjects [i] = (GameObject)Instantiate (flockObject, pos + this.transform.position, Quaternion.identity, this.transform);
			allObjects [i].AddComponent<FlockObject> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.Range (0, 10000) < 50) {
			goalPos = this.transform.position + new Vector3 (Random.Range (-swarmSize, swarmSize), Random.Range (-swarmSize, swarmSize), Random.Range (-swarmSize, swarmSize));
			goalPosObject.transform.position = goalPos;
		}

		//transform.Translate (0, 0, Time.deltaTime * 2);
	}

	public GameObject[] getSwarmObjects(){
		return allObjects;
	}
}
