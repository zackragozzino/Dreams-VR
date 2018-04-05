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

	public void loadFirstScene(){
		StartCoroutine (LoadAsynchronously ());
	}

	public void loadNewScene(){
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
		//director.player = GameObject.FindGameObjectWithTag ("Player");
		director.mapGenerator = GameObject.FindGameObjectWithTag ("MapGenerator");
		director.mapGenerator.GetComponent<TerrainGenerator> ().viewer = director.getPlayer ().transform;
		director.getPlayer ().transform.position = new Vector3 (0, 1.55f, 0);
		//Start producing portals now that the scene is loaded
		director.startPortalGeneration ();
	}

	IEnumerator DestroyAndQueueScene(){
		director.stopPortalGeneration ();

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
		
}
