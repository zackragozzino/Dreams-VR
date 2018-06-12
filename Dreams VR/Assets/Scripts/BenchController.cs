using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchController : MonoBehaviour {

	private Vector3 position;
	float mod = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		mod += Time.deltaTime;
		position = transform.position;
		position.y += (Mathf.Sin (mod) / 500);

		transform.position = position;
	}
}
