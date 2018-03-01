using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItFollows : MonoBehaviour {
	public GameObject follower;
	private GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		GameObject followerClone = (GameObject)Instantiate(follower, player.transform.position + new Vector3(0,0,-60), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
