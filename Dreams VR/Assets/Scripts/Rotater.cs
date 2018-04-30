using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour {

	public Vector3 RotationPerSecond;
	public bool enablePivot;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (RotationPerSecond * Time.deltaTime);

		if (enablePivot)
			StartCoroutine (pivotTimer ());
	}

	IEnumerator pivotTimer(){
		enablePivot = false;
		yield return new WaitForSeconds (10f);
		RotationPerSecond = RotationPerSecond * -1;
		enablePivot = true;
	}
}
