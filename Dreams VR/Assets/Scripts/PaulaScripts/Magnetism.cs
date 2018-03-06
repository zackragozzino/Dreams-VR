using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetism : MonoBehaviour {

	public GameObject magnet;
	public GameObject player;
	public int effectiveRadius;
	public float magneticStrength;

	// Use this for initialization
	void Start () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 difference = new Vector3(magnet.transform.position.y, 0.0f, magnet.transform.position.z) - new Vector3(player.transform.position.y, 0.0f, player.transform.position.z);
		if (difference.magnitude <= this.effectiveRadius) {
			Rigidbody rigidbody = magnet.GetComponent(typeof(Rigidbody)) as Rigidbody;
			if (!rigidbody) {
				rigidbody = magnet.AddComponent(typeof(Rigidbody)) as Rigidbody;
			}
			rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
			rigidbody.AddForce(difference.normalized * this.magneticStrength);
		}
	}
}
