using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// saves on processor memory by moving the same platforms along
// instead of instantiating every time
public class ObjectPooler : MonoBehaviour {

	public GameObject pooledObject;
	public int pooledAmount = 3;
	private List<GameObject> pooledObjects;
	public GameObject platforms; // instantiate as a child of this for cleaner code

	// Use this for initialization
	void Start () {
		pooledObjects = new List<GameObject> (pooledAmount);
		for (int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject) Instantiate (pooledObject);
			obj.SetActive (false);
			obj.transform.parent = platforms.transform;
			pooledObjects.Add (obj);
		}
	
	}

	// finds the first inactive pooled object in the list
	public GameObject GetPooledObject() {
		for (int i = 0; i < pooledObjects.Count; i++) {
			if (!pooledObjects [i].activeInHierarchy)
				return pooledObjects [i];
		}

		// if all active, create an inactive object
		GameObject obj = (GameObject) Instantiate (pooledObject);
		obj.SetActive (false);
		obj.transform.parent = platforms.transform;
		pooledObjects.Add (obj);
		return obj;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
