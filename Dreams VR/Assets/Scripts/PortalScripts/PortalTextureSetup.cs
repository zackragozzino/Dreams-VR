using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour {

	public Camera cameraA;
	public Camera cameraB;
	public Camera cameraC;
	public Camera cameraD;

	public Material cameraMatA;
	public Material cameraMatB;
	public Material cameraMatC;
	public Material cameraMatD;

	// Use this for initialization
	void Start () {
		/*
		 * Sets the initial resolution of the portals
		 * to match the resolution of the computer screen
		 * This script only runs at the very beginning of runtime
		 * */

		//Portal A
		if (cameraA.targetTexture != null)
		{
			cameraA.targetTexture.Release();
		}
		cameraA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMatA.mainTexture = cameraA.targetTexture;

		//Portal B
		if (cameraB.targetTexture != null)
		{
			cameraB.targetTexture.Release();
		}
		cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMatB.mainTexture = cameraB.targetTexture;

		//Portal C
		if (cameraC.targetTexture != null)
		{
			cameraC.targetTexture.Release();
		}
		cameraC.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMatC.mainTexture = cameraC.targetTexture;

		//Portal D
		if (cameraD.targetTexture != null)
		{
			cameraD.targetTexture.Release();
		}
		cameraD.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMatD.mainTexture = cameraD.targetTexture;
	}
	
}
