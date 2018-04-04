using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour {

	public GameObject[] dreamScripts;
	public float doorSpawnRate = 10f; 
	public GameObject doorPortal;

	private float timer;
	private int timerMin = 5;
	private int timerMax = 30;

	public GameObject player;
	public GameObject mapGenerator;
	private SceneLoader sceneLoader;

	private Scene currentScene;

	// Use this for initialization
	void Start () {
		timer = Random.Range (timerMin, timerMax);

		sceneLoader = this.GetComponent<SceneLoader> ();
	}
	
	// Update is called once per frame
	void Update () {

		/*timer -= Time.deltaTime;

		if (timer <= 0) {
			AddScript ();
			timer = Random.Range (timerMin, timerMax);
		}*/


		//Instantiates the crawler script. Used for testing purposes
		/*if (Input.GetKeyDown (KeyCode.G)) {
			Instantiate (dreamScripts [0], player.transform.position, Quaternion.identity, this.transform);
		}*/
		
	}

	public void startPortalGeneration(){
		StartCoroutine (GeneratePortal ());
	}

	public void stopPortalGeneration(){
		StopCoroutine (GeneratePortal ());
	}

	public void GenerateNewWorld(){
		sceneLoader.loadNewScene ();
	}

	//This code is kind of basic and should be updated to reflect player movement, not time elapsed
	IEnumerator GeneratePortal(){
		yield return new WaitForSeconds(doorSpawnRate);

		//Random ranges between -80 to 80 but not within 50 units of the player
		float xPos = player.transform.position.x + (Random.Range (50, 80) * ((Random.Range (0, 2) == 0) ? 1 : -1));
		float zPos = player.transform.position.z + (Random.Range (50, 80) * ((Random.Range (0, 2) == 0) ? 1 : -1));

		Vector3 doorPos = new Vector3 (xPos, doorPortal.transform.position.y, zPos);
		GameObject spawnedDoor = Instantiate (doorPortal, doorPos, doorPortal.transform.rotation, mapGenerator.transform);
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
