using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItRotates : MonoBehaviour {
	public GameObject rotator;
	public GameObject player;
	//public Vector3 offset;
    public float rotateSpeed = 2f;
    public int numRotators = 10;
    private Director director;

    // Use this for initialization
    void Start () {
        director = GameObject.Find("GameManager").GetComponent<Director>();
        player = director.getPlayer();
        Vector3 playerPos = player.transform.position;

		for (int i = 0; i < numRotators; i++) {
			GameObject rotatorClone = (GameObject)Instantiate(rotator, new Vector3(playerPos.x + Random.Range(-10, 10), 6, playerPos.z + Random.Range(-10, 10)), Quaternion.identity);
		}
	}
}
