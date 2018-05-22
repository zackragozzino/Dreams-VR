using DigitalRuby.WeatherMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NickWeatherManager : MonoBehaviour {

	public GameObject weatherPrefab;
	public WeatherMakerProfileScript[] profiles;
	private int counter;
	// Use this for initialization
	void Start () {
		WeatherMakerScript.Instance._WeatherProfile = profiles[2];
	}
	
	// Update is called once per frame
	void Update () {
		counter++;
		if (counter % 1000 == 0)
		{
			WeatherMakerScript.Instance._WeatherProfile = profiles[(int)(Random.Range(0f, 2.0f))];
		}

	}
}
