using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Populator : MonoBehaviour {

	public Renderer textureRender;

	public MeshSettings meshSettings;
	public HeightMapSettings heightMapSettings;
	public TextureData textureData;
	//public HeightMap meshHeightMap;
	public float[,] meshHeightMap;

	public GameObject testMesh;

	HeightMap heightMap;
	bool heightMapReceived;

	public Vector2 sampleCentre;

	public Object_Populator(){

	}

	void Start(){
		ThreadedDataRequester.RequestData(() => HeightMapGenerator.GenerateHeightMap (meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, sampleCentre), OnHeightMapReceived);
	}

	public void DrawMapInEditor() {
		HeightMap heightMap = HeightMapGenerator.GenerateHeightMap (meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, Vector2.zero);
		DrawTexture (TextureGenerator.TextureFromHeightMap (heightMap));
	}

	public void DrawTexture(Texture2D texture) {
		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height) /10f;

		textureRender.gameObject.SetActive (true);
	}

	void OnValuesUpdated() {
		if (!Application.isPlaying) {
			DrawMapInEditor ();
		}
	}
		
	void OnHeightMapReceived(object heightMapObject) {
		this.heightMap = (HeightMap)heightMapObject;
		heightMapReceived = true;

		Populate ();
	}

	void OnValidate() {

		if (meshSettings != null) {
			meshSettings.OnValuesUpdated -= OnValuesUpdated;
			meshSettings.OnValuesUpdated += OnValuesUpdated;
		}
		if (heightMapSettings != null) {
			heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
			heightMapSettings.OnValuesUpdated += OnValuesUpdated;
		}

	}

	public void Populate() {
		int width = heightMap.values.GetLength (0);
		int height = heightMap.values.GetLength (1);
		AssetMaster assetMaster = transform.GetComponentInParent<AssetMaster> ();
		
		GameObject[] urbanAssets = assetMaster.urbanAssets;

		if (heightMapReceived && assetMaster.generateAssets) {
			float[] noiseMap = new float[width * height];
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					/*if (heightMap.values [x, y] < 0.01f && assetMaster.starterEnvironment == AssetMaster.StarterEnvironment.upsideDown) {
						GameObject asset = Instantiate (assetMaster.urbanAssets[Random.Range(0, urbanAssets.Length)], new Vector3 (this.transform.position.x + x - (width/2f), Random.Range(5,15), this.transform.position.z + y - (height/2f)), Quaternion.identity, this.transform);
						asset.transform.eulerAngles = new Vector3 (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));
					}*/

					/*if (heightMap.values [x, y] > 0.7f && assetMaster.starterEnvironment != AssetMaster.StarterEnvironment.upsideDown) {
						//GameObject asset = Instantiate (assets[Random.Range(0, assets.Length)], new Vector3 (this.transform.position.x + x - (width/2f), 0, this.transform.position.z + y - (height/2f)), Quaternion.identity, this.transform);
						//GameObject asset = Instantiate (testMesh, new Vector3 (this.transform.position.x + x - (width/2f), 0, this.transform.position.z + y - (height/2f)), Quaternion.identity, this.transform);
						//asset.transform.localScale = asset.transform.localScale * (10 * Random.value + 4);
						//asset.transform.localScale = asset.transform.localScale * 0.5f;
						assetMaster.generateObject (x, y, height, width, this.transform, heightMap.values[x,y]);
					}*/
					
				assetMaster.generateObject (x, y, height, width, this.transform, heightMap.values[x,y], meshHeightMap[x,y]);

					/*if(heightMap.values[x,y] < 0.0){
						
						GameObject asset = Instantiate (testMesh, new Vector3 (this.transform.position.x + x - (width/2f), Random.Range(20,100), this.transform.position.z + y - (height/2f)), Quaternion.identity, this.transform);

						asset.transform.eulerAngles = new Vector3 (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));

						Vector3 dir = new Vector3 (20, 0, 0);
						asset.AddComponent<Rotater> ().RotationPerSecond = dir;

						asset.transform.localScale = asset.transform.localScale * 10 * Random.value;

					}*/
				}
			}
		}
	}
}