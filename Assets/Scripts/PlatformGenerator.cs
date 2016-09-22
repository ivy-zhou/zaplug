using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {

	// different kinds of platforms
	//public GameObject[] platformPrefabs;
	public ObjectPooler[] platformPools;
	private float[] platformWidths;

	public Transform generationPoint;
	//private float distanceBetween;

	//private float platformWidth;

	// range of gap
	public float distanceMin = 7;
	public float distanceMax = 11;

	public Transform maxHeightPoint;
	private float heightMin; // max and min height of screen
	private float heightMax;
	private float heightChangeMax = 6;

	//public ObjectPooler objectPool;

	// Use this for initialization
	void Start () {
		platformWidths = new float [platformPools.Length];

		for(int i = 0; i < platformPools.Length; i++)
			platformWidths[i] = platformPools[i].pooledObject.GetComponent<BoxCollider2D> ().size.x * 3;

		heightMin = transform.position.y;
		heightMax = maxHeightPoint.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x < generationPoint.position.x) {
			int platformSelect = Random.Range (0, platformPools.Length);

			float distanceBetween = Random.Range (distanceMin, distanceMax);
			float heightChange = transform.position.y + Random.Range (-heightChangeMax, heightChangeMax);

			if (heightChange > heightMax)
				heightChange = heightMax;
			else if (heightChange < heightMin)
				heightChange = heightMin;

			// instantiate at next position
			transform.position = new Vector3 (transform.position.x + (platformWidths[platformSelect] / 2) + distanceBetween, 
				heightChange, transform.position.z); 
			
			GameObject nextPlatform = platformPools[platformSelect].GetPooledObject ();
			nextPlatform.transform.position = transform.position;
			nextPlatform.transform.rotation = transform.rotation;
			nextPlatform.SetActive (true);

			transform.position = new Vector3 (transform.position.x + (platformWidths[platformSelect] / 2), 
				transform.position.y, transform.position.z); 


			//Instantiate (platformPrefabs[platformSelect].pooledObject, transform.position, transform.rotation);
		}
	}
}
