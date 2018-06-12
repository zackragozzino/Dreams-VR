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
			//collapseWalls ();
			doorAnimator.Play ("Door_open");
			audm.Play ("DoorOpen");
			director.startPortalGeneration ();
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
		Transform lController = GameObject.Find ("Controller (left)").transform.GetChild (0);
		if (lController != null && lController.childCount > 0) {
			leftControllerEvents = GameObject.Find ("Controller (left)").transform.GetChild (1).GetComponent<VRTK.VRTK_ControllerEvents>();
			leftInteractGrab = GameObject.Find ("Controller (left)").transform.GetChild (1).GetComponent<VRTK.VRTK_InteractGrab> ();

			lTrackPad = lController.GetChild (12).gameObject;
			lTrigger = lController.GetChild (15).gameObject;
		}

		Transform rController = GameObject.Find ("Controller (right)").transform.GetChild (0);
		if (rController != null && rController.childCount > 0) {
			rightControllerEvents = GameObject.Find ("Controller (right)").transform.GetChild (1).GetComponent<VRTK.VRTK_ControllerEvents>();
			rightInteractGrab = GameObject.Find ("Controller (right)").transform.GetChild (1).GetComponent<VRTK.VRTK_InteractGrab> ();

			rTrackPad = rController.GetChild (12).gameObject;
			rTrigger = rController.GetChild (15).gameObject;
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
		//Destroy (ceiling, 1.5f);

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
	}

	IEnumerator tutorialRoutine(){
		tutorialText.text = "";
        doorAnimator.Play("Door_close");
        yield return new WaitForSeconds (5f);

		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
      if (FacebookLoginHybriona.Instance.firstName != "") {
         tutorialText.text = "Hi " + FacebookLoginHybriona.Instance.firstName + ",\n" + 
         "Welcome to DreamWalker VR.";
      }
      else {
		   tutorialText.text = "Welcome to DreamWalker VR.";
      }
		//Fade in
		StartCoroutine (fadeInText (3f));
		yield return new WaitForSeconds (3f);
		//Let the player read it
		yield return new WaitForSeconds (3f);
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
		yield return new WaitForSeconds (3f);
		//Fade out
		StartCoroutine (fadeOutText (1.5f));
		yield return new WaitForSeconds(1.5f);
		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/



		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
		tutorialText.text = 
		"Place your thumb on the " +
		"\ntrack pad located on the" +
		"\ncontroller to move in" +
		"\ndifferent directions.";

		getViveControllers();
		//trackpad = GameObject.Find ("trackpad");
		Renderer lTrackpadRenderer = lTrackPad.GetComponent<Renderer> ();
		Renderer rTrackpadRenderer = rTrackPad.GetComponent<Renderer> ();
		//Save the trackpad material for later
		Material trackpadMaterial = lTrackpadRenderer.material;

		//Turn the trackpad on the controllers red
		lTrackpadRenderer.material = attentionMaterial;
		rTrackpadRenderer.material = attentionMaterial;

		//Fade in
		StartCoroutine (fadeInText (3f));
		yield return new WaitForSeconds (3f);

		while (!leftControllerEvents.touchpadTouched && !rightControllerEvents.touchpadTouched) {
			//Let the player read it
			yield return null;
		}

		//Return the trackpad to its original color
		lTrackpadRenderer.material = trackpadMaterial;
		rTrackpadRenderer.material = trackpadMaterial;

		//Fade out
		StartCoroutine (fadeOutText (1.5f));
		yield return new WaitForSeconds(1.5f);
		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
		tutorialText.text = 
		"The trigger is located on the" +
		"\nunderside of your controller." +
		"\nTry pressing it.";

		//trigger = GameObject.Find ("trigger");
		Renderer lTriggerRenderer = lTrigger.GetComponent<Renderer> ();
		Renderer rTriggerRenderer = rTrigger.GetComponent<Renderer> ();
		//Save the trigger material for later
		Material triggerMaterial = lTriggerRenderer.material;
		//Turn the trigger on the controllers red
		lTriggerRenderer.material = attentionMaterial;
		rTriggerRenderer.material = attentionMaterial;

		//Fade in
		StartCoroutine (fadeInText (3f));
		yield return new WaitForSeconds (3f);

		while (!leftControllerEvents.triggerPressed && !rightControllerEvents.triggerPressed) {
			yield return null;
		}
			
		//Return the trigger to its original color
		lTriggerRenderer.material = triggerMaterial;
		rTriggerRenderer.material = triggerMaterial;

		//Fade out
		StartCoroutine (fadeOutText (1.5f));
		yield return new WaitForSeconds(1.5f);
		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/



		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
		tutorialText.text = 
		"Certain objects can be grabbed" +
		"\nby hovering over them and " +
		"\nholding the trigger." +
		"\nTry picking up a small item.";
		
		//Fade in
		StartCoroutine (fadeInText (3f));
		yield return new WaitForSeconds (3f);

		while (leftInteractGrab.GetGrabbedObject() == null && rightInteractGrab.GetGrabbedObject() == null) {
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
		yield return new WaitForSeconds (3f);
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
