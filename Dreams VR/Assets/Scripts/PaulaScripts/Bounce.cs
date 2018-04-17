using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour {

   public GameObject item;
   public PhysicMaterial material;
   public float bounciness = 0.8f;

	// Use this for initialization
	void Start () {
      if (gameObject) {
         this.item = gameObject;
      }
		Collider collider = item.GetComponent(typeof(Collider)) as Collider;
      if (!collider) {
         collider = item.AddComponent(typeof(BoxCollider)) as BoxCollider;
      } 
      if (!this.material) {
         this.material = new PhysicMaterial();
      }
      // Set correct settings of PhysicMaterial
      this.material.bounciness = this.bounciness;
      this.material.bounceCombine = PhysicMaterialCombine.Maximum;

      collider.material = this.material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
