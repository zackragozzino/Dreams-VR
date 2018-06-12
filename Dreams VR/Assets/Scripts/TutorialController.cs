using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

	public Text tutorialText;
	private AudioManager audm;

	private GameObject trackpad;
	private GameObject trigger;

	public Material attentionMaterial;

	private VRTK.VRTK_ControllerEvents vrControllerEvents;
	private VRTK.VRTK_InteractGrab vrInteractGrab;

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

		//director.getPlayer ().transform.position = new Vector3 (0.5f, 2.1f, -1.9f);

		if (director.getVRStatus ()) {
			vrControllerEvents = FindObjectOfType<VRTK.VRTK_ControllerEvents> ();
			vrInteractGrab = FindObjectOfType<VRTK.VRTK_InteractGrab> ();

			StartCoroutine (tutorialRoutine ());
		} else {
			collapseWalls ();
			doorAnimator.Play ("Door_open");
			audm.Play ("DoorOpen");
			director.startPortalGeneration ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P))
			collapseWalls ();
	}

	void collapseWalls(){
		//ceiling.GetComponent<BoxCollider> ().enabled = false;

		audm.Play ("Ding");
		audm.Play ("ambient1");
		audm.Play ("WindTrees");

		float force = 50f;

		ceiling.AddComponent<Rigidbody> ();
		ceiling.GetComponent<Rigidbody> ().AddExplosionForce (500f, new Vector3 (0.5f, 2.1f, -1.9f), 100, 3000.0f);
		Destroy (ceiling, 1.5f);

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
         "Welcome to DreamWalker VR.\n" + 
         "Please read the tutorial that follows.";
      }
      else {
		   tutorialText.text = "Welcome to DreamWalker VR." + 
         "Please read the tutorial that follows.";
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
		"You can move forwards, backwards, " +
		"\nleft, and right by touching" +
		"\nthe circular trackpad on the controller" +
		"\nin the corresponding" + 
      "\ndirection. Try it out now.";

		trackpad = GameObject.Find ("trackpad");
		Renderer trackpadRenderer = trackpad.GetComponent<Renderer> ();
		//Save the trackpad material for later
		Material trackpadMaterial = trackpadRenderer.material;
		//Turn the trackpad on the controller red
		trackpadRenderer.material = attentionMaterial;

		//Fade in
		StartCoroutine (fadeInText (3f));
		yield return new WaitForSeconds (3f);

		float horizontal = 0;
		float vertical = 0; 

		while (horizontal == 0 && vertical == 0) {
			
			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical"); 
			//Let the player read it

			yield return null;
		}

		yield return new WaitForSeconds (1.5f);

		//Return the trackpad to its original color
		trackpadRenderer.material = trackpadMaterial;

		//Fade out
		StartCoroutine (fadeOutText (1.5f));
		yield return new WaitForSeconds(1.5f);
		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
		tutorialText.text = 
		"You will move based on where " +
		"\nyou are looking. " +
		"\nTo avoid getting sick, " +
		"\nwe recommend not using the left " +
      "\nand right movement, but to look " +
      "\nin that direction to turn.";
		
		//Fade in
		StartCoroutine (fadeInText (3f));
		yield return new WaitForSeconds (3f);

		//Fade out
		StartCoroutine (fadeOutText (1.5f));
		yield return new WaitForSeconds(1.5f);
		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
		tutorialText.text = 
		"You can also pick up objects " +
      "\nby reaching out to touch them " +
		"\nwith the controller." +
		"\nThen hold the trigger with " + 
      "\nyour pointer finger to pick it up." + 
      "\nTry it now.";
		trigger = GameObject.Find ("trigger");
		Renderer triggerRenderer = trigger.GetComponent<Renderer> ();
		//Save the trigger material for later
		Material triggerMaterial = triggerRenderer.material;
		//Turn the trigger on the controller red
		triggerRenderer.material = attentionMaterial;

		//Fade in
		StartCoroutine (fadeInText (3f));
		yield return new WaitForSeconds (3f);

		while (!vrControllerEvents.triggerPressed) {
			yield return null;
		}

		yield return new WaitForSeconds (1.5f);
		//Return the trigger to its original color
		triggerRenderer.material = triggerMaterial;


		while (vrInteractGrab.GetGrabbedObject() == null) {
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
		StartCoroutine (fadeInText (3f));
		yield return new WaitForSeconds (3f);
		//Let the player read it
		yield return new WaitForSeconds (5f);
		//Fade out
		StartCoroutine (fadeOutText (1.5f));
		yield return new WaitForSeconds(1.5f);
		/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

		doorAnimator.Play ("Door_open");
		audm.Play ("DoorOpen");
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
}
