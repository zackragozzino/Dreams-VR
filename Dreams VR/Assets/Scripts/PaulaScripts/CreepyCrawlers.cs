using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CreepyCrawlers : MonoBehaviour {

    public GameObject crawler;
    public GameObject center;
    public int numStartCrawlers;
    public int maxCrawlers;
    public float radius;
    public float speed;
    public List<GameObject> crawlers;

    public CreepyCrawlers () {
        this.numStartCrawlers = 0;
        this.maxCrawlers = 10;
        this.radius = 10.0f; 
        this.speed = 0.4f;
    }

    // Use this for initialization
    void Start () {

        for (int i = 0; i < this.maxCrawlers; i++) {
            print("Instantiate!");
            crawlers.Add((GameObject)Instantiate(this.crawler, this.crawler.transform,instantiateInWorldSpace:true));
        }
    }
    
    // Update is called once per frame
    void Update () {      
        // foreach (GameObject c in crawlers) {
        //     Vector3 newDir = this.center.transform.position - c.transform.position;
        //     if (newDir.x > 3 && newDir.y > 3) {
        //         c.transform.Translate(newDir * this.speed);
        //     }
        // }
    }

    IEnumerator wait(int timeWait) {
        yield return new WaitForSeconds(timeWait);
    }
}