using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	private Scene currentScene;
	private Director director;

	// Use this for initialization
	void Start () {
		director = this.GetComponent<Director> ();

		SceneManager.sceneLoaded += OnSceneLoaded;

		//StartCoroutine (LoadAsynchronously ());
	}

	void Update(){

	}

	public void loadSceneButton(){
		director.getEnvironmentChoice ();
		StartCoroutine(DestroyAndQueueScene ());
	}

	public void loadFirstScene(){
		StartCoroutine (LoadAsynchronously ());
	}

	public void loadNewScene(){
		StartCoroutine(DestroyAndQueueScene());
	}

	public IEnumerator loadFinalArea(){
		director.stopPortalGeneration ();

		AsyncOperation operation = SceneManager.UnloadSceneAsync (currentScene);
		while (!operation.isDone) {
			yield return null;
		}

		director.getPlayer ().GetComponent<Rigidbody> ().useGravity = false;

		SteamVR_LoadLevel.Begin ("Final_Area");
		while (SteamVR_LoadLevel.loading) {
			yield return null;
		}

		director.getPlayer ().GetComponent<Rigidbody> ().useGravity = true;

		director.getPlayer ().transform.position = new Vector3 (0, 0.01f, 0);
	}

	IEnumerator LoadUsingSteamVR(){
		director.getPlayer ().GetComponent<Rigidbody> ().useGravity = false;

		if (director.getEmotionLevel () > 3)
			SteamVR_LoadLevel.Begin ("Flat_Land");
		else
			SteamVR_LoadLevel.Begin ("BlackSpace");
		
		while (SteamVR_LoadLevel.loading) {
			yield return null;
		}

		director.getPlayer ().GetComponent<Rigidbody> ().useGravity = true;

		//Reset player and map generator  reference
		//director.player = GameObject.FindGameObjectWithTag ("Player");
	
		if (currentScene.name == "Flat_Land") {
			director.mapGenerator = GameObject.FindGameObjectWithTag ("MapGenerator");
			director.mapGenerator.GetComponent<TerrainGenerator> ().viewer = director.getPlayer ().transform;
		}

		director.getPlayer ().transform.position = new Vector3 (0, 0.01f, 0);
		//Start producing portals now that the scene is loaded
		director.startPortalGeneration ();



		//director.mapGenerator.AddComponent<Magnetism> ();
	}

	IEnumerator LoadAsynchronously(){
		AsyncOperation operation;
		if(director.getEmotionLevel() > 3)
			operation = SceneManager.LoadSceneAsync ("Flat_Land", LoadSceneMode.Additive);
		else
			operation = SceneManager.LoadSceneAsync ("BlackSpace", LoadSceneMode.Additive);
		while (!operation.isDone) {
			float progress = Mathf.Clamp01 (operation.progress / .9f);
			Debug.Log ("Loading ... " + progress * 100f + "%");
			yield return null;
		}
		//Reset player and map generator  reference
		//director.player = GameObject.FindGameObjectWithTag ("Player");

		director.mapGenerator = GameObject.FindGameObjectWithTag ("MapGenerator");
		director.mapGenerator.GetComponent<TerrainGenerator> ().viewer = director.getPlayer ().transform;

		//director.getPlayer ().transform.position = new Vector3 (0, 3f, 0);
		director.getPlayer ().transform.position = new Vector3 (0.5f, 2.1f, -1.9f);

		//Start producing portals now that the scene is loaded
		director.startPortalGeneration ();

		//if (director.sceneNum == 0)
			//director.spawnInitialDoor ();
	}

	IEnumerator DestroyAndQueueScene(){
		director.stopPortalGeneration ();

		AsyncOperation operation = SceneManager.UnloadSceneAsync (currentScene);
		while (!operation.isDone) {
			yield return null;
		}
		//StartCoroutine (LoadAsynchronously ());
		StartCoroutine (LoadUsingSteamVR ());
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode){
		Debug.Log ("Scene loaded: " + scene.name + "-" + director.environment);
		currentScene = scene;
		//SceneManager.LoadScene -= OnSceneLoaded;
	}
		
}
