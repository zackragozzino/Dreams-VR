﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();
		if (Input.GetKeyDown (KeyCode.Tab))
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		
	}
}
