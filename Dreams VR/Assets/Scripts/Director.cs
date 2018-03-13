using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour {

	public GameObject[] dreamScripts;
	private float timer;
	private int timerMin = 5;
	private int timerMax = 30;

	private GameObject player;

	// Use this for initialization
	void Start () {
		timer = Random.Range (timerMin, timerMax);
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {

		/*timer -= Time.deltaTime;

		if (timer <= 0) {
			AddScript ();
			timer = Random.Range (timerMin, timerMax);
		}*/

		if (Input.GetKeyDown (KeyCode.G)) {
			Instantiate (dreamScripts [0], player.transform.position, Quaternion.identity, this.transform);
			Debug.Log ("test");
		}

		if (Input.GetKeyDown (KeyCode.H)) {
			Instantiate (this, new Vector3 (this.transform.position.x, this.transform.position.y - 500, this.transform.position.z), Quaternion.identity, transform);
		}
		
	}

	void AddScript(){
		GameObject dreamScript = Instantiate (dreamScripts [Random.Range (0, dreamScripts.Length)], this.transform.position, Quaternion.identity, this.transform);
		float waitTime = Random.Range (5, 10);
		StartCoroutine (WaitAndKillGameObject(dreamScript, waitTime));
	}

	IEnumerator WaitAndKillGameObject(GameObject gameObject, float waitTime){
		yield return new WaitForSeconds (waitTime);
		Destroy (gameObject);
	}
}
