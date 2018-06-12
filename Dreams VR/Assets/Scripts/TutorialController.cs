using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

	public Text tutorialText;
	private AudioManager audm;

	private GameObject lTrackPad;
	private GameObject lTrigger;
	private GameObject rTrackPad;
	private GameObject rTrigger;

	private Material trackpadMaterial;
	private Material triggerMaterial;
	public Material attentionMaterial;

	private VRTK.VRTK_ControllerEvents leftControllerEvents;
	private VRTK.VRTK_ControllerEvents rightControllerEvents;
	private VRTK.VRTK_InteractGrab leftInteractGrab;
	private VRTK.VRTK_InteractGrab rightInteractGrab;

	private Director director;

	public Animator doorAnimator; 

	public GameObject backWall;
	public GameObject frontWall;
	public GameObject leftWall;
	public GameObject rightWall;
	public GameObject ceiling;

	// Use this for initialization
	void Start () {
		audm = FindObjectOfType<AudioManager>();
		director = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Director> ();

		//StartCoroutine (fadeOutCeiling (3f));

		if (director.getVRStatus ()) {
			StartCoroutine (tutorialRoutine ());
		} else {
			collapseWalls ();
			doorAnimator.Play ("Door_open");
			audm.Play ("DoorOpen");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			collapseWalls ();
			StartCoroutine (fadeOutCeiling (1.5f));
		}
	}

	void getViveControllers(){
		if (GameObject.Find ("Controller (left)") != null) {
			Transform lController = GameObject.Find ("Controller (left)").transform.GetChild (0);
			if (lController != null && lController.childCount > 0) {
				leftControllerEvents = GameObject.Find ("Controller (left)").transform.GetChild (1).GetComponent<VRTK.VRTK_ControllerEvents> ();
				leftInteractGrab = GameObject.Find ("Controller (left)").transform.GetChild (1).GetComponent<VRTK.VRTK_InteractGrab> ();

				lTrackPad = lController.GetChild (12).gameObject;
				lTrigger = lController.GetChild (15).gameObject;

				trackpadMaterial = lTrackPad.GetComponent<Renderer> ().material;
				triggerMaterial = lTrigger.GetComponent<Renderer> ().material;
			}
		}
			
		if (GameObject.Find ("Controller (right)") != null) {
			Transform rController = GameObject.Find ("Controller (right)").transform.GetChild (0);
			if (rController != null && rController.childCount > 0) {
				rightControllerEvents = GameObject.Find ("Controller (right)").transform.GetChild (1).GetComponent<VRTK.VRTK_ControllerEvents> ();
				rightInteractGrab = GameObject.Find ("Controller (right)").transform.GetChild (1).GetComponent<VRTK.VRTK_InteractGrab> ();

				rTrackPad = rController.GetChild (12).gameObject;
				rTrigger = rController.GetChild (15).gameObject;

				trackpadMaterial = rTrackPad.GetComponent<Renderer> ().material;
				triggerMaterial = rTrigger.GetComponent<Renderer> ().material;
			}
		}
	}

	void collapseWalls(){
		//ceiling.GetComponent<BoxCollider> ().enabled = false;

		audm.Play ("Ding");
		audm.Play ("ambient1");
		audm.Play ("WindTrees");

		float force = 50f;

		//ceiling.AddComponent<Rigidbody> ();
		//ceiling.GetComponent<Rigidbody> ().AddExplosionForce (500f, new Vector3 (0.5f, 2.1f, -1.9f), 100, 3000.0f);
		Destroy (ceiling);

		backWall.AddComponent<Rigidbody> ();
		backWall.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, 5, -force));
		//backWall.GetComponent<Rigidbody> ().AddExplosionForce (100f, new Vector3 (0.5f, 2.1f, -1.9f), 100, 3.0f);

		frontWall.AddComponent<Rigidbody> ();
		frontWall.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, 5, force));
		//frontWall.GetComponent<Rigidbody> ().AddExplosionForce (100f, new Vector3 (0.5f, 2.1f, -1.9f), 100, 3.0f);

		leftWall.AddComponent<Rigidbody> ();
		leftWall.GetComponent<Rigidbody> ().AddForce (new Vector3 (-force, 5, 0));
		//leftWall.GetComponent<Rigidbody> ().AddExplosionForce (100f, new Vector3 (0.5f, 2.1f, -1.9f), 100, 3.0f);

		rightWall.AddComponent<Rigidbody> ();
		rightWall.GetComponent<Rigidbody> ().AddForce (new Vector3 (force, 5, 0));
		//rightWall.GetComponent<Rigidbody> ().AddExplosionForce (100f, new Vector3 (0.5f, 2.1f, -1.9f), 100, 3.0f);

		director.startPortalGeneration ();
		director.startDreamTimer ();
	}

	IEnumerator tutorialRoutine(){
		tutorialText.text = "";
        doorAnimator.Play("Door_close");
        yield return new WaitForSeconds (2f);

		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
      if (FacebookLoginHybriona.Instance.firstName != "") {
         tutorialText.text = "Hi " + FacebookLoginHybriona.Instance.firstName + ",\n" + 
				"Welcome to DreamWalker VR tutorial."; 
      }
      else {
			tutorialText.text = "Welcome to DreamWalker VR tutorial."; 
      }
		//Fade in
		StartCoroutine (fadeInText (3f));
		yield return new WaitForSeconds (3f);
		//Let the player read it
		yield return new WaitForSeconds (1f);
		//Fade out
		StartCoroutine (fadeOutText (1.5f));
		yield return new WaitForSeconds(1.5f);
		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
		tutorialText.text = 
		"Before we begin, " +					
		"\nlet's go over the controls.";
		//Fade in
		StartCoroutine (fadeInText (3f));
		yield return new WaitForSeconds (3f);
		//Let the player read it
		yield return new WaitForSeconds (1f);
		//Fade out
		StartCoroutine (fadeOutText (1.5f));
		yield return new WaitForSeconds(1.5f);
		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/



		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
		tutorialText.text = 
		/*"Place your thumb on the " +
		"\ntrack pad located on the" +
		"\ncontroller to move in" +
		"\ndifferent directions.";*/

		"You can move forwards, backwards, " +
		"\nleft, and right by touching" +
		"\nthe circular trackpad on the" +
		"\ncontroller in the" + 
		"\ncorresponding direction. " +
		"\nTry it out now.";

		getViveControllers();

		//Turn the trackpad on the controllers red
		if(lTrackPad != null)
			lTrackPad.GetComponent<Renderer>().material = attentionMaterial;
		if(rTrackPad != null)
			rTrackPad.GetComponent<Renderer>().material = attentionMaterial;

		//Fade in
		StartCoroutine (fadeInText (3f));
		yield return new WaitForSeconds (3f);

		bool touchpadPressed = false;

		while (!touchpadPressed) {
			if (leftControllerEvents != null && leftControllerEvents.touchpadTouched)
				touchpadPressed = true;
			if (rightControllerEvents != null && rightControllerEvents.touchpadTouched)
				touchpadPressed = true;
			yield return null;
		}

		//Return the trackpad to its original color
		if(lTrackPad != null)
			lTrackPad.GetComponent<Renderer>().material = trackpadMaterial;
		if(rTrackPad != null)
			rTrackPad.GetComponent<Renderer>().material = trackpadMaterial;

		//Fade out
		StartCoroutine (fadeOutText (1.5f));
		yield return new WaitForSeconds(1.5f);
		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
		tutorialText.text = 
		"To avoid getting sick, " +
		"\nwe recommend looking" +
		"\nin the direction you" +
		"\nwish to move.";

		StartCoroutine (fadeInText (1.5f));
		yield return new WaitForSeconds (1.5f);

		yield return new WaitForSeconds (3f);
		//Fade out
		StartCoroutine (fadeOutText (1.5f));
		yield return new WaitForSeconds(1.5f);

		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/



		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
		tutorialText.text = 
		"The trigger is located on the" +
		"\nunderside of your controller." +
		"\nTry pressing it.";

		//Turn the trigger on the controllers red
		if (lTrigger != null)
			lTrigger.GetComponent<Renderer>().material = attentionMaterial;
		if(rTrigger != null)
			rTrigger.GetComponent<Renderer>().material = attentionMaterial;

		//Fade in
		StartCoroutine (fadeInText (1.5f));
		yield return new WaitForSeconds (1.5f);

		bool triggerPressed = false;

		while (!triggerPressed) {
			if (leftControllerEvents != null && leftControllerEvents.triggerPressed)
				triggerPressed = true;
			if (rightControllerEvents != null && rightControllerEvents.triggerPressed)
				triggerPressed = true;
			yield return null;
		}
			
		//Return the trigger to its original color
		if (lTrigger != null)
			lTrigger.GetComponent<Renderer>().material = triggerMaterial;
		if(rTrigger != null)
			rTrigger.GetComponent<Renderer>().material = triggerMaterial;

		//Fade out
		StartCoroutine (fadeOutText (1.5f));
		yield return new WaitForSeconds(1.5f);

		tutorialText.text = 
			"You can also pick up objects " +
			"\nby reaching out to touch them " +
			"\nwith the controller." +
			"\nThen hold the trigger" + 
			"\nto pick it up." + 
			"\nTry it now.";
		
		//Fade in
		StartCoroutine (fadeInText (1.5f));
		yield return new WaitForSeconds (1.5f);

		bool objectGrabbed = false;

		while (!objectGrabbed) {
			if (leftInteractGrab != null && leftInteractGrab.GetGrabbedObject () != null)
				objectGrabbed = true;
			if (rightInteractGrab != null && rightInteractGrab.GetGrabbedObject () != null)
				objectGrabbed = true;
			yield return null;
		}
			

		//Fade out
		StartCoroutine (fadeOutText (1.5f));
		yield return new WaitForSeconds(1.5f);
		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
		tutorialText.text = 
			"Great! You are now ready" +
			"\nto explore the world of" +
			"\nDreamWalker VR. Have fun!";
		//Fade in
		StartCoroutine (fadeInText (1.5f));
		yield return new WaitForSeconds (1.5f);
		//Let the player read it
		yield return new WaitForSeconds (1.5f);
		//Fade out
		StartCoroutine (fadeOutText (1.5f));
		yield return new WaitForSeconds(1.5f);
		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

		//doorAnimator.Play ("Door_open");
		//audm.Play ("DoorOpen");
        collapseWalls();
   
        director.startPortalGeneration ();
	}

	IEnumerator fadeInText(float duration){
		audm.Play ("Transition");
		//Fade in text
		Color fadeColor = tutorialText.color;
		float timer = 0f;

		while (timer < duration) {
			fadeColor.a = timer / duration;
			timer += Time.deltaTime;
			tutorialText.color = fadeColor;
			yield return null;
		}
	}

	IEnumerator fadeOutText(float duration){
		//Fade the text to black
		Color fadeColor = tutorialText.color;
		float timer = 0f;

		while (timer < duration) {
			fadeColor.a = 1f - timer / duration;
			timer += Time.deltaTime;
			tutorialText.color = fadeColor;
			yield return null;
		}
	}

	IEnumerator fadeOutCeiling(float duration){
		Material ceilingMat = ceiling.GetComponent<MeshRenderer> ().material;
		Color ceilingColor = ceilingMat.color;
		float timer = 0f;

		Debug.Log (ceilingColor);

		Destroy (ceiling.GetComponent<BoxCollider> ());
		while (timer < duration) {
			ceilingColor.a = 1f - timer / duration;
			timer += Time.deltaTime;
			yield return null;
		}

		//Destroy (ceiling);
	}
}
