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
        InvokeRepeating("GenerateInCircle", 0.0f, 0.4f);
        Invoke("setDisperseToTrue", this.timeUntilDisperse);
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
    
// Things I could add
// -- random directions which are facing the user in general
// -- move alternating things at different times
// -- move things in groups
// -- swarming effect
    void Update () {  
        foreach (GameObject c in crawlers.ToArray()) {
			if (c.transform.position.y > center.transform.position.y * 3) {
				Destroy(c, 0.2f);
				this.crawlers.Remove(c);
			}
				

            if (this.disperse) {
                moveCrawlerAway(c);
            }
            else {
                moveCrawler(c);
            }
        }
        StartCoroutine(Wait(this.restTime));

		if(this.crawlers.Count == 0)
			FindObjectOfType<AudioManager> ().Pause("Creepy Crawlies");
    }

    private void moveCrawler(GameObject c) {
        float r = Random.Range(-1.0f, 1.0f);
		Vector3 towardsCenter = new Vector3(this.center.transform.position.x, 0, this.center.transform.position.z)- c.transform.position;
        if (towardsCenter.magnitude > closest && this.moveCloser) {
            Vector3 translate = (towardsCenter + new Vector3(r, 0.0f, r))* this.speed * Time.deltaTime;
            c.transform.Translate(translate);
        }
        else {
            this.moveCloser = false;
            Vector3 tangent;
            Vector3 t1 = Vector3.Cross( towardsCenter, Vector3.forward );
            Vector3 t2 = Vector3.Cross( towardsCenter, Vector3.up );
            if ( t1.magnitude > t2.magnitude ) {
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

    private void moveCrawlerAway(GameObject c) {
        float r = Random.Range(-1.0f, 1.0f);
		Vector3 awayFromCenter = c.transform.position - new Vector3(this.center.transform.position.x, 0, this.center.transform.position.z);
        Vector3 translate = (awayFromCenter + new Vector3(r, 0.0f, r))* this.speed * Time.deltaTime;
        c.transform.Translate(translate);
        print("move away");
		if (awayFromCenter.magnitude > (this.radius)) {
            Destroy(c, 0.2f);
            this.crawlers.Remove(c);
            print("destroy!!");
        }
    }
}