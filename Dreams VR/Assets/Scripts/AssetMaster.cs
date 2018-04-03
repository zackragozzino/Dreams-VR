using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetMaster : MonoBehaviour {

	public Object_Populator ObjectPopulator;
	public bool generateAssets;
	public enum StarterEnvironment {forest, urban, furniture, palm};
	public StarterEnvironment starterEnvironment;

	public GameObject[] forestAssets;
	public GameObject[] urbanAssets;
	public GameObject[] furnitureAssets;
	public GameObject[] palmTreeAssets;

	private GameObject[] starterEnvironmentAssets;

	void Start(){
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

	public void generateObject(int x, int y, int width, int height, Transform parent){
		GameObject asset = starterEnvironmentAssets [Random.Range (0, starterEnvironmentAssets.Length)];
		asset = Instantiate (asset, new Vector3 (parent.position.x + x - (width/2f), parent.position.y, parent.position.z + y - (height/2f)), asset.transform.rotation, parent);
		asset.tag = "EnvironmentObject";

		if (starterEnvironment == StarterEnvironment.forest) {
			asset.transform.localScale = asset.transform.localScale * (10 * Random.value + 4);
		} else if (starterEnvironment == StarterEnvironment.urban) {
			asset.transform.localScale = asset.transform.localScale * 0.5f;
		} else if (starterEnvironment == StarterEnvironment.furniture) {
			asset.transform.localScale = asset.transform.localScale * 1.5f;
			asset.AddComponent<BoxCollider>();
		} else if (starterEnvironment == StarterEnvironment.palm) {
			asset.transform.localScale = asset.transform.localScale * 4f;		
		}
	}
}
