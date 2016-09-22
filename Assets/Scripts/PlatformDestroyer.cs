using UnityEngine;
using System.Collections;

public class PlatformDestroyer : MonoBehaviour {

	public GameObject destructionPoint;

	// Use this for initialization
	void Start () {
		destructionPoint = GameObject.Find ("PlatformDestructionPoint");
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x < destructionPoint.transform.position.x)
			gameObject.SetActive (false);
			//Destroy (gameObject);
	
	}
}
