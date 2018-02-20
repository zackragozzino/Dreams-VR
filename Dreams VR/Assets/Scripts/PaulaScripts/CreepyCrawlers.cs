using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CreepyCrawlers : MonoBehaviour {

    public GameObject crawler;
    public GameObject center;
    public int numStartCrawlers = 0;
    public int maxCrawlers = 10;
    public float radius = 10.0f;
    public float speed = 1.0f;
    public float restTime = 0.5f;
    private List<GameObject> crawlers;
    private float closest = 1;
    private bool move;

    public CreepyCrawlers() {
        this.crawlers  = new List<GameObject>();
        this.move = true;
    }

    // Use this for initialization
    void Start () {
        Quaternion zeroQuaternion = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        float crawlerAngle = 0;
        float angleIncrement = 2 * Mathf.PI / this.maxCrawlers;
        for (int i = 0; i < this.maxCrawlers; i++) {
            float x = Mathf.Sin(crawlerAngle) * this.radius;
            float z = Mathf.Cos(crawlerAngle) * this.radius;
            Vector3 newPos = this.center.transform.position + new Vector3(x, -this.center.transform.position.y, z);
            GameObject crawlThing = (GameObject)Instantiate(this.crawler, newPos, zeroQuaternion);
            crawlers.Add(crawlThing);
            crawlerAngle += angleIncrement;
        }
    }
    
// Things I could add
// -- random directions which are facing the user in general
// -- move alternating things at different times
// -- move things in groups
// -- swarming effect
    void Update () {   
        if (this.move == true) {
            foreach (GameObject c in crawlers) {
                Vector3 diffVector = this.center.transform.position - c.transform.position;
                if (diffVector.magnitude >= closest) {
                    Vector3 translate = diffVector * this.speed * Time.deltaTime;
                    c.transform.Translate(translate);
                }
            }
            StartCoroutine(Wait(this.restTime));
        }
    }

    private IEnumerator Wait(float timeWait) {
        this.move = false;
        yield return new WaitForSecondsRealtime(timeWait);
        this.move = true;
    }
}