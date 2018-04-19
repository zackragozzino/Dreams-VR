using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {
	public float maxDegreesPerSecond = 10f;
	public Transform needleTransform;
	public Transform target;
	public Transform compassFace;
	public Transform compassBase;

	void Update() {
		Vector3 v3T = target.position - needleTransform.position;
		//Debug.Log ("v3T" + v3T);
		v3T.y = compassBase.up.y;
		Quaternion qTo = Quaternion.LookRotation(v3T, compassBase.up); 
		needleTransform.rotation = Quaternion.AngleAxis(0,compassBase.up) * Quaternion.RotateTowards(needleTransform.rotation, qTo, maxDegreesPerSecond * Time.deltaTime);
		Vector3 eulers = needleTransform.localEulerAngles;
		needleTransform.localEulerAngles = new Vector3 (0, eulers.y, 0);

		compassFace.rotation = Quaternion.AngleAxis(0,compassBase.up) * Quaternion.RotateTowards(compassFace.rotation, qTo, -maxDegreesPerSecond * Time.deltaTime);
		Vector3 eulers2 = compassFace.localEulerAngles;
		compassFace.localEulerAngles = new Vector3 (0, eulers2.y, 0);
	
	}

}
