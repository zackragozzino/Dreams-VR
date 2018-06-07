using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;


public class Director : MonoBehaviour {

	public GameObject[] dreamScripts;
	public List<ColorCorrectionCurves> colorSchemes_Pos = new List<ColorCorrectionCurves>();
	public List<ColorCorrectionCurves> colorSchemes_Neg = new List<ColorCorrectionCurves>();
	public float doorSpawnRate = 5f; 
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

   public RawImage startScreenBackground;
   public RawImage startScreenLogo;
	public GameObject startScreenButtons;
	public GameObject startScreenCamera;

	private Scene currentScene;
	private IEnumerator currentPortalCoroutine;

	private float timeInSeconds = 300f;

	public NickWeatherManager nickWeatherManager;
	private AudioManager audm;

	public GameObject bearSweetSpot;
	private float bearTimerLength = 45f;
	private float bearTimer;

	private bool isVR;

	// Use this for initialization
	void Start () {
		sdkManager = VRTK.VRTK_SDKManager.instance;

		sceneLoader = this.GetComponent<SceneLoader> ();

		audm = FindObjectOfType<AudioManager>();

		//Build list of different color palettes
		foreach (Transform child in GameObject.Find ("ColorSchemes_Pos").transform) {
			colorSchemes_Pos.Add (child.GetComponent<ColorCorrectionCurves> ());
		}

		foreach (Transform child in GameObject.Find ("ColorSchemes_Neg").transform) {
			colorSchemes_Neg.Add (child.GetComponent<ColorCorrectionCurves> ());
		}

      //StartCoroutine(Wait(0.5f));
      FadeImage(startScreenLogo, false);

		bearTimer = bearTimerLength;
	}
	
	// Update is called once per frame
	void Update () {
		if(sceneNum > 0)
			bearTimer -= Time.deltaTime;

		if (bearTimer <= 0)
			generateBearTrap ();

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

	public float getTimeLeft(){
		return timeInSeconds;
	}

	IEnumerator dreamTimer(){
		while (timeInSeconds >= 0) {
			timeInSeconds -= Time.deltaTime;
			yield return null;
		}
		//yield return new WaitForSeconds (timeInSeconds);
		audm.Play("Teleport");
		StartCoroutine (sceneLoader.loadFinalArea ());
	}

	public GameObject getPlayer(){
		return player;
	}

	public void enableVR(){
		VRTK.VRTK_SDKSetup[] setups = sdkManager.setups;
		sdkManager.TryLoadSDKSetup (0, true, setups);
		player = VR_Rig;

		playerCamera = VRCamera;
		//startScreen.SetActive (false);

		isVR = true;

		startScreenButtons.SetActive (false);
		startScreenCamera.SetActive (false);

		sceneLoader.loadFirstScene ();

		StartCoroutine (dreamTimer ());
	}

	public void enableSimulator(){
		getEnvironmentChoice ();
		VRTK.VRTK_SDKSetup[] setups = sdkManager.setups;
		sdkManager.TryLoadSDKSetup (1, true, setups);
		player = simulator_Rig;

		playerCamera = simulatorCamera;
		//startScreen.SetActive (false);

		isVR = false;

		startScreenButtons.SetActive (false);
		startScreenCamera.SetActive (false);

		sceneLoader.loadFirstScene ();

		StartCoroutine(dreamTimer ());
	}

	public bool getVRStatus(){
		return isVR;
	}

   // This is triggered by the onClick on the Facebook Login button on the startscreen
   // It calls the FB login function from the FacebookLogin Singleton object
   // Then it waits so the Facebook user information can populate the Singleton object
   // Sign in using the test user access token: 
   // EAAIAEhtLLU4BADBGHfCK8mZC2w8oZBxwr2p6zb7ZBeAJTx3o3kdWRudoZBukdXMZBYQaRr3woEeB0WUj1NVY7Jn3vZCEVwD07xZARIaSvHYW5ACiXkEVPzwfz6Vcb3E1ZAretVckI7kyydejJ7ey9hMmwEwBQlhVPP7nlHzyWZCZAkLW76QAqx5Tl54K9OFsiVB74OQkgQAnPqnOb0aSi42V8cIuasGfVJsZCpcCNNqm6hGmvWLNvhE75ve
   public void enableFacebookLogin() {
      startScreenButtons.SetActive(false);
      FacebookLoginHybriona.Instance.Login();
      StartCoroutine(waitForFacebook(10));
   }


   IEnumerator waitForFacebook(int seconds) {
      yield return new WaitForSeconds(seconds);
      startScreenButtons.SetActive(true);
      Debug.Log("Hi, " + FacebookLoginHybriona.Instance.firstName);
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

	public int getEmotionLevel(){
		return emotionSpectrum;
	}

	public void startPortalGeneration(){
		currentPortalCoroutine = GeneratePortal ();
		StartCoroutine (currentPortalCoroutine);
	}

	public void stopPortalGeneration(){
		//StopCoroutine (currentPortalCoroutine);
	}

	public void GenerateNewWorld(){

		audm.Play ("Teleport");
		bearTimer = bearTimerLength;

		AssetMaster.StarterEnvironment newEnvironment = environment;

		//Ensure the new environment isn't the same as the current environment
		while (newEnvironment == environment) {
			newEnvironment = (AssetMaster.StarterEnvironment)Random.Range (0, System.Enum.GetValues(typeof(AssetMaster.StarterEnvironment)).Length);
		}
		environment = newEnvironment;


		//sceneMod = (AssetMaster.SceneMod)Random.Range (0, System.Enum.GetValues (typeof(AssetMaster.SceneMod)).Length);
		sceneNum++;
	
		//if (sceneNum > 0)
			//sceneMod = (AssetMaster.SceneMod)Random.Range (0,System.Enum.GetValues(typeof(AssetMaster.SceneMod)).Length);

		//Apply a random change to the 2 spectrums
		//Clamp the values so they can't go past 0 and 10
		emotionSpectrum = (int)Mathf.Clamp(emotionSpectrum + Random.Range (-3, 3), 0f, 10f);
		intensitySpectrum = (int)Mathf.Clamp(intensitySpectrum + Random.Range (-3, 3), 0f, 10f);


		//Tying weather to basic emotion spectrums
		if (emotionSpectrum < 5) {
			playerCamera.GetComponent<ColorController> ().addColorScheme (colorSchemes_Neg [Random.Range (0, colorSchemes_Neg.Count)]);
			nickWeatherManager.profile = NickWeatherManager.WeatherProfile.HeavyRain;
		} else if (emotionSpectrum > 5) {
			playerCamera.GetComponent<ColorController> ().addColorScheme (colorSchemes_Pos [Random.Range (0, colorSchemes_Pos.Count)]);
			nickWeatherManager.profile = NickWeatherManager.WeatherProfile.Sunny;
		}
		else
			nickWeatherManager.profile = NickWeatherManager.WeatherProfile.none;
			

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
		float xPos = player.transform.position.x + (Random.Range (15, 40) * ((Random.Range (0, 2) == 0) ? 1 : -1));
		float zPos = player.transform.position.z + (Random.Range (15, 40) * ((Random.Range (0, 2) == 0) ? 1 : -1));


		Vector3 doorPos = new Vector3 (xPos, doorPortal.transform.position.y + 10, zPos);
		GameObject spawnedDoor = Instantiate (doorPortal, doorPos, doorPortal.transform.rotation, mapGenerator.transform);
		spawnedDoor.AddComponent<RaycastGrounder> ();
		spawnedDoor.transform.LookAt (player.transform.position);
		//spawnedDoor.transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
		currentPortalCoroutine = GeneratePortal ();
		Debug.Log ("Door spawned: " + doorPos);

		StartCoroutine (currentPortalCoroutine);
	}

	void generateBearTrap(){

		Debug.Log ("Generating bear trap...");

		float xPos = player.transform.position.x + (Random.Range (30, 60) * ((Random.Range (0, 2) == 0) ? 1 : -1));
		float zPos = player.transform.position.z + (Random.Range (30, 60) * ((Random.Range (0, 2) == 0) ? 1 : -1));

		GameObject bearTrap = Instantiate (bearSweetSpot, new Vector3(xPos, 0, zPos), bearSweetSpot.transform.rotation, mapGenerator.transform);

		bearTimer = bearTimerLength;
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

   IEnumerator Wait(float seconds) {
      yield return new WaitForSeconds(seconds);
   }

   IEnumerator FadeImage(RawImage img, bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }
}
