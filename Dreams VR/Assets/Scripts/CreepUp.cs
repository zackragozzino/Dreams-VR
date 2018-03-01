using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepUp : MonoBehaviour {
	private GameObject Player;
	private Transform playerT;
	private float distance;
	private bool visible = false;
	private bool lastView = false;
	private bool encountered = false;
	public float moveSpeed = 25f;
	public float rotateSpeed = 0.5f;
	private float timer;

	void Start()
	{
		Player = GameObject.FindGameObjectWithTag ("Player");
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
		encountered = true;
		visible = true;
		Debug.Log ("Visible!");
		if ( distance > 9) {
			FindObjectOfType<AudioManager> ().Play ("FirstJumpScare");
		}
		//too close!
		if (distance < 10) {
			//CameraShake cs = Player.GetComponentInChildren<CameraShake> ();
			//cs.shakecamera();
			FindObjectOfType<AudioManager> ().Play ("LastJumpScare");
			if (lastView == false) {
				timer = 5.0f;
				lastView = true;
			}
		}
		/*Possibly use lineCast to see if blocked*/
	}
	void Update()
	{
		// creep up
		distance = Vector3.Distance (transform.position, playerT.position);
		if (!visible && encountered && distance > 5) {
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
		if (lastView) {
			timer -= Time.deltaTime;
			if (timer <= 0) {
				Destroy (gameObject);
			}
		}
		// too close!
	}
}
