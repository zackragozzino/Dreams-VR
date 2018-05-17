using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class ColorController : MonoBehaviour {

	private Director director;

	void Start () {
		director = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Director>();
	}


	public void addColorScheme(ColorCorrectionCurves newColorScheme){
		Debug.Log ("Changing color schemes...");
		ColorCorrectionCurves colorScheme = gameObject.GetComponent<ColorCorrectionCurves> ();

		colorScheme.redChannel = newColorScheme.redChannel;
		colorScheme.blueChannel = newColorScheme.blueChannel;
		colorScheme.greenChannel = newColorScheme.greenChannel;
		//colorScheme.saturation = newColorScheme.saturation;
		colorScheme.saturation = getSaturationLevel();
			
		if (newColorScheme.mode == ColorCorrectionCurves.ColorCorrectionMode.Advanced) {
			colorScheme.mode = ColorCorrectionCurves.ColorCorrectionMode.Advanced;
			colorScheme.depthRedChannel = newColorScheme.depthRedChannel;
			colorScheme.depthGreenChannel = newColorScheme.depthGreenChannel;
			colorScheme.depthRedChannel = newColorScheme.depthRedChannel;
		}

		colorScheme.UpdateParameters();
	}

	private float getSaturationLevel(){
		int intensityLevel = director.getIntensityLevel ();
		float minSaturation = 0.5f;
		float maxSaturation = 5f;
		int intensityMin = 0;
		int intensityMax = 10;

		return (float)(minSaturation + (intensityLevel - intensityMin) * (maxSaturation - minSaturation) / (intensityMax - intensityMin));
	}
}
