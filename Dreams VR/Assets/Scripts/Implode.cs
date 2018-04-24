using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Implode : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 pos = transform.position;
		transform.position = new Vector3 (pos.x, Random.Range (20, 100), pos.z);
		transform.eulerAngles = new Vector3 (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));

		gameObject.AddComponent<Rigidbody> ().useGravity = false;

		StartCoroutine (waitAndFall());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator waitAndFall(){
		yield return new WaitForSeconds (Random.Range(5f, 15f));
		gameObject.GetComponent<Rigidbody> ().useGravity = true;
	}
}
