using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject mainMenuPanel;
	public GameObject pauseMenuPanel;
	public GameObject pauseButton;
	public GameObject sessionScoreText;
	public GameObject scoreTracker;
	public GameObject ball;
	public GameObject platformSpawner;

	private PlatformSpawner platformSpawnerSc;
	private BallController ballControllerSc;
	private ScoreTracker scoreTrackerSc;

	void Start() {
		scoreTrackerSc = scoreTracker.GetComponent<ScoreTracker>();
		ballControllerSc = ball.GetComponent<BallController>();
		platformSpawnerSc = platformSpawner.GetComponent<PlatformSpawner>();

		//Can not control the ball. The physics are not enabled untill 
		//the game start
		Time.timeScale = 0;
		ballControllerSc.enabled = false;

		InitializeSession();

	}

	public void StartSession() {
		mainMenuPanel.SetActive(false);
		sessionScoreText.SetActive(true);
		pauseButton.SetActive(true);

		ballControllerSc.enabled = true;
		Time.timeScale = 1;
	}

	public void PauseGame() {
		Time.timeScale = 0;
		ballControllerSc.enabled = false;
		pauseMenuPanel.SetActive(true);
		pauseButton.SetActive(false);
	}

	public void ResumeGame() {
		Time.timeScale = 1;
		ballControllerSc.enabled = true;
		pauseMenuPanel.SetActive(false);
		pauseButton.SetActive(true);
	}

	public void GameOver() {
		Time.timeScale = 0;
		ballControllerSc.enabled = false;

		scoreTrackerSc.SessionEnd();
		platformSpawnerSc.SessionEnd();
		InitializeSession();

		mainMenuPanel.SetActive(true);
		pauseMenuPanel.SetActive(false);
		sessionScoreText.SetActive(false);
		pauseButton.SetActive(false);
	}

	public void AddPoints() {
		scoreTrackerSc.AddPoints();
	}

	public void IncreaseSpeed() {
		ballControllerSc.IncreaseSpeed();
	}

	public void PlatformDestroyed() {
		platformSpawnerSc.PlatformDestroyed();
	}

	void InitializeSession() {
		ballControllerSc.InitializeBall();
		platformSpawnerSc.InitializeSession();
	}
}
