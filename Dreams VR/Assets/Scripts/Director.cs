using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;


public class Director : MonoBehaviour {

	public GameObject[] dreamScripts;
	public List<ColorCorrectionCurves> colorSchemes = new List<ColorCorrectionCurves>();
	public float doorSpawnRate = 10f; 
	public GameObject doorPortal;

	public int sceneNum = 0;

	public GameObject mapGenerator;
	private SceneLoader sceneLoader;

	private GameObject player;
	private GameObject playerCamera;
	public GameObject VR_Rig;
	public GameObject simulator_Rig;
	public GameObject VRCamera;
	public GameObject simulatorCamera;

	private VRTK.VRTK_SDKManager sdkManager;

	public AssetMaster.StarterEnvironment environment;
	public AssetMaster.SceneMod sceneMod;

	private int emotionSpectrum = 5;
	private int intensitySpectrum = 5;

	public Dropdown dropdown;

	public GameObject startScreen;

	private Scene currentScene;

	// Use this for initialization
	void Start () {

		sdkManager = VRTK.VRTK_SDKManager.instance;

		sceneLoader = this.GetComponent<SceneLoader> ();

		//Build list of different color pallets
		Transform colorParent = GameObject.Find ("ColorSchemes").transform;
		foreach (Transform child in colorParent) {
			colorSchemes.Add (child.GetComponent<ColorCorrectionCurves> ());
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.M)) {
			GenerateNewWorld();
			//StartCoroutine(sceneLoader.loadFinalArea());
		}

		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			intensitySpectrum = 0;
		}
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			intensitySpectrum = 1;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			intensitySpectrum = 2;
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			intensitySpectrum = 3;
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			intensitySpectrum = 4;
		}
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			intensitySpectrum = 5;
		}
		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			intensitySpectrum = 6;
		}
		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			intensitySpectrum = 7;
		}
		if (Input.GetKeyDown (KeyCode.Alpha8)) {
			intensitySpectrum = 8;
		}
		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			intensitySpectrum = 9;
		}
	
	}

	public GameObject getPlayer(){
		return player;
	}

	public void enableVR(){
		VRTK.VRTK_SDKSetup[] setups = sdkManager.setups;
		sdkManager.TryLoadSDKSetup (0, true, setups);
		player = VR_Rig;
		playerCamera = VRCamera;
		startScreen.SetActive (false);
		sceneLoader.loadFirstScene ();
	}

	public void enableSimulator(){
		getEnvironmentChoice ();
		VRTK.VRTK_SDKSetup[] setups = sdkManager.setups;
		sdkManager.TryLoadSDKSetup (1, true, setups);
		player = simulator_Rig;
		playerCamera = simulatorCamera;
		startScreen.SetActive (false);
		sceneLoader.loadFirstScene ();
	}

	public void getEnvironmentChoice(){
		switch (dropdown.value)
		{
		case 0:
			environment = AssetMaster.StarterEnvironment.forest;
			break;
		case 1:
			environment = AssetMaster.StarterEnvironment.palm;
			break;
		case 2:
			environment = AssetMaster.StarterEnvironment.furniture;
			break;
		case 3:
			environment = AssetMaster.StarterEnvironment.urban;
			break;
		case 4:
			environment = AssetMaster.StarterEnvironment.upsideDown;
			break;
		}
	}

	public int getIntensityLevel(){
		return intensitySpectrum;
	}

	public void startPortalGeneration(){
		StartCoroutine (GeneratePortal ());
	}

	public void stopPortalGeneration(){
		StopCoroutine (GeneratePortal ());
	}

	public void GenerateNewWorld(){
		//environment = (AssetMaster.StarterEnvironment)Random.Range (0, System.Enum.GetValues(typeof(AssetMaster.StarterEnvironment)).Length);
		//sceneMod = (AssetMaster.SceneMod)Random.Range (0, System.Enum.GetValues (typeof(AssetMaster.SceneMod)).Length);
		sceneNum++;
	
		//if (sceneNum > 0)
			//sceneMod = (AssetMaster.SceneMod)Random.Range (0,System.Enum.GetValues(typeof(AssetMaster.SceneMod)).Length);

		playerCamera.GetComponent<ColorController> ().addColorScheme (colorSchemes [Random.Range(0, colorSchemes.Count)]);

		/*switch (sceneNum) {
		case 1:
			sceneMod = AssetMaster.SceneMod.bounce;
			environment = AssetMaster.StarterEnvironment.furniture;
			//environment = AssetMaster.StarterEnvironment.empty;

			//environment = AssetMaster.StarterEnvironment.empty;
			break;
		
		case 2:
			sceneMod = AssetMaster.SceneMod.rotater;
			environment = AssetMaster.StarterEnvironment.forest;
			break;
		case 3:
			sceneMod = AssetMaster.SceneMod.none;
			environment = AssetMaster.StarterEnvironment.upsideDown;
			break;

		}
			

		if (sceneNum == 4) {
			StartCoroutine (sceneLoader.loadFinalArea ());
		} else {
			Debug.Log ("Scene mod: " + sceneMod);
			sceneLoader.loadNewScene ();
		}
		*/

		sceneLoader.loadNewScene ();
	}

	//This code is kind of basic and should be updated to reflect player movement, not time elapsed
	IEnumerator GeneratePortal(){
		yield return new WaitForSeconds(doorSpawnRate);

		//Random ranges between -80 to 80 but not within 50 units of the player
		 float xPos = player.transform.position.x + (Random.Range (50, 80) * ((Random.Range (0, 2) == 0) ? 1 : -1));
		 float zPos = player.transform.position.z + (Random.Range (50, 80) * ((Random.Range (0, 2) == 0) ? 1 : -1));

     	//float xPos = player.transform.position.x + (Random.Range (5, 8) * ((Random.Range (0, 2) == 0) ? 1 : -1));
		//float zPos = player.transform.position.z + (Random.Range (5, 8) * ((Random.Range (0, 2) == 0) ? 1 : -1));

		Vector3 doorPos = new Vector3 (xPos, doorPortal.transform.position.y + 10, zPos);
		GameObject spawnedDoor = Instantiate (doorPortal, doorPos, doorPortal.transform.rotation, mapGenerator.transform);
		spawnedDoor.AddComponent<RaycastGrounder> ();
		spawnedDoor.transform.LookAt (player.transform.position);
		//spawnedDoor.transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

		Debug.Log ("Door spawned: " + doorPos);
		StartCoroutine (GeneratePortal ());
	}

	public void spawnInitialDoor(){
		Vector3 pos = doorPortal.transform.position;
		pos.z = 21.3f;

		GameObject spawnedDoor = Instantiate (doorPortal, pos, doorPortal.transform.rotation, mapGenerator.transform);
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
