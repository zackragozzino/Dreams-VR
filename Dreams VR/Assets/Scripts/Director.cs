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

	private GameObject player;
	private GameObject mapGenerator;

	private Scene currentScene; 

	// Use this for initialization
	void Start () {
		timer = Random.Range (timerMin, timerMax);

		SceneManager.sceneLoaded += OnSceneLoaded;

		StartCoroutine (LoadAsynchronously ());
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
		}

		if (Input.GetKeyDown (KeyCode.K)) {
			//StartCoroutine (LoadAsynchronously ());
			StartCoroutine(DestroyAndQueueScene());

		}
		
	}

	public void GenerateNewWorld(){
		StartCoroutine(DestroyAndQueueScene());
	}

	IEnumerator LoadAsynchronously(){
		AsyncOperation operation = SceneManager.LoadSceneAsync ("Flat_Land", LoadSceneMode.Additive);
		while (!operation.isDone) {
			float progress = Mathf.Clamp01 (operation.progress / .9f);
			Debug.Log ("Loading ... " + progress * 100f + "%");
			yield return null;
		}
		//Reset player and map generator  reference
		player = GameObject.FindGameObjectWithTag ("Player");
		mapGenerator = GameObject.FindGameObjectWithTag ("MapGenerator");
		//Start producing portals now that the scene is loaded
		StartCoroutine (GeneratePortal ());
	}

	IEnumerator DestroyAndQueueScene(){
		StopCoroutine (GeneratePortal ());

		AsyncOperation operation = SceneManager.UnloadSceneAsync (currentScene);
		while (!operation.isDone) {
			yield return null;
		}
		StartCoroutine (LoadAsynchronously ());
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode){
		Debug.Log ("Scene loaded: " + scene.name);
		currentScene = scene;
		//SceneManager.LoadScene -= OnSceneLoaded;
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
