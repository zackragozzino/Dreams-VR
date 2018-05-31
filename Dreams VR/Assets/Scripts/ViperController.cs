using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViperController : MonoBehaviour {

	private Animator animator;
	private bool isAttacking;
	private float speed = 3f;
	private GameObject player;
	private Director director;
	private AudioManager audm;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
		director = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Director>();
		player = director.getPlayer ();
		audm = FindObjectOfType<AudioManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isAttacking) {
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, player.transform.position, step);
			transform.LookAt (player.transform, Vector3.up);
		}
		
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Player") {
			director.GenerateNewWorld ();
		}
	}

	public void startAttacking(){
		if(!isAttacking){
			audm.Play ("Snake Hiss");
			animator.Play ("fastglide");
			isAttacking = true;
		}
	}
}
