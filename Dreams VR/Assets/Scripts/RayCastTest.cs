using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTest : MonoBehaviour {

	private bool lookAt = false;
	private Vector3 lastPosition;
	private Transform hitObject;
	private float speed = 5f;
	public float targetDistance;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = new Ray (transform.position, transform.forward);
		Debug.DrawRay (ray.origin, ray.direction, Color.cyan);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.gameObject.tag == "test") {
				hitObject = hit.transform;
				lookAt = true;
				Debug.Log ("hit");
			} else {
				lookAt = false;
			}
				
		}
		if (!lookAt && hitObject != null) {
			float step = speed * Time.deltaTime;
			hitObject.transform.position = Vector3.MoveTowards (hitObject.transform.position, transform.position, step);
		}
	}
	/*void Update () {
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit)) {
			targetDistance = hit.distance;
		}
	}*/
}
