using DigitalRuby.WeatherMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NickWeatherManager : MonoBehaviour {



	public enum WeatherProfile { Blizzard, LightSnow, HeavyRain, LightRain, Sunny, none };

	public WeatherProfile profile;

	public GameObject weatherPrefab;
	public WeatherMakerProfileScript[] profiles;
	[Tooltip("Intensity of precipitation (0-1)")]
	[Range(0.0f, 1.0f)]
	public float PrecipitationIntensity;
	private int counter;
	// Use this for initialization
	void Start () {
		WeatherMakerScript.Instance._WeatherProfile = profiles[4];
	}
	
	// Update is called once per frame
	void Update () {
		if (profile == WeatherProfile.Blizzard)
		{
			WeatherMakerScript.Instance._WeatherProfile = profiles[0];

		}
		if (profile == WeatherProfile.LightSnow)
		{
			WeatherMakerScript.Instance._WeatherProfile = profiles[1];

		}
		if (profile == WeatherProfile.HeavyRain)
		{
			WeatherMakerScript.Instance._WeatherProfile = profiles[2];

		}
		if (profile == WeatherProfile.LightRain)
		{
			WeatherMakerScript.Instance._WeatherProfile = profiles[3];

		}
		if (profile == WeatherProfile.Sunny)
		{
			WeatherMakerScript.Instance._WeatherProfile = profiles[4];

		}
		WeatherMakerScript.Instance.PrecipitationIntensity = this.PrecipitationIntensity;

	}
}
