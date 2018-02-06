using System.Collections;
using UnityEngine;

public static class Noise{

	//Use local if you're only generating terrain for a single "chunk"
	//Otherwise use global to eliminate seam issues
	public enum NormalizeMode {Local, Global};

	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode){
		float[,] noiseMap = new float[mapWidth, mapHeight];

		//Generates a random offset for the array based off the seed value
		System.Random prng = new System.Random (seed);
		Vector2[] octaveOffsets = new Vector2[octaves];

		float maxPossibleHeight = 0;
		float amplitude = 1;
		float frequency = 1;

		for (int i = 0; i < octaves; i++) {
			//Keeps the number from getting too high otherwise perline returns weird values
			float offsetX = prng.Next (-100000, 100000) + offset.x;
			float offsetY = prng.Next (-100000, 100000) - offset.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);

			maxPossibleHeight += amplitude;
			amplitude *= persistance;
		}

		//A scale of 0 would cause a divide by zero error
		if (scale <= 0)
			scale = 0.0001f;

		float maxLocalNoiseHeight = float.MinValue;
		float minLocalNoiseHeight = float.MaxValue;

		//Ensures the noise scale parameter zooms in from the center of the map
		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

				amplitude = 1;
				frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {
					float sampleX = (x - halfWidth + + octaveOffsets[i].x) / scale * frequency;
					float sampleY = (y - halfHeight + + octaveOffsets[i].y) / scale * frequency;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1; //A value between -1 and 1
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance; //Decreases with each octave
					frequency *= lacunarity; //Increases with each octave
				}

				if (noiseHeight > maxLocalNoiseHeight)
					maxLocalNoiseHeight = noiseHeight;
				else if (noiseHeight < minLocalNoiseHeight)
					minLocalNoiseHeight = noiseHeight;
				
				noiseMap [x, y] = noiseHeight;
			}
		}

		/*
		 * For each value in the noise map, 
		 * remap the value to fit within the range determined
		 * by the max and min values
		 * */
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				if (normalizeMode == NormalizeMode.Local) {
					noiseMap [x, y] = Mathf.InverseLerp (minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap [x, y]);
				} else {
					float normalizedHeight = (noiseMap [x, y] + 1) / (maxPossibleHeight);
					noiseMap [x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
				}
			}
		}
		return noiseMap;
	}
}
