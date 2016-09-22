using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Transform platformGenerator;
	private Vector3 platformStartPoint;

	public PlayerController player;
	private Vector3 playerStartPoint;

	private PlatformDestroyer[] platforms; // excluding 2 at beginnning

	public ScoreManager scoreManager;

	// Use this for initialization
	void Start () {
		// remember start points
		platformStartPoint = platformGenerator.position;
		playerStartPoint = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RestartGame() {
		StartCoroutine("RestartGameCo");
	}

	public IEnumerator RestartGameCo() {
		scoreManager.scoreIncreasing = false;
		player.gameObject.SetActive (false);
		yield return new WaitForSeconds(0.5f); // wait before starting new game
		platforms = FindObjectsOfType<PlatformDestroyer>();
		for (int i = 0; i < platforms.Length; i++) {
			platforms [i].gameObject.SetActive (false);
		}
		player.transform.position = playerStartPoint;
		platformGenerator.position = platformStartPoint;
		player.gameObject.SetActive (true);

		scoreManager.score = 0;
		scoreManager.scoreIncreasing = true;
	}
}
