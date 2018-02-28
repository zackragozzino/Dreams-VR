using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepUp : MonoBehaviour {
	public GameObject Player;
	private Transform playerT;
	private float distance;
	private bool visible = false;
	public float moveSpeed = 25f;
	public float rotateSpeed = 0.5f;

	void Start()
	{
		playerT = Player.GetComponent<Transform> ();
		distance = Vector3.Distance (transform.position, playerT.position);
	}
	void OnBecameInvisible()
	{		
		visible = false;
		Debug.Log ("Invisible!");
	}
	void OnBecameVisible()
	{
		visible = true;
		Debug.Log ("Visible!");
		if ( distance > 9) {
			FindObjectOfType<AudioManager> ().Play ("FirstJumpScare");
		}
		//too close!
		if (distance < 10) {
			//CameraShake cs = Player.GetComponentInChildren<CameraShake> ();
			//cs.shakecamera();
			FindObjectOfType<AudioManager>().Play ("LastJumpScare");
		}
		/*Possibly use lineCast to see if blocked*/
	}
	void Update()
	{
		// creep up
		distance = Vector3.Distance (transform.position, playerT.position);
		if (!visible && distance > 5) {
			float step = moveSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, playerT.position, step);
		}
		// rotate
		if (visible) {
			Vector3 targetDir = playerT.position - transform.position;
			float step = rotateSpeed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);
			Debug.DrawRay (transform.position, newDir, Color.red);
			transform.rotation = Quaternion.LookRotation (newDir);
		}
		// too close!
	}
}
