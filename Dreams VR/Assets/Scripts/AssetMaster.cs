using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetMaster : MonoBehaviour {

	public Object_Populator ObjectPopulator;
	public bool generateAssets;
	public enum StarterEnvironment {forest, urban, furniture, palm, upsideDown};
	public enum SceneMod {none, rotater, bounce, magnet, implode};
	public StarterEnvironment starterEnvironment;
	public SceneMod sceneMod;

	public GameObject[] forestAssets;
	public GameObject[] urbanAssets;
	public GameObject[] furnitureAssets;
	public GameObject[] palmTreeAssets;
	public GameObject[] subProps;

	public GameObject grass;

	public GameObject startingRoom;

	private GameObject[] starterEnvironmentAssets;

	private Director director;

	void Start(){
		director = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Director> ();

		if (director != null)
			starterEnvironment = director.environment;

		setStarterAssets ();

		//sceneMod = SceneMod.bounce;
		sceneMod = director.sceneMod;
	
		if (director.sceneNum == 0)
			generateStartingRoom ();
	}

	void setStarterAssets(){
		switch (starterEnvironment) {
		case StarterEnvironment.forest:
			starterEnvironmentAssets = forestAssets;
			break;
		case StarterEnvironment.furniture:
			starterEnvironmentAssets = furnitureAssets;
			break;
		case StarterEnvironment.palm:
			starterEnvironmentAssets = palmTreeAssets;
			break;
		case StarterEnvironment.urban:
			starterEnvironmentAssets = urbanAssets;
			break;
		case StarterEnvironment.upsideDown:
			starterEnvironmentAssets = urbanAssets;
			break;
		}
	}

	void setSceneMod(GameObject asset){
		switch (sceneMod) {
		case SceneMod.rotater:
			Vector3 dir = new Vector3 (Random.Range (-5, 5), Random.Range (-5, 5), Random.Range (-5, 5));
			asset.AddComponent<Rotater> ().RotationPerSecond = dir;
			break;
		case SceneMod.magnet:
			asset.AddComponent<Magnetism> ().player = director.getPlayer ();
			break;
		case SceneMod.bounce:
			Vector3 pos = asset.transform.position;
			asset.transform.position = new Vector3 (pos.x, Random.Range (1, 100), pos.z);
			asset.AddComponent<Bounce> ().bounciness = 1;

			break;
		case SceneMod.implode:
			asset.AddComponent<Implode> ();
			break;
		}
	}

	void generateStartingRoom(){
		Instantiate (startingRoom, startingRoom.transform.position, startingRoom.transform.rotation, this.transform);
	}

	public void generateObject(int x, int y, int width, int height, Transform parent, float noiseVal, float heightVal){
		
		if (starterEnvironment == StarterEnvironment.forest) {
			/*if (noiseVal > 0.7f) {
				GameObject asset = starterEnvironmentAssets [Random.Range (0, starterEnvironmentAssets.Length)];
				asset = Instantiate (asset, new Vector3 (parent.position.x + x - (width / 2f), parent.position.y+ heightVal, parent.position.z + y - (height / 2f)), asset.transform.rotation, parent);
				asset.tag = "EnvironmentObject";
				asset.transform.localScale = asset.transform.localScale * (5 * Random.value + 4);

				setSceneMod (asset);
			
				//asset.transform.position = new Vector3(asset.transform.position.x, Random.Range (5, 15), asset.transform.position.z);
				//asset.transform.eulerAngles = new Vector3 (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));
			}
			*/
			/*
			if (noiseVal < 0) {
				GameObject asset2 = urbanAssets [Random.Range (0, urbanAssets.Length)];
				asset2 = Instantiate (asset2, new Vector3 (parent.position.x + x - (width/2f), parent.position.y, parent.position.z + y - (height/2f)), asset2.transform.rotation, parent);
				asset2.tag = "EnvironmentObject";
			}
			*/

			if (noiseVal < 0.03) {
				GameObject asset2 = Instantiate (grass, new Vector3 (parent.position.x + x - (width/2f), parent.position.y + heightVal + 10, parent.position.z + y - (height/2f)), grass.transform.rotation, parent);
				asset2.AddComponent<RaycastGrounder> ();
			}


			//Vector3 dir = new Vector3 (Random.Range(-5,5), Random.Range(-5,5), Random.Range(-5,5));
			//asset.AddComponent<Rotater> ().RotationPerSecond = dir;
		} 

		else if (starterEnvironment == StarterEnvironment.urban && noiseVal > 0.7f) {
			GameObject asset = starterEnvironmentAssets [Random.Range (0, starterEnvironmentAssets.Length)];
			asset = Instantiate (asset, new Vector3 (parent.position.x + x - (width/2f), parent.position.y, parent.position.z + y - (height/2f)), asset.transform.rotation, parent);
			asset.tag = "EnvironmentObject";
			asset.transform.localScale = asset.transform.localScale * 0.5f;

			setSceneMod (asset);
		} 

		else if (starterEnvironment == StarterEnvironment.furniture && noiseVal > 0.7f) {
			GameObject asset = starterEnvironmentAssets [Random.Range (0, starterEnvironmentAssets.Length)];
			asset = Instantiate (asset, new Vector3 (parent.position.x + x - (width/2f), parent.position.y, parent.position.z + y - (height/2f)), asset.transform.rotation, parent);
			asset.tag = "EnvironmentObject";
			asset.transform.localScale = asset.transform.localScale * 1.5f;
			asset.AddComponent<BoxCollider> ();

			setSceneMod (asset);
		} 

		else if (starterEnvironment == StarterEnvironment.palm && noiseVal > 0.7f) {
			GameObject asset = starterEnvironmentAssets [Random.Range (0, starterEnvironmentAssets.Length)];
			asset = Instantiate (asset, new Vector3 (parent.position.x + x - (width/2f), parent.position.y, parent.position.z + y - (height/2f)), asset.transform.rotation, parent);
			asset.tag = "EnvironmentObject";
			asset.transform.localScale = asset.transform.localScale * 4f;

			setSceneMod (asset);
		} 

		else if (starterEnvironment == StarterEnvironment.upsideDown && noiseVal < 0.01f) {
			GameObject asset = urbanAssets [Random.Range (0, urbanAssets.Length)];
			asset = Instantiate (asset, new Vector3 (parent.position.x + x - (width/2f), Random.Range(20,100), parent.position.z + y - (height/2f)), asset.transform.rotation, parent);
			//asset.tag = "EnvironmentObject";
			asset.transform.eulerAngles = new Vector3 (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));

			setSceneMod (asset);
		}
			

		/*if (noiseVal < 0.0) {
			GameObject asset2 = subProps [Random.Range (0, subProps.Length)];
			asset2 = Instantiate (asset2, new Vector3 (parent.position.x + x - (width/2f), parent.position.y, parent.position.z + y - (height/2f)), asset2.transform.rotation, parent);
			asset2.tag = "EnvironmentObject";
		}*/

	}
}
