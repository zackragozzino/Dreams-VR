using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claustrophobia : MonoBehaviour
{

	public float wallHeight = 5.0f; //for generated walls
	public float wallWidth = 10.0f; //for generated walls
	public float wallDepth = 2.0f; //for generated walls
	public float moveSpeed = 2.0f; //determines how fast walls close in
	public float expRadius = 5.0f; //for explosion
	public float expPower = 10.0f; //for explosion (not collapseExplosive)
	public float rotateSpeed = 15.0f; // for collapse
	public float timeToClose = 10.0f; // time until claustrophia closing in begins after walls raise up.
	public float timeToRelease; //time until walls begin transition after shrinking or starting the script
	public float finalWallSpace = 5.0f; //space between each wall at the end (make lower for scarier effect
	public bool willCloseIn = true; //sets whether or not walls shrink towards you
	public bool triggerRise = false;
	public bool wallTrigger = false;


	public enum WallTransition { explosion, drop, shrink, lift, collapse, collapseExplosive, none };
	//explosion throws all walls out in a glorious rage
	//drop causes walls to go through floor * works with gabriella's
	//shrink is weird
	//lift causes walls to go to the sky * works with gabriella's
	//collapse rotates the walls almost about their bottom axes then drops them
	//collapseExplosive shrinks so that edges don't overlap then tips them over

	public WallTransition wallTrans;

	private float angle;
	private float radius;
	private bool final = false;
	private bool hasFinalScale = false;
	private Vector3 finalScale;
	private int count = 0;
	private float ground = -1;
	private bool triggerClose = false;

	public GameObject player;
	public GameObject WallPrefab;
	public GameObject wall1; //front
	public GameObject wall2; //back, opposite of wall 1 
	public GameObject wall3; //right, rotated 1
	public GameObject wall4; //left, rotated 2, opposite of wall 3

	//All these below aren't really necessary but I don't feel like refactoring them yet and feel like
	//it may make the code look cleaner
	private Transform wall1T;
	private Transform wall2T;
	private Transform wall3T;
	private Transform wall4T;
	private Rigidbody wall1RB;
	private Rigidbody wall2RB;
	private Rigidbody wall3RB;
	private Rigidbody wall4RB;

	// Use this for initialization
	public void OnTriggerEnter()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		if (!wall1)
		{
			wall1 = (GameObject)Instantiate(WallPrefab, player.transform.position + new Vector3(0, ground - (wallHeight / 2), (wallWidth / 2 - wallDepth / 2)), Quaternion.identity);
			wall1.transform.localScale += new Vector3(wallWidth, wallHeight, wallDepth);
		}
		if (!wall2)
		{
			wall2 = (GameObject)Instantiate(WallPrefab, player.transform.position + new Vector3(0, ground - (wallHeight / 2), (-wallWidth / 2 + wallDepth / 2)), Quaternion.identity);
			wall2.transform.localScale += new Vector3(wallWidth, wallHeight, wallDepth);
		}
		if (!wall3)
		{
			wall3 = (GameObject)Instantiate(WallPrefab, player.transform.position + new Vector3((wallWidth / 2 - wallDepth / 2), ground - (wallHeight / 2), 0), Quaternion.identity * Quaternion.Euler(0, 90, 0));
			wall3.transform.localScale += new Vector3(wallWidth, wallHeight, wallDepth);
		}
		if (!wall4)
		{
			wall4 = (GameObject)Instantiate(WallPrefab, player.transform.position + new Vector3((-wallWidth / 2 + wallDepth / 2), ground - (wallHeight / 2), 0), Quaternion.identity * Quaternion.Euler(0, 90, 0));
			wall4.transform.localScale += new Vector3(wallWidth, wallHeight, wallDepth);
		}
		wall1T = wall1.GetComponent<Transform>();
		wall2T = wall2.GetComponent<Transform>();
		wall3T = wall3.GetComponent<Transform>();
		wall4T = wall4.GetComponent<Transform>();
		wallTrigger = true;
		triggerRise = true;
		Debug.Log("entered");

	}
	void switchState()
	{
		triggerRise = false;
		triggerClose = true;
	}
	// Update is called once per frame
	void Update()
	{
		if (wallTrigger)
		{
			if (!willCloseIn)
			{
				final = true;
			}
			float step = moveSpeed * Time.deltaTime;
			radius = Vector3.Distance(wall1.transform.position, wall2.transform.position);
			if (radius > finalWallSpace && !final)
			{

					if (wall1T.position.y < ground + (wallHeight / 2))
					{
						wall1T.Translate(new Vector3(0f, step, 0f));
						wall2T.Translate(new Vector3(0f, step, 0f));
						wall3T.Translate(new Vector3(0f, step, 0f));
						wall4T.Translate(new Vector3(0f, step, 0f));
					}
					else
					{
						Invoke("switchState", timeToClose);
					}
				if (triggerClose)
				{
					wall1T.position = Vector3.MoveTowards(wall1T.position, wall2T.position, step);
					wall2T.position = Vector3.MoveTowards(wall2T.position, wall1T.position, step);
					wall3T.position = Vector3.MoveTowards(wall3T.position, wall4T.position, step);
					wall4T.position = Vector3.MoveTowards(wall4T.position, wall3T.position, step);
					wall1T.localScale -= new Vector3(2 * step, 0f, 0f);
					wall2T.localScale -= new Vector3(2 * step, 0f, 0f);
					wall3T.localScale -= new Vector3(2 * step, 0f, 0f);
					wall4T.localScale -= new Vector3(2 * step, 0f, 0f);
				}
			}
			else
			{
				final = true;
				if (!hasFinalScale)
				{
					finalScale = wall1T.localScale;
					hasFinalScale = true;
				}
				count++;
				if (count > timeToRelease * 60)
				{
					if (wallTrans == WallTransition.explosion)
					{
						wall1RB = wall1.AddComponent<Rigidbody>();
						wall2RB = wall2.AddComponent<Rigidbody>();
						wall3RB = wall3.AddComponent<Rigidbody>();
						wall4RB = wall4.AddComponent<Rigidbody>();
						wall1RB.isKinematic = false;
						wall2RB.isKinematic = false;
						wall3RB.isKinematic = false;
						wall4RB.isKinematic = false;
						wall1RB.AddExplosionForce(expPower, player.transform.position, expRadius, 3.0F);
						wall2RB.AddExplosionForce(expPower, player.transform.position, expRadius, 3.0F);
						wall3RB.AddExplosionForce(expPower, player.transform.position, expRadius, 3.0F);
						wall4RB.AddExplosionForce(expPower, player.transform.position, expRadius, 3.0F);

						wallTrans = WallTransition.none;
					}

					// no need for "wallTrans = WallTransition.none;" for next 3 bc last if takes care of them
					else if (wallTrans == WallTransition.lift)
					{
						wall1T.Translate(new Vector3(0f, 4f * step, 0f));
						wall2T.Translate(new Vector3(0f, 4f * step, 0f));
						wall3T.Translate(new Vector3(0f, 4f * step, 0f));
						wall4T.Translate(new Vector3(0f, 4f * step, 0f));
					}
					else if (wallTrans == WallTransition.drop)
					{
						wall1T.Translate(new Vector3(0f, -4f * step, 0f));
						wall2T.Translate(new Vector3(0f, -4f * step, 0f));
						wall3T.Translate(new Vector3(0f, -4f * step, 0f));
						wall4T.Translate(new Vector3(0f, -4f * step, 0f));
					}
					else if (wallTrans == WallTransition.shrink && wall1T.localScale.x >= 0.1)
					{
						wall1T.localScale -= new Vector3(step * 3, step * 3, 0);
						wall2T.localScale -= new Vector3(step * 3, step * 3, 0);
						wall3T.localScale -= new Vector3(step * 3, step * 3, 0);
						wall4T.localScale -= new Vector3(step * 3, step * 3, 0);
					}

					else if (wallTrans == WallTransition.collapse)
					{
						angle = 0;
						if (wall1.transform.localRotation.eulerAngles.x <= 89.0f)
						{ //90 was being buggy... floating pt impercision
							angle += step * rotateSpeed;
							wall1.transform.RotateAround(wall1T.position - new Vector3(0, wall1T.position.y, 0), Vector3.right, angle);
							wall2.transform.RotateAround(wall2T.position - new Vector3(0, wall2T.position.y, 0), -Vector3.right, angle);
							wall3.transform.RotateAround(wall3T.position - new Vector3(0, wall3T.position.y, 0), -Vector3.forward, angle);
							wall4.transform.RotateAround(wall4T.position - new Vector3(0, wall4T.position.y, 0), Vector3.forward, angle);
						}
						else
						{
							wall1RB = wall1.AddComponent<Rigidbody>();
							wall2RB = wall2.AddComponent<Rigidbody>();
							wall3RB = wall3.AddComponent<Rigidbody>();
							wall4RB = wall4.AddComponent<Rigidbody>();
							wall1RB.isKinematic = false;
							wall2RB.isKinematic = false;
							wall3RB.isKinematic = false;
							wall4RB.isKinematic = false;
							wallTrans = WallTransition.none;
						}
					}
					else if (wallTrans == WallTransition.collapseExplosive)
					{

						if (wall1T.localScale.x > finalScale.x - finalScale.z * 2)
						{
							//Debug.Log("x local" + wall1T.localScale.x);
							//Debug.Log("z local" + wall1T.localScale.z);
							//Debug.Log("x final" + finalScale.x);
							//Debug.Log("z final" + finalScale.z);
							wall1T.localScale -= new Vector3(finalScale.z, 0, 0);
							wall2T.localScale -= new Vector3(finalScale.z, 0, 0);
							wall3T.localScale -= new Vector3(finalScale.z, 0, 0);
							wall4T.localScale -= new Vector3(finalScale.z, 0, 0);
							count = 0;
						}
						else
						{
							if (count > 60 * timeToRelease)
							{
								wall1RB = wall1.AddComponent<Rigidbody>();
								wall2RB = wall2.AddComponent<Rigidbody>();
								wall3RB = wall3.AddComponent<Rigidbody>();
								wall4RB = wall4.AddComponent<Rigidbody>();
								wall1RB.isKinematic = false;
								wall2RB.isKinematic = false;
								wall3RB.isKinematic = false;
								wall4RB.isKinematic = false;
								wall1RB.AddForceAtPosition(wall1.transform.forward * 100, new Vector3(wall1.transform.position.x, wall1.transform.position.y + wall1.transform.position.y, wall1.transform.position.z));
								wall2RB.AddForceAtPosition(-wall2.transform.forward * 100, new Vector3(wall2.transform.position.x, wall2.transform.position.y + wall2.transform.position.y, wall2.transform.position.z));
								wall3RB.AddForceAtPosition(wall3.transform.right * 100, new Vector3(wall3.transform.position.x, wall3.transform.position.y + wall3.transform.position.y, wall3.transform.position.z));
								wall4RB.AddForceAtPosition(-wall4.transform.right * 100, new Vector3(wall4.transform.position.x, wall4.transform.position.y + wall4.transform.position.y, wall4.transform.position.z));
								wallTrans = WallTransition.none;
							}
						}
					}
					//destroys them after wall shrinks too small, moves too far away up or down, or after 3000 frames go by
					if (wall1.transform.localScale.x <= 0.1 || wall1.transform.position.y < player.transform.position.y - 100 || count > 60 * 50)
					{
						Destroy(wall1T.gameObject);
						Destroy(wall2T.gameObject);
						Destroy(wall3T.gameObject);
						Destroy(wall4T.gameObject);
						Destroy(gameObject);
					}
				}
			}
		}
	}
	//May come in handy later...
	public void SetAllCollidersStatus(bool active, GameObject obj)
	{
		foreach (Collider c in obj.GetComponents<Collider>())
		{
			c.enabled = active;

		 }
	}
	//to set all the walls colliders off:
	//SetAllCollidersStatus(false, wall1);
	///SetAllCollidersStatus(false, wall2);
	//SetAllCollidersStatus(false, wall3);
	//SetAllCollidersStatus(false, wall4);
	//could possibly use these after collapse's drop
}