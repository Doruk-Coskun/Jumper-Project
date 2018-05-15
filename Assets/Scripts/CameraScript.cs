using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public GameObject ball;

	private Vector3 ballPosition;
	private Vector3 distance;
	private Vector3 offset;

	void Start() {
		ballPosition = ball.transform.position;
		distance = ballPosition - transform.position;
	}

	// Update is called once per frame
	void Update () {
		ballPosition = ball.transform.position;
		offset = new Vector3(ballPosition.x - distance.x, transform.position.y, 
			ballPosition.z - distance.z);
		transform.position = offset;
	}
}
