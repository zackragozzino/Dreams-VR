using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetMaster : MonoBehaviour {

	public Object_Populator ObjectPopulator;
	public bool generateAssets;
	public enum StarterEnvironment {forest, urban, furniture, palm, upsideDown};
	public StarterEnvironment starterEnvironment;

	public GameObject[] forestAssets;
	public GameObject[] urbanAssets;
	public GameObject[] furnitureAssets;
	public GameObject[] palmTreeAssets;

	private GameObject[] starterEnvironmentAssets;

	void Start(){
		Director director = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Director> ();

		if (director != null)
			starterEnvironment = director.environment;

		setStarterAssets ();
	}

	void setStarterAssets(){
		if (starterEnvironment == StarterEnvironment.forest)
			starterEnvironmentAssets = forestAssets;
		else if (starterEnvironment == StarterEnvironment.urban)
			starterEnvironmentAssets = urbanAssets;
		else if (starterEnvironment == StarterEnvironment.furniture)
			starterEnvironmentAssets = furnitureAssets;
		else if (starterEnvironment == StarterEnvironment.palm)
			starterEnvironmentAssets = palmTreeAssets;
		else {
			Debug.Log ("Incorrect starter environment");
		}
	}

	public void generateObject(int x, int y, int width, int height, Transform parent, float noiseVal){

		if (starterEnvironment == StarterEnvironment.forest) {
			if (noiseVal > 0.7f) {
				GameObject asset = starterEnvironmentAssets [Random.Range (0, starterEnvironmentAssets.Length)];
				asset = Instantiate (asset, new Vector3 (parent.position.x + x - (width / 2f), parent.position.y, parent.position.z + y - (height / 2f)), asset.transform.rotation, parent);
				asset.tag = "EnvironmentObject";
				asset.transform.localScale = asset.transform.localScale * (5 * Random.value + 4);
			}

			/*
			if (noiseVal < 0) {
				GameObject asset2 = urbanAssets [Random.Range (0, urbanAssets.Length)];
				asset2 = Instantiate (asset2, new Vector3 (parent.position.x + x - (width/2f), parent.position.y, parent.position.z + y - (height/2f)), asset2.transform.rotation, parent);
				asset2.tag = "EnvironmentObject";
			}
			*/


			//Vector3 dir = new Vector3 (Random.Range(-5,5), Random.Range(-5,5), Random.Range(-5,5));
			//asset.AddComponent<Rotater> ().RotationPerSecond = dir;
		} 

		else if (starterEnvironment == StarterEnvironment.urban && noiseVal > 0.7f) {
			GameObject asset = starterEnvironmentAssets [Random.Range (0, starterEnvironmentAssets.Length)];
			asset = Instantiate (asset, new Vector3 (parent.position.x + x - (width/2f), parent.position.y, parent.position.z + y - (height/2f)), asset.transform.rotation, parent);
			asset.tag = "EnvironmentObject";
			asset.transform.localScale = asset.transform.localScale * 0.5f;
		} 

		else if (starterEnvironment == StarterEnvironment.furniture && noiseVal > 0.7f) {
			GameObject asset = starterEnvironmentAssets [Random.Range (0, starterEnvironmentAssets.Length)];
			asset = Instantiate (asset, new Vector3 (parent.position.x + x - (width/2f), parent.position.y, parent.position.z + y - (height/2f)), asset.transform.rotation, parent);
			asset.tag = "EnvironmentObject";
			asset.transform.localScale = asset.transform.localScale * 1.5f;
			asset.AddComponent<BoxCollider> ();
		} 

		else if (starterEnvironment == StarterEnvironment.palm && noiseVal > 0.7f) {
			GameObject asset = starterEnvironmentAssets [Random.Range (0, starterEnvironmentAssets.Length)];
			asset = Instantiate (asset, new Vector3 (parent.position.x + x - (width/2f), parent.position.y, parent.position.z + y - (height/2f)), asset.transform.rotation, parent);
			asset.tag = "EnvironmentObject";
			asset.transform.localScale = asset.transform.localScale * 4f;		
		} 

		else if (starterEnvironment == StarterEnvironment.upsideDown && noiseVal < 0.01f) {
			GameObject asset = urbanAssets [Random.Range (0, urbanAssets.Length)];
			asset = Instantiate (asset, new Vector3 (parent.position.x + x - (width/2f), parent.position.y, parent.position.z + y - (height/2f)), asset.transform.rotation, parent);
			//asset.tag = "EnvironmentObject";
			asset.transform.eulerAngles = new Vector3 (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));
		}

	}
}
