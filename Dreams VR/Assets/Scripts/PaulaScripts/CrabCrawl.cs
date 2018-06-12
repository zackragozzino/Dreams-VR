using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CrabCrawl : MonoBehaviour {
   public GameObject crawler;
   public GameObject player;
   public float radius = 5.0f;
   public float speed = 0.5f;
   public float restTime = 0.3f;
   public float timeUntilDisperse = 10.0f;
   private float closest = 0.3f;
   private bool moveCloser;
   private bool moveAway;

   public CrabCrawl() {
      this.moveCloser = true;
      this.moveAway = false;
   }

   // Use this for initialization
   void Start () {
      // set timer until dispersal
      Invoke("setDisperseToTrue", this.timeUntilDisperse);
      if (player == null)
         player = GameObject.FindGameObjectWithTag ("Player");
   }
   
   void Update () {  
      // get vector from crawler to player
      Vector3 towardsCenter = new Vector3(this.player.transform.position.x, 0, this.player.transform.position.z) - gameObject.transform.position; 
      // if we are dispersing, hand crawler and outwards direction to moveCrawlerAway
      if (this.moveAway) {
         moveCrawler(gameObject, -1 * towardsCenter);
      }
      // else we want to move closer or circle the player
      else {
         // move closer
         if (towardsCenter.magnitude > closest && this.moveCloser) {
            moveCrawler(gameObject, towardsCenter);
         }
         else {
            // move in Circle
            moveCrawlerInCircle(gameObject, towardsCenter);
         }
      }
      
      // wait for a little before moving again
      StartCoroutine(Wait(this.restTime));
   }

   private void moveCrawlerInCircle(GameObject c, Vector3 towardsCenter) {
      Vector3 newDir;
      float r = Random.Range(-3.0f, 3.0f);
      // if this object isn't as close as they could be, we want to move in a circle around the player
      this.moveCloser = false;
      // all of this is just math to get the correct tangent direction
      Vector3 tangent;
      Vector3 t1 = Vector3.Cross( towardsCenter, Vector3.forward );
      Vector3 t2 = Vector3.Cross( towardsCenter, Vector3.up );
      if (t1.magnitude > t2.magnitude) {
            tangent = t1;
      } else {
            tangent = t2;
      }
      // we get the new direction
      newDir = new Vector3(tangent.x + r * 2.0f, 0.0f, tangent.z + r * 2.0f);
      Vector3 translate = newDir * this.speed * Time.deltaTime;
      // then translate
      c.transform.Translate(translate, Space.World);
   }
   

   private IEnumerator Wait(float timeWait) {
      //this.moveCloser = false;
      yield return new WaitForSecondsRealtime(timeWait);
      //this.moveCloser = true;
   }

   private void setDisperseToTrue() {
      this.moveAway = true;
   }

   private void moveCrawler(GameObject c, Vector3 direction) {
      float r = Random.Range(-1.0f, 1.0f);
      Vector3 translate = (direction + new Vector3(r, 0.0f, r))* this.speed * Time.deltaTime;
      c.transform.Translate(translate, Space.World);
      // if further than the effective radius, then just delete
      if (direction.magnitude > this.radius && this.moveAway) {
            Destroy(c, 0.2f);
      }
   }
}