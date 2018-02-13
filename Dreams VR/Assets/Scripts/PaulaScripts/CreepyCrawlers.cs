using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CreepyCrawlers : MonoBehaviour {

    public GameObject crawler;

    // Use this for initialization
    void Start () {
        Instantiate(crawler, crawler.transform,instantiateInWorldSpace:true);
    }
    
    // Update is called once per frame
    void Update () {
    }
}