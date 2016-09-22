using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public PlayerController zaplug;
	private Vector3 lastPlayerLoc;
	private float distanceToMove;

	// Use this for initialization
	void Start () {
		zaplug = FindObjectOfType<PlayerController> (); // only 1 player
		lastPlayerLoc = zaplug.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		distanceToMove = zaplug.transform.position.x - lastPlayerLoc.x;
		transform.position = new Vector3 (transform.position.x + distanceToMove, 
			transform.position.y, transform.position.z);
		lastPlayerLoc = zaplug.transform.position;
	}
}
