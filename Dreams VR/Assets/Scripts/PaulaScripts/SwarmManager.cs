using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class SwarmManager : MonoBehaviour {

   public GameObject crawler;
   public GameObject player;
   public string script;
   public float radius = 5.0f;
   public float timeUntilDisperse = 10;
   private int numStartCrawlers = 0;
   private int maxCrawlers = 10;
   private List<GameObject> swarm;
   private bool generate = true;

   public SwarmManager() {
      this.swarm  = new List<GameObject>();
   }

   // Use this for initialization
   void Start () {
      // set timer until dispersal
      Invoke("setGenerateToFalse", this.timeUntilDisperse);
      // create crawlers around player
      InvokeRepeating("GenerateInCircle", 0.0f, 0.4f);
      if (player == null)
         player = GameObject.FindGameObjectWithTag ("Player");
   }

   void GenerateInCircle() {
      if (this.generate == true) {
         float crawlerAngle = 0;
         float angleIncrement = 2 * Mathf.PI / this.maxCrawlers;
         for (int i = 0; i < this.maxCrawlers; i++) {
            // Calculate the right placement of crawl thing in circle around player
            float r = UnityEngine.Random.Range(-1.0f, 1.0f);
            float x = Mathf.Sin(crawlerAngle  + r) * this.radius + this.player.transform.position.x;
            float z = Mathf.Cos(crawlerAngle + r) * this.radius + this.player.transform.position.z;
            Vector3 newPos = new Vector3(x + r, 0.0f, z + r);
            // get rotation to face towards player
            Quaternion newRotation = Quaternion.LookRotation(player.transform.position - newPos);

            // instantiate
            GameObject crawlThing = (GameObject)Instantiate(this.crawler, newPos, newRotation);


            crawlThing.transform.parent = transform;
            crawlerAngle += angleIncrement;

            crawlThing.AddComponent(Type.GetType(script));
         }
      }
   }

   private void setGenerateToFalse() {
      this.generate = false;
   }
}