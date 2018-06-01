using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBCube : MonoBehaviour {

	// Use this for initialization
	void Start () {

		if (FacebookLogin.Instance.getLoggedIn()) {
			// If so, get the FB image and set the texture to it
			Renderer renderer = this.GetComponent<Renderer>();
			renderer.material.mainTexture = FacebookLogin.Instance.getNextUserPhoto();
		}

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
