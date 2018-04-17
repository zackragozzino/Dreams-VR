using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claustrophobia : MonoBehaviour {

	public float wallHeight;
	public float wallWidth;
	public float wallDepth;
	public float moveSpeed;
	public float expRadius = 5.0f;
	public float expPower = 10.0f;
	public float finalX = 0.0f;
	public bool explosion = false;
	public bool drop = false;
	public bool shrink = false;
	public bool lift = false;
	public bool collapse = false;
	public GameObject Wall;

	private float radius;
	private bool final = false;
	public bool hasFinalX = false;
	private GameObject player;

	private int count = 0;
	private GameObject wall1; //front
	private GameObject wall2; //back, opposite of wall 1 
	private GameObject wall3; //right, rotated 1
	private GameObject wall4; //left, rotated 2, opposite of wall 3
	private Transform wall1T;
	private Transform wall2T;
	private Transform wall3T;
	private Transform wall4T;
	private Rigidbody wall1RB;
	private Rigidbody wall2RB;
	private Rigidbody wall3RB;
	private Rigidbody wall4RB;

	//private Transform center;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");

		wall1 = (GameObject)Instantiate(Wall, player.transform.position + new Vector3(0,wallHeight/2,(wallWidth/2 - wallDepth/2)), Quaternion.identity);
		wall1.transform.localScale += new Vector3(wallWidth,wallHeight,wallDepth);

		wall2 = (GameObject)Instantiate(Wall, player.transform.position + new Vector3(0,wallHeight/2,(-wallWidth/2 + wallDepth/2)), Quaternion.identity);
		wall2.transform.localScale += new Vector3(wallWidth,wallHeight,wallDepth);

		wall3 = (GameObject)Instantiate(Wall, player.transform.position + new Vector3((wallWidth/2 - wallDepth/2),wallHeight/2, 0), Quaternion.identity * Quaternion.Euler(0, 90, 0));
		wall3.transform.localScale += new Vector3(wallWidth,wallHeight,wallDepth);

		wall4 = (GameObject)Instantiate(Wall, player.transform.position + new Vector3((-wallWidth/2 + wallDepth/2),wallHeight/2,0), Quaternion.identity * Quaternion.Euler(0, 90, 0));
		wall4.transform.localScale += new Vector3(wallWidth,wallHeight,wallDepth);

		wall1T = wall1.GetComponent<Transform> ();
		wall2T = wall2.GetComponent<Transform> ();
		wall3T = wall3.GetComponent<Transform> ();
		wall4T = wall4.GetComponent<Transform> ();

	}
	
	// Update is called once per frame
	void Update () {
		float step = moveSpeed * Time.deltaTime;
		radius = Vector3.Distance (wall1T.position, wall2T.position);
		if (radius > wallWidth / 5 && !final) {
			wall1T.position = Vector3.MoveTowards (wall1T.position, wall2T.position, step);
			wall2T.position = Vector3.MoveTowards (wall2T.position, wall1T.position, step);
			wall3T.position = Vector3.MoveTowards (wall3T.position, wall4T.position, step);
			wall4T.position = Vector3.MoveTowards (wall4T.position, wall3T.position, step);
			wall1T.localScale -= new Vector3 (2 * step, 0f, 0f);
			wall2T.localScale -= new Vector3 (2 * step, 0f, 0f);
			wall3T.localScale -= new Vector3 (2 * step, 0f, 0f);
			wall4T.localScale -= new Vector3 (2 * step, 0f, 0f);
		} else {
			final = true;
			if (!hasFinalX) {
				finalX = wall1T.localScale.x;
				hasFinalX = true;
			}
			count++;
			if (count > 1) { //>120
				if (explosion) {
					wall1RB = wall1.AddComponent<Rigidbody> ();
					wall2RB = wall2.AddComponent<Rigidbody> ();
					wall3RB = wall3.AddComponent<Rigidbody> ();
					wall4RB = wall4.AddComponent<Rigidbody> ();
					wall1RB.isKinematic = false;
					wall2RB.isKinematic = false;
					wall3RB.isKinematic = false;
					wall4RB.isKinematic = false;
					wall1RB.AddExplosionForce (expPower, player.transform.position, expRadius, 3.0F);
					wall2RB.AddExplosionForce (expPower, player.transform.position, expRadius, 3.0F);
					wall3RB.AddExplosionForce (expPower, player.transform.position, expRadius, 3.0F);
					wall4RB.AddExplosionForce (expPower, player.transform.position, expRadius, 3.0F);

					explosion = false;
				} else if (lift) {
					wall1T.Translate (new Vector3 (0f, 4f * step, 0f));
					wall2T.Translate (new Vector3 (0f, 4f * step, 0f));
					wall3T.Translate (new Vector3 (0f, 4f * step, 0f));
					wall4T.Translate (new Vector3 (0f, 4f * step, 0f));
				} else if (drop) {
					wall1T.Translate (new Vector3 (0f, -4f * step, 0f));
					wall2T.Translate (new Vector3 (0f, -4f * step, 0f));
					wall3T.Translate (new Vector3 (0f, -4f * step, 0f));
					wall4T.Translate (new Vector3 (0f, -4f * step, 0f));
				} else if (shrink && wall1T.localScale.x >= 0.1) {
					wall1T.localScale -= new Vector3 (step * 3, step * 3, 0);
					wall2T.localScale -= new Vector3 (step * 3, step * 3, 0);
					wall3T.localScale -= new Vector3 (step * 3, step * 3, 0);
					wall4T.localScale -= new Vector3 (step * 3, step * 3, 0);
				} else if (collapse && wall1T.localScale.x >  finalX - wallDepth*2.1) {
					wall1T.localScale -= new Vector3 (wallDepth, 0, 0);
					wall2T.localScale -= new Vector3 (wallDepth, 0, 0);
					wall3T.localScale -= new Vector3 (wallDepth, 0, 0);
					wall4T.localScale -= new Vector3 (wallDepth, 0, 0);
					wall1RB = wall1.AddComponent<Rigidbody> ();
					wall2RB = wall2.AddComponent<Rigidbody> ();
					wall3RB = wall3.AddComponent<Rigidbody> ();
					wall4RB = wall4.AddComponent<Rigidbody> ();
					wall1RB.isKinematic = false;
					wall2RB.isKinematic = false;
					wall3RB.isKinematic = false;
					wall4RB.isKinematic = false;
					wall1RB.AddForceAtPosition (Vector3.right , new Vector3 (wall1T.localScale.x / 2, wallHeight - wallHeight / 4));
					wall2RB.AddForceAtPosition (-Vector3.right , new Vector3 (wall1T.localScale.x / 2, wallHeight - wallHeight / 4));
					wall3RB.AddForceAtPosition (Vector3.forward , new Vector3 (wall1T.localScale.x / 2, wallHeight - wallHeight / 4));
					wall4RB.AddForceAtPosition (-Vector3.forward , new Vector3 (wall1T.localScale.x / 2, wallHeight - wallHeight / 4));
					collapse = false;
				}
				/* } else if (collapse){
					wall1.transform.RotateAround (wall1T.position, Vector3.right, step* 90f);
					wall2.transform.RotateAround (wall1T.position, -Vector3.right, step * 90f);
					wall3.transform.RotateAround (wall1T.position, Vector3.forward, step *90f);
					wall4.transform.RotateAround (wall1T.position, -Vector3.forward, step *90f);

				}*/
				if (wall1T.localScale.x <= 0.1 || wall1T.position.y < player.transform.position.y -100 || count > 3600) {
					Destroy (wall1T.gameObject);
					Destroy (wall2T.gameObject);
					Destroy (wall3T.gameObject);
					Destroy (wall4T.gameObject);
					Destroy (gameObject);
				}
			}
		}
	}
}
