using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItFollows : MonoBehaviour {
	public GameObject follower;
	public GameObject player;
	public Vector3 offset;
    private Director director;

    // Use this for initialization
    void Start () {
        director = GameObject.Find("GameManager").GetComponent<Director>();
        player = director.getPlayer();
        //player = GameObject.FindGameObjectWithTag ("Player");
        Vector3 playerPos = player.transform.position;
		//GameObject followerClone = (GameObject)Instantiate(follower, new Vector3(playerPos.x + offset.x, 0 + offset.y, playerPos.z + offset.z), Quaternion.identity);

		for (int i = 0; i < 10; i++) {
			GameObject followerClone = (GameObject)Instantiate (follower, new Vector3 (playerPos.x + Random.Range (10, 60), 0, playerPos.z + Random.Range (10, 60)), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
