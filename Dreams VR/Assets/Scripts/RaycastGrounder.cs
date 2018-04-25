using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastGrounder : MonoBehaviour {

	bool grounded;

	// Use this for initialization
	void Start () {

	}

	void Update(){
		if (!grounded) {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
			{
				//Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
				this.transform.position = hit.point;
				this.transform.eulerAngles = hit.normal;
				grounded = true;
			}
		}
	}

}
