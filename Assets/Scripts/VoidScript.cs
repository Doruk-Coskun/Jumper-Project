using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidScript : MonoBehaviour {

	public GameObject gameController;
	public GameObject ball;

	private GameController gameControllerSc;
	private Vector3 ballPosition;

	void Start() {
		gameControllerSc = gameController.GetComponent<GameController>();
	}

	void Update() {
		ballPosition = ball.transform.position;
		transform.position = new Vector3(ballPosition.x, -30f, ballPosition.z);
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Player") {
			gameControllerSc.GameOver();
		}
	}
}
