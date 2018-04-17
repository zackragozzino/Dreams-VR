using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {
	public bool continuous;
	const float maxDegreesPerSecond = 10f;
	public Transform needleTransform;
	public Transform target;
	public Transform compassFace;
	public Transform compassBase;
	private Vector3 prevDir;
	// Use this for initialization
	void Start () {
		prevDir = new Vector3 (0, 0, 0);
	}

	void Update() {
		Vector3 v3T = target.position - needleTransform.position;
		Debug.Log ("v3T" + v3T);
		v3T.y = compassBase.up.y;
		Quaternion qTo = Quaternion.LookRotation(v3T, compassBase.up); 
		needleTransform.rotation = Quaternion.AngleAxis(0,compassBase.up) * Quaternion.RotateTowards(needleTransform.rotation, qTo, maxDegreesPerSecond * Time.deltaTime);

		/* OTHER TECHNIQUE
		 * 
		 * float angle = Mathf.Atan2(v3T.x, v3T.z)*Mathf.Rad2Deg;
		Debug.Log (angle);
		//if (!v3T.Equals (prevDir)) {
			needleTransform.RotateAround (compassBase.localPosition, compassBase.up, angle);
			prevDir = v3T;
		}
		*/
		//compassFace.rotation = Quaternion.RotateTowards(compassFace.rotation, qTo, -maxDegreesPerSecond * Time.deltaTime);
	
	}

}
