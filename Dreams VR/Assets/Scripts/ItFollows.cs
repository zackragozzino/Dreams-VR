using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItFollows : MonoBehaviour {
	public GameObject follower;
	private GameObject player;
	public Vector3 offset;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		Vector3 playerPos = player.transform.position;
		//GameObject followerClone = (GameObject)Instantiate(follower, new Vector3(playerPos.x + offset.x, 0 + offset.y, playerPos.z + offset.z), Quaternion.identity);
		for (int i = 0; i < 10; i++) {
			GameObject followerClone = (GameObject)Instantiate(follower, new Vector3(playerPos.x + Random.Range(50, 100), 0, playerPos.z+ Random.Range(50, 100)), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
