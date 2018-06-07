using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public GameObject player;
    //public Vector3 offset;
    public float rotateSpeed = 2f;
    private Director director;

    // Use this for initialization
    void Start()
    {
        director = GameObject.Find("GameManager").GetComponent<Director>();
        player = director.getPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDir = player.transform.position - transform.position;
        float step = rotateSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
