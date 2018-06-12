using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShape : MonoBehaviour {
	public GameObject[] rand;
	// Use this for initialization
	void Start () {
		Instantiate (rand [Random.Range (0, rand.Length - 1)], transform);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
