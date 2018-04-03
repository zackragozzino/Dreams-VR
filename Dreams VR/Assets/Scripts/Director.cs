using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour {

	public GameObject[] dreamScripts;
	public float doorSpawnRate = 10f; 
	public GameObject doorPortal;

	private float timer;
	private int timerMin = 5;
	private int timerMax = 30;

	private GameObject player;

	// Use this for initialization
	void Start () {
		timer = Random.Range (timerMin, timerMax);
		player = GameObject.FindGameObjectWithTag ("Player");
		StartCoroutine (GeneratePortal ());
	}
	
	// Update is called once per frame
	void Update () {

		/*timer -= Time.deltaTime;

		if (timer <= 0) {
			AddScript ();
			timer = Random.Range (timerMin, timerMax);
		}*/

		if (Input.GetKeyDown (KeyCode.G)) {
			Instantiate (dreamScripts [0], player.transform.position, Quaternion.identity, this.transform);
			Debug.Log ("test");
		}
		
	}

	//This code is kind of basic and should be updated to reflect player movement, not time elapsed
	IEnumerator GeneratePortal(){
		yield return new WaitForSeconds(doorSpawnRate);

		Vector3 doorPos = new Vector3 (player.transform.position.x + Random.Range (50, 80), doorPortal.transform.position.y, player.transform.position.z + Random.Range (50, 80));
		GameObject spawnedDoor = Instantiate (doorPortal, doorPos, doorPortal.transform.rotation, this.transform);
		spawnedDoor.transform.LookAt (player.transform.position);
		spawnedDoor.transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

		//Debug.Log ("Door spawned: " + doorPos);
		StartCoroutine (GeneratePortal ());
	}
		

	void AddScript(){
		GameObject dreamScript = Instantiate (dreamScripts [Random.Range (0, dreamScripts.Length)], this.transform.position, Quaternion.identity, this.transform);
		float waitTime = Random.Range (5, 10);
		StartCoroutine (WaitAndKillGameObject(dreamScript, waitTime));
	}

	IEnumerator WaitAndKillGameObject(GameObject gameObject, float waitTime){
		yield return new WaitForSeconds (waitTime);
		Destroy (gameObject);
	}
}
