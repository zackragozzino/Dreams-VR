using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CreepyCrawlers : MonoBehaviour {

   public GameObject crawler;
   public GameObject center;
   public int numStartCrawlers = 0;
   public int maxCrawlers = 10;
   public float radius = 5.0f;
   public float speed = 1.0f;
   public float restTime = 0.3f;
   public float timeUntilDisperse = 10.0f;
   private List<GameObject> crawlers;
   private float closest = 0.3f;
   private bool moveCloser;
   private bool disperse;
   private bool generate;
   private bool crawl;

   public CreepyCrawlers() {
      this.crawlers  = new List<GameObject>();
      this.moveCloser = true;
      this.generate = true;
      this.crawl = false;
      this.disperse = false;
   }

   // Use this for initialization
   void Start () {
      // create crawlers around player
      InvokeRepeating("GenerateInCircle", 0.0f, 0.4f);
      // set timer until dispersal
      Invoke("setDisperseToTrue", this.timeUntilDisperse);
      // Play crawling sound
      FindObjectOfType<AudioManager> ().Play ("Creepy Crawlies");
      if (center == null)
         center = GameObject.FindGameObjectWithTag ("Player");
   }

   void GenerateInCircle() {
      if (this.generate == true) {
         float crawlerAngle = 0;
         float angleIncrement = 2 * Mathf.PI / this.maxCrawlers;
         for (int i = 0; i < this.maxCrawlers; i++) {
               float r = Random.Range(-1.0f, 1.0f);
               float x = Mathf.Sin(crawlerAngle  + r) * this.radius + this.center.transform.position.x;
               float z = Mathf.Cos(crawlerAngle + r) * this.radius + this.center.transform.position.z;
               Vector3 newPos = new Vector3(x + r, 0.3f, z + r);
               GameObject crawlThing = (GameObject)Instantiate(this.crawler, newPos, Quaternion.identity);
         crawlThing.transform.parent = transform;
               crawlers.Add(crawlThing);
               crawlerAngle += angleIncrement;
         }
      }
   }
   
   void Update () {  
      // for each crawler
      foreach (GameObject c in crawlers.ToArray()) {
         // if super far away from player, just delete
         if (c.transform.position.y > center.transform.position.y * 3) {
            Destroy(c, 0.2f);
            this.crawlers.Remove(c);
         }
         // get vector from crawler to player
         Vector3 towardsCenter = new Vector3(this.center.transform.position.x, 0, this.center.transform.position.z)- c.transform.position; 
         // if we are dispersing, hand crawler and outwards direction to moveCrawlerAway
         if (this.disperse) {
            moveCrawlerAway(c, -1 * towardsCenter);
         }
         // else we want to move closer or circle the player
         else {
            moveCrawler(c, towardsCenter);
         }
      }
      // wait for a little before moving again
      StartCoroutine(Wait(this.restTime));

      // if there are no more crawlers, stop the crawling sound
      if(this.crawlers.Count == 0)
         FindObjectOfType<AudioManager> ().Pause("Creepy Crawlies");
   }

   private void moveCrawler(GameObject c, Vector3 towardsCenter) {
      float r = Random.Range(-1.0f, 1.0f);
      // if in radius and moveCloser == true
      if (towardsCenter.magnitude > closest && this.moveCloser) {
         Vector3 translate = (towardsCenter + new Vector3(r, 0.0f, r))* this.speed * Time.deltaTime;
         c.transform.Translate(translate);
      }
      // if this object isn't in the radius, we want to move in a circle around the player
      else {
         this.moveCloser = false;
         // all of this is just math to get the correct tangent direction
         Vector3 tangent;
         Vector3 t1 = Vector3.Cross( towardsCenter, Vector3.forward );
         Vector3 t2 = Vector3.Cross( towardsCenter, Vector3.up );
         if (t1.magnitude > t2.magnitude) {
               tangent = t2;
         } else {
               tangent = t2;
         }
         Vector3 newDir = new Vector3(tangent.x + r*2, 0.0f, tangent.z + r*2);
         Vector3 translate = newDir * this.speed * Time.deltaTime;
         c.transform.Translate(translate);
      }
   }

   private IEnumerator Wait(float timeWait) {
      this.moveCloser = false;
      yield return new WaitForSecondsRealtime(timeWait);
      this.moveCloser = true;
   }

   private void setDisperseToTrue() {
      this.disperse = true;
      this.generate = false;
   }

   private void moveCrawlerAway(GameObject c, Vector3 awayFromCenter) {
      float r = Random.Range(-1.0f, 1.0f);
      Vector3 translate = (awayFromCenter + new Vector3(r, 0.0f, r))* this.speed * Time.deltaTime;
      c.transform.Translate(translate);
      // if further than the effective radius, then just delete
      if (awayFromCenter.magnitude > (this.radius)) {
            Destroy(c, 0.2f);
            this.crawlers.Remove(c);
      }
   }
}