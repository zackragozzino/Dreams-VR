using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItFollows : MonoBehaviour {
	public GameObject follower;
	private GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		Vector3 playerPos = player.transform.position;
		GameObject followerClone = (GameObject)Instantiate(follower, new Vector3(playerPos.x, 0, playerPos.z + 100), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
