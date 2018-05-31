using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionViewer : MonoBehaviour {

	public Text emotion;
	public Text intensity;
	public Text timeLeft;
	public Director director;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(director.getEmotionLevel() < 5)
			emotion.text = "Emotion: Negative(" + director.getEmotionLevel() +")";
		else if (director.getEmotionLevel() > 5)
			emotion.text = "Emotion: Positive (" + director.getEmotionLevel() +")";
		else
			emotion.text = "Emotion: Neutral(" + director.getEmotionLevel() +")";

		intensity.text = "Intensity: " + director.getIntensityLevel();

		timeLeft.text = "Seconds Remaining: " + Mathf.RoundToInt(director.getTimeLeft());

	}
}
