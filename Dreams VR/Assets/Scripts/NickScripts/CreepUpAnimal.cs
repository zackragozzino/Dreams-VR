using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepUpAnimal : MonoBehaviour {
	private GameObject Player;
	private Transform playerT;
    private Animator anim;
    private Director director;
	private float distance;
	private bool visible = false;
	private bool lastView = false;
	private bool encountered = false;
    private bool lastViewBegin = false;
	public float moveSpeed = 20f;
	public float rotateSpeed = 3f;
	private float timer;

	void Start()
	{
        director = GameObject.Find("GameManager").GetComponent<Director>();
        anim = GetComponent<Animator>();
		Player = director.getPlayer();
		playerT = Player.GetComponent<Transform> ();
		distance = Vector3.Distance (transform.position, playerT.position);
	}
	void OnBecameInvisible()
	{		
		visible = false;
		//Debug.Log ("Invisible!");
	}
	void OnBecameVisible()
	{
		if (!encountered) {
			encountered = true;
		}
		visible = true;
		//Debug.Log ("Visible!");
		if ( distance > 8) {
            FindObjectOfType<AudioManager>().Play("BearGrowl");
        }
        //too close!
        if (distance <= 6) {
            lastView = true;
		}
		/*Possibly use lineCast to see if blocked*/
	}
	void Update()
	{
		// creep up
		distance = Vector3.Distance (transform.position, playerT.position);
		if (!visible && encountered && distance > 4 && !lastView) {
			float step = moveSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, new Vector3(playerT.position.x, 0, playerT.position.z), step);
            Vector3 targetDir = playerT.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
		// walk and rotate
		else if (visible && !lastView) {
            Vector3 targetDir = playerT.position - transform.position;
			float step = rotateSpeed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);
			transform.rotation = Quaternion.LookRotation (newDir);
            if (Vector3.Angle(newDir, targetDir) > 10)
                anim.Play("walk");
        }
        if (lastView || distance < 4) {
            if (!lastViewBegin)
            {
                FindObjectOfType<AudioManager>().Play("BearAttack");
                timer = 1.5f;
                lastViewBegin = true;
            }
            anim.Play("attack");
            timer -= Time.deltaTime;
			if (timer <= 0) {
				Destroy (gameObject);
				director.GenerateNewWorld();
			}
		}
	}
}
