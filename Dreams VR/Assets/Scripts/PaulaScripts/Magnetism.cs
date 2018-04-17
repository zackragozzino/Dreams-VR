using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetism : MonoBehaviour {

	public GameObject magnet;
	public GameObject player;
	public int effectiveRadius = 10;
	public float magneticStrength = 1;

	// Use this for initialization
	void Start () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}
      if (gameObject) {
         magnet = gameObject;
      } 
      else {
         magnet = GameObject.FindGameObjectWithTag("Magnet");
      }
      checkCollider(magnet);
      checkRigidbody(magnet);
	}

   void checkCollider(GameObject item){
      Collider collider = item.GetComponent(typeof(Collider)) as Collider;
      if (!collider) {
         collider = item.AddComponent(typeof(BoxCollider)) as BoxCollider;
      }
   }
	
   void checkRigidbody(GameObject item) {
      Rigidbody rigidbody = item.GetComponent(typeof(Rigidbody)) as Rigidbody;
      if (!rigidbody) {
         rigidbody = item.AddComponent(typeof(Rigidbody)) as Rigidbody;
      }
      rigidbody.constraints = RigidbodyConstraints.FreezePositionY | 
         RigidbodyConstraints.FreezeRotationX |
         RigidbodyConstraints.FreezeRotationY | 
         RigidbodyConstraints.FreezeRotationZ;
   }

	// Update is called once per frame
	void Update () {
      Vector3 difference = magnet.transform.position - player.transform.position;
      difference = new Vector3(difference.x, 0.0f, difference.z);
      if (difference.magnitude <= this.effectiveRadius) {
         Rigidbody rigidbody = magnet.GetComponent(typeof(Rigidbody)) as Rigidbody;
         Vector3 force = difference.normalized * this.magneticStrength;
         rigidbody.AddForce(force, ForceMode.VelocityChange);
      }
	}
}
