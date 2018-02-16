using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CreepyCrawlers : MonoBehaviour {

    public GameObject crawler;
    public GameObject center;
    public int numStartCrawlers = 0;
    public int maxCrawlers = 10;
    public float radius = 25.0f;
    public float speed = 1.0f;
    public float restTime = 3;
    private List<GameObject> crawlers;
    private float closest = 1;
    private bool move = true;

    // Use this for initialization
    void Start () {
        Quaternion zeroQuaternion = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        float crawlerAngle = 0;
        float angleIncrement = 2 * Mathf.PI / this.maxCrawlers;
        for (int i = 0; i < this.maxCrawlers; i++) {
            float x = Mathf.Sin(crawlerAngle) * this.radius;
            float z = Mathf.Cos(crawlerAngle) * this.radius;
            Vector3 newPos = this.center.transform.position + new Vector3(x, 0.0f, z);
            print("Instantiate");
            print(i);
            print(newPos);
            crawlers.Add((GameObject)Instantiate(this.crawler, newPos, zeroQuaternion));
            crawlerAngle += angleIncrement;
        }
        StartCoroutine(Wait(5.0f));
    }
    
    // Update is called once per frame
    void Update () {   
        if (this.move == true) {
            foreach (GameObject c in crawlers) {
                Vector3 diffVector = this.center.transform.position - c.transform.position;
                if (diffVector.magnitude >= closest) {
                    print(diffVector.magnitude);
                    print("Closest " + closest);
                    print(diffVector);
                    Vector3 translate = diffVector * this.speed * Time.deltaTime;
                    c.transform.Translate(translate);
                    print("Hi there!");
                    print(c.transform.position);
                }
            }
            StartCoroutine(Wait(3.0f));
        }
    }

    private IEnumerator Wait(float timeWait) {
        this.move = false;
        yield return new WaitForSecondsRealtime(timeWait);
        this.move = true;
    }
}