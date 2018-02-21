using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour {
	public bool continuous;
	const float degreesPerHour = 30f;
	const float degreesPerMinute = 6f;
	const float degreesPerSecond = 6f;
	public Transform hoursTransform, minutesTransform, secondsTransform;
	// Use this for initialization
	void Awake () {
		DateTime time = DateTime.Now;
		hoursTransform.localRotation = Quaternion.Euler(0f, time.Hour * degreesPerHour, 0f);
		minutesTransform.localRotation = Quaternion.Euler(0f, time.Minute * degreesPerMinute, 0f);
		secondsTransform.localRotation = Quaternion.Euler(0f, time.Second * degreesPerSecond, 0f);
	}
	
	void Update() {
		if (continuous) {
			UpdateContinuous();
		} else {
			UpdateDiscrete();
		}
	}

	// Update is called once per frame
	void UpdateDiscrete () {
		DateTime time = DateTime.Now;
		hoursTransform.localRotation = Quaternion.Euler(0f, time.Hour * degreesPerHour, 0f);
		minutesTransform.localRotation = Quaternion.Euler(0f, time.Minute * degreesPerMinute, 0f);
		secondsTransform.localRotation = Quaternion.Euler(0f, time.Second * degreesPerSecond, 0f);
	}

	// Update is called once per frame
	void UpdateContinuous () {
		TimeSpan time = DateTime.Now.TimeOfDay;
		hoursTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalHours * degreesPerHour, 0f);
		minutesTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalMinutes * degreesPerMinute, 0f);
		secondsTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalSeconds * degreesPerSecond, 0f);
	}
}
