using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour {

	public Text scoreText;
	public Text highScoreText;

	private int highScore;
	private int sessionScore;

	// Use this for initialization
	void Start() {
		highScore = 0;
		sessionScore = 0;
	}

	public void AddPoints() {
		sessionScore += 1;
		UpdateScoreText();
	}

	public void SessionEnd() {
		if (sessionScore > highScore) {
			highScore = sessionScore;
			UpdateHighScoreText();
		}

		sessionScore = 0;
		UpdateScoreText();
	}

	public void UpdateScoreText() {
		scoreText.text = "Score: " + sessionScore.ToString();
	}

	void UpdateHighScoreText() {
		highScoreText.text = "High Score: " + highScore.ToString();
	}

}
