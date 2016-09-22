using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public Text scoreText;
	public Text highScoreText;

	public float score;
	public float highScore;

	public float pointsPerSecond;
	public bool scoreIncreasing = true;

	// Use this for initialization
	void Start () {
		highScore = PlayerPrefs.HasKey("highScore") ? PlayerPrefs.GetFloat("highScore") : 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (scoreIncreasing) {
			score += pointsPerSecond * Time.deltaTime;

			if (score > highScore) {
				highScore = score;
				PlayerPrefs.SetFloat ("highScore", highScore);
			}

			scoreText.text = "Score: " + Mathf.Round (score);
			highScoreText.text = "High score: " + Mathf.Round (highScore);
		}
	}
}
