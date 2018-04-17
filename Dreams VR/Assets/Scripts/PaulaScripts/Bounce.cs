using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour {

   public GameObject item;
   public PhysicMaterial material;
   public float bounciness = 0.0f;

	// Use this for initialization
	void Start () {
      if (gameObject) {
         this.item = gameObject;
      }
      Collider collider = getCollider(this.item);
      addRigidbody(this.item);
      checkBouncyMaterial();
      collider.material = this.material;
	}

   Collider getCollider(GameObject item){
      Collider collider = item.GetComponent(typeof(Collider)) as Collider;
      if (!collider) {
         collider = item.AddComponent(typeof(SphereCollider)) as SphereCollider;
      }
      return collider;
   }

   void addRigidbody(GameObject item) {
      Rigidbody rigidbody = item.GetComponent(typeof(Rigidbody)) as Rigidbody;
      if (!rigidbody) {
         rigidbody = item.AddComponent(typeof(Rigidbody)) as Rigidbody;
      }
      // rigidbody.constraints = RigidbodyConstraints.FreezePositionY | 
      //    RigidbodyConstraints.FreezeRotationX |
      //    RigidbodyConstraints.FreezeRotationY | 
      //    RigidbodyConstraints.FreezeRotationZ;
   }   
   void checkBouncyMaterial() {

      if (!this.material) {
         this.material = new PhysicMaterial();
      }
      // Set correct settings of PhysicMaterial
      if (this.bounciness == 0) {
         this.bounciness = Random.RandomRange(0.5f, 1.0f);
      }
      this.material.bounciness = this.bounciness;
      this.material.bounceCombine = PhysicMaterialCombine.Maximum;
   }
	
	// Update is called once per frame
	void Update () {
		
	}
}
