using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleAudio : MonoBehaviour {

	public AudioClip first;
	public AudioClip second;
	public AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();	
	}
}
