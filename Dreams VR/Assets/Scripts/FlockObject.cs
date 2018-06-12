using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockObject : MonoBehaviour {

	public float speed = 0.5f;
	float rotationSpeed = 4.0f;
	Vector3 averageHeading;
	Vector3 averagePosition;
	float neighborDistance = 2.0f;

	bool turning = false;

	FlockingController flockingController;

	// Use this for initialization
	void Start () {
		speed = Random.Range (0.5f, 1f);
		flockingController = this.GetComponentInParent<FlockingController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (transform.position, flockingController.transform.position) >= flockingController.swarmSize)
			turning = true;
		else
			turning = false;

		if (turning) {
			Vector3 direction = flockingController.transform.position - transform.position;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), rotationSpeed * Time.deltaTime);
			speed = Random.Range (0.5f, 1f);
		} else {
			if (Random.Range (0, 5) < 1)
				ApplyRules ();
		}

		transform.Translate (0, 0, Time.deltaTime * speed);
	}

	void ApplyRules(){
		GameObject[] flockObjects;
		flockObjects = flockingController.getSwarmObjects();

		Vector3 vCenter = flockingController.transform.position;
		Vector3 vAvoid = Vector3.zero;
		float gSpeed = 0.1f;

		Vector3 goalPos = flockingController.goalPos;
		float dist;
		int groupSize = 0;

		foreach (GameObject go in flockObjects) {
			if (go != this.gameObject) {
				dist = Vector3.Distance (go.transform.position, this.transform.position);
				//If you're in a swarm
				if (dist <= neighborDistance) {
					vCenter += go.transform.position;
					groupSize++;

					//If you're about to hit a neighbor
					if (dist < 1.0f) {
						vAvoid += (this.transform.position - go.transform.position);
					}
					FlockObject anotherFlock = go.GetComponent<FlockObject> ();
					gSpeed += anotherFlock.speed;
				}
			}
		}

		if (groupSize > 0) {
			vCenter = vCenter / groupSize + (goalPos - this.transform.position);
			speed = gSpeed / groupSize;

			Vector3 direction = (vCenter + vAvoid) - transform.position;
			if (direction != Vector3.zero)
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), rotationSpeed * Time.deltaTime);
		}
	}
}
