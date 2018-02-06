using System.Collections;
using UnityEngine;

public static class TextureGenerator {

	//Creates a texture applies the given color values
	//Specified in the color map
	public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height){
		Texture2D texture = new Texture2D (width, height);
		//Fixes texture blurriness
		texture.filterMode = FilterMode.Point;
		//Fixes overlap on the edges of the texture
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels (colorMap);
		texture.Apply ();
		return texture;
	}

	//Translates the height values to b/w color values
	//Then sends this color map to the TextureFromColorMap() function
	public static Texture2D TextureFromHeightMap(float[,] heightMap){
		int width = heightMap.GetLength (0);
		int height = heightMap.GetLength (1);

		/*Calculate all color values before applying them to the texture.
		* This is considered faster than applying them after each calculation.
		*/
		Color[] colorMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				//Index of colormap relative to the index in the noise map 2D array
				colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
			}
		}
		return TextureFromColorMap (colorMap, width, height);
	}
	
}
