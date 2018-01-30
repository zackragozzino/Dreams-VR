using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour {
	public Transform playerCamera;
	public Transform portal;
	public Transform otherPortal;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//Moves the camera to match movement of the player (with the initial offset)
		Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
		transform.position = portal.position + playerOffsetFromPortal;

		//Rotate the camera to match the direction the player is looking in
		float angularDifferenceBetweenPortalRotations = Quaternion.Angle (portal.rotation, otherPortal.rotation);

		//ew...
		Quaternion portalRotationalDifference = Quaternion.AngleAxis (angularDifferenceBetweenPortalRotations, Vector3.up);
		Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
		transform.rotation = Quaternion.LookRotation (newCameraDirection, Vector3.up);
		
	}
}
