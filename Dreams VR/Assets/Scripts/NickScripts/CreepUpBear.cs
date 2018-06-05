using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepUpBear : MonoBehaviour {
	public GameObject Player;
	private Transform playerT;
    private Animator anim;
	private float distance;
	private bool visible = false;
	private bool lastView = false;
	private bool encountered = false;
	public float moveSpeed = 25f;
	public float rotateSpeed = 0.5f;
	private float timer;

	void Start()
	{
        anim = GetComponent<Animator>();
		//Player = GameObject.FindGameObjectWithTag ("Player");
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
		if (!encountered) {
			encountered = true;
			//FindObjectOfType<AudioManager> ().Play ("FirstJumpScare");
		}
		visible = true;
		Debug.Log ("Visible!");
		if ( distance > 9) {
			//FindObjectOfType<AudioManager> ().Play ("FirstJumpScare");
		}
		//too close!
		if (distance < 5) {
            //CameraShake cs = Player.GetComponentInChildren<CameraShake> ();
            //cs.shakecamera();
            //FindObjectOfType<AudioManager> ().Play ("LastJumpScare");
            anim.Play("attack");
			if (lastView == false) {
				FindObjectOfType<AudioManager> ().Play ("BearAttack");
				timer = 5.0f;
				lastView = true;
			}
		}
		/*Possibly use lineCast to see if blocked*/
	}
	void Update()
	{
        if (visible)
            Debug.Log(visible);
		// creep up
		distance = Vector3.Distance (transform.position, playerT.position);
		if (!visible && encountered && distance > 5) {
			float step = moveSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, new Vector3(playerT.position.x, 0, playerT.position.z), step);
            Vector3 targetDir = playerT.position - transform.position;
            //float step = rotateSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
		// creepy rotate 
		if (visible && !lastView) {
            Vector3 targetDir = playerT.position - transform.position;
			float step = rotateSpeed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);
			Debug.DrawRay (transform.position, newDir, Color.red);
			transform.rotation = Quaternion.LookRotation (newDir);
            if (Vector3.Angle(newDir, targetDir) >10)
                anim.Play("walk");
        }
        if (lastView) {
			timer -= Time.deltaTime;
			if (timer <= 0) {
				//FindObjectOfType<AudioManager> ().Pause ("FirstJumpScare");
				Destroy (gameObject);
			}
		}
		// too close!
	}
}
