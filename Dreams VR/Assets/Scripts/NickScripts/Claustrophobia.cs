using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claustrophobia : MonoBehaviour
{

	public float wallHeight;
	public float wallWidth;
	public float wallDepth;
	public float moveSpeed;
	public float expRadius = 5.0f;
	public float expPower = 10.0f;
	public Vector3 finalScale;


	public enum WallTrans { explosion, drop, shrink, lift, collapse, none };
	public WallTrans wallTrans;

	public GameObject Wall;
	public float timeToRelease;
	private float radius;
	private bool final = false;
	private bool hasFinalScale = false;
	public bool willMove = true;
	private GameObject player;

	private int count = 0;
	public GameObject wall1; //front
	public GameObject wall2; //back, opposite of wall 1 
	public GameObject wall3; //right, rotated 1
	public GameObject wall4; //left, rotated 2, opposite of wall 3
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
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		if (wall1 == null)
		{
			wall1 = (GameObject)Instantiate(Wall, player.transform.position + new Vector3(0, wallHeight / 2, (wallWidth / 2 - wallDepth / 2)), Quaternion.identity);
			wall1.transform.localScale += new Vector3(wallWidth, wallHeight, wallDepth);
		}
		if (!wall2)
		{
			wall2 = (GameObject)Instantiate(Wall, player.transform.position + new Vector3(0, wallHeight / 2, (-wallWidth / 2 + wallDepth / 2)), Quaternion.identity);
			wall2.transform.localScale += new Vector3(wallWidth, wallHeight, wallDepth);
		}
		if (!wall3)
		{
			wall3 = (GameObject)Instantiate(Wall, player.transform.position + new Vector3((wallWidth / 2 - wallDepth / 2), wallHeight / 2, 0), Quaternion.identity * Quaternion.Euler(0, 90, 0));
			wall3.transform.localScale += new Vector3(wallWidth, wallHeight, wallDepth);
		}
		if (!wall4)
		{
			wall4 = (GameObject)Instantiate(Wall, player.transform.position + new Vector3((-wallWidth / 2 + wallDepth / 2), wallHeight / 2, 0), Quaternion.identity * Quaternion.Euler(0, 90, 0));
			wall4.transform.localScale += new Vector3(wallWidth, wallHeight, wallDepth);
		}
		wall1T = wall1.GetComponent<Transform>();
		wall2T = wall2.GetComponent<Transform>();
		wall3T = wall3.GetComponent<Transform>();
		wall4T = wall4.GetComponent<Transform>();

	}
	IEnumerator Waiter()
	{

		//Wait for 4 seconds
		yield return new WaitForSeconds(seconds: 10);
	}

	// Update is called once per frame
	void Update()
	{
		if (!willMove)
		{
			final = true;
		}
		float step = moveSpeed * Time.deltaTime;
		radius = Vector3.Distance(wall1.transform.position, wall2.transform.position);
		if (radius > wallWidth / 5 && !final)
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
			{ //>120
				if (wallTrans == WallTrans.explosion)
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

					wallTrans = WallTrans.none;
				}
				else if (wallTrans == WallTrans.lift)
				{
					//SetAllCollidersStatus(false, wall1);
					///SetAllCollidersStatus(false, wall2);
					//SetAllCollidersStatus(false, wall3);
					//SetAllCollidersStatus(false, wall4);

					wall1T.Translate(new Vector3(0f, 4f * step, 0f));
					wall2T.Translate(new Vector3(0f, 4f * step, 0f));
					wall3T.Translate(new Vector3(0f, 4f * step, 0f));
					wall4T.Translate(new Vector3(0f, 4f * step, 0f));
				}
				else if (wallTrans == WallTrans.drop)
				{
					//SetAllCollidersStatus(false, wall1);
					///SetAllCollidersStatus(false, wall2);
					//SetAllCollidersStatus(false, wall3);
					//SetAllCollidersStatus(false, wall4);

					wall1T.Translate(new Vector3(0f, -4f * step, 0f));
					wall2T.Translate(new Vector3(0f, -4f * step, 0f));
					wall3T.Translate(new Vector3(0f, -4f * step, 0f));
					wall4T.Translate(new Vector3(0f, -4f * step, 0f));
				}
				else if (wallTrans == WallTrans.shrink && wall1T.localScale.x >= 0.1)
				{
					wall1T.localScale -= new Vector3(step * 3, step * 3, 0);
					wall2T.localScale -= new Vector3(step * 3, step * 3, 0);
					wall3T.localScale -= new Vector3(step * 3, step * 3, 0);
					wall4T.localScale -= new Vector3(step * 3, step * 3, 0);
				}
				else if (wallTrans == WallTrans.collapse)
				{

					if (wall1T.localScale.x > finalScale.x - finalScale.z * 2)
					{
						Debug.Log("x local" + wall1T.localScale.x);
						Debug.Log("z local" + wall1T.localScale.z);
						Debug.Log("x final" + finalScale.x);
						Debug.Log("z final" + finalScale.z);
						wall1T.localScale -= new Vector3(finalScale.z, 0, 0);
						wall2T.localScale -= new Vector3(finalScale.z, 0, 0);
						wall3T.localScale -= new Vector3(finalScale.z, 0, 0);
						wall4T.localScale -= new Vector3(finalScale.z, 0, 0);
						count = 0;
					}
					else
					{
						if (count > 60 * 5)
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
							wall3RB.AddForceAtPosition(wall3.transform.forward * 100, new Vector3(wall3.transform.position.x, wall3.transform.position.y + wall3.transform.position.y, wall3.transform.position.z));
							wall4RB.AddForceAtPosition(-wall4.transform.forward * 100, new Vector3(wall4.transform.position.x, wall4.transform.position.y + wall4.transform.position.y, wall4.transform.position.z));
							wallTrans = WallTrans.none;
						}
					}
				}
				/* } else if (collapse){
                    wall1.transform.RotateAround (wall1T.position, Vector3.right, step* 90f);
                    wall2.transform.RotateAround (wall1T.position, -Vector3.right, step * 90f);
                    wall3.transform.RotateAround (wall1T.position, Vector3.forward, step *90f);
                    wall4.transform.RotateAround (wall1T.position, -Vector3.forward, step *90f);

                }*/
				if (wall1T.localScale.x <= 0.1 || wall1T.position.y < player.transform.position.y - 100 || count > 60*50)
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



public void SetAllCollidersStatus(bool active, GameObject obj)
{
	foreach (Collider c in obj.GetComponents<Collider>())
	{
		c.enabled = active;

	 }
}
}