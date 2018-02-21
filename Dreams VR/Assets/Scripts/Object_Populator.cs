using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Populator : MonoBehaviour {

	public Renderer textureRender;

	public MeshSettings meshSettings;
	public HeightMapSettings heightMapSettings;
	public TextureData textureData;


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
		
	/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
	void OnHeightMapReceived(object heightMapObject) {
		this.heightMap = (HeightMap)heightMapObject;
		heightMapReceived = true;

		TextureFromHeightMap ();
	}

	public void TextureFromHeightMap() {
		int width = heightMap.values.GetLength (0);
		int height = heightMap.values.GetLength (1);

		if (heightMapReceived) {

			//Instantiate (testMesh, new Vector3 (this.transform.position.x - (width / 2f), 0, this.transform.position.z - (height / 2f)), Quaternion.identity, this.transform);


			float[] noiseMap = new float[width * height];
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					//colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, Mathf.InverseLerp(heightMap.minValue,heightMap.maxValue,heightMap.values [x, y]));
					//colourMap [y * width x] = Mathf.Lerp(1, 0,  Mathf.InverseLerp(heightMap.minValue,heightMap.maxValue,heightMap.values [x, y]));
					//print(heightMap.values[x,y]);
					if (heightMap.values [x, y] > 0.9f) {
						Instantiate (testMesh, new Vector3 (this.transform.position.x + x - (width/2f), 0, this.transform.position.z + y - (height/2f)), Quaternion.identity, this.transform);
					}
				}
			}
		}
	}
}
