using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingController : MonoBehaviour {

	public GameObject[] randomFlockObjects;
	private GameObject flockObject;

	static int numObjects = 25;
	public int swarmSize = 20;
	private GameObject[] allObjects = new GameObject[numObjects];

	public Vector3 goalPos = Vector3.zero;
	private Vector3 parentGoal = Vector3.zero;

	private float parentDist = 25f;

	// Use this for initialization
	void Start () {
		flockObject = randomFlockObjects [Random.Range(0, randomFlockObjects.Length - 1)];

		/*for (int i = 0; i < numObjects; i++) {
			Vector3 pos = new Vector3 (Random.Range (-swarmSize, swarmSize), Random.Range (-swarmSize, swarmSize), Random.Range (-swarmSize, swarmSize));
			allObjects [i] = (GameObject)Instantiate (flockObject, pos + this.transform.position, Quaternion.identity, this.transform);
			allObjects [i].AddComponent<FlockObject> ();
		}*/

		StartCoroutine (generateFlock ());

		StartCoroutine (changeParentGoal ());
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.Range (0, 100000) < 50) {
			goalPos = this.transform.position + new Vector3 (Random.Range (-swarmSize, swarmSize), Random.Range (-swarmSize, swarmSize), Random.Range (-swarmSize, swarmSize));
		}
			
		//transform.Translate (0, 0, Time.deltaTime);
		transform.position = Vector3.MoveTowards(transform.position, parentGoal, Time.deltaTime * 2f);

		Vector3 direction = parentGoal - transform.position;
		if (direction != Vector3.zero)
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), 0.5f * Time.deltaTime);

	}

	IEnumerator generateFlock(){
		for (int i = 0; i < numObjects; i++) {
			Vector3 pos = new Vector3 (Random.Range (-swarmSize, swarmSize), Random.Range (-swarmSize, swarmSize), Random.Range (-swarmSize, swarmSize));
			allObjects [i] = (GameObject)Instantiate (flockObject, pos + this.transform.position, Quaternion.identity, this.transform);
			yield return new WaitForSeconds (0.2f);
		}

		for (int i = 0; i < numObjects; i++) {
			allObjects [i].AddComponent<FlockObject> ();
		}
	}

	IEnumerator changeParentGoal(){
		parentGoal = this.transform.position + new Vector3 (Random.Range (-parentDist, parentDist), 0, Random.Range (-parentDist, parentDist));

		yield return new WaitForSeconds (Random.Range (2f, 6f));
		StartCoroutine (changeParentGoal ());
	}

	public GameObject[] getSwarmObjects(){
		return allObjects;
	}
}
