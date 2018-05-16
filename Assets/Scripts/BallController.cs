using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	public GameObject gameController;

	public float verticalSpeed;
	public float initialVerticalSpeed;
	public float horizontalSpeed;
	public float maxSpeed;
	public float jumpForce = 1000.0f;

	private Rigidbody rigidb;
	private GameController gameControllerSc;
	private bool canJump = true;

	private Vector2 touchOriginalPosition = -Vector2.one;

	void Start() {
		gameControllerSc = gameController.GetComponent<GameController>();
		rigidb = GetComponent<Rigidbody>();
	}

	void Update() {

		float horizontalMovement = 0;

		transform.position += Vector3.forward * verticalSpeed * Time.deltaTime;

		//Clears velocity in case the ball gains speed because of physics events.
		Vector3 zVelocity = rigidb.velocity - new Vector3(rigidb.velocity.x, 0, rigidb.velocity.z);
		rigidb.velocity = zVelocity;

		//Check if the game is running on the editor.
		#if UNITY_EDITOR

		horizontalMovement = Input.GetAxisRaw("Horizontal");

		if (Input.GetKeyDown("space") && canJump) {
			rigidb.AddForce(new Vector3 (0, jumpForce, 0));
		}

		//Check if the game is running on iOS or IPhone.
		#endif

		#if UNITY_IOS || UNITY_IPHONE

		int touchCount = Input.touchCount;

		if (touchCount > 0) {
			for (int i = 0; i < touchCount; i++) {
				//Store the first touch detected.
				Touch myTouch = Input.touches[i];
				Vector2 touchEndPosition;

				TouchPhase touchPhase = myTouch.phase;

				switch(touchPhase) {
				case TouchPhase.Stationary:
				case TouchPhase.Began:
					touchOriginalPosition = myTouch.position;
					Debug.Log("touchOriginalPosition: " + touchOriginalPosition);
					horizontalMovement = touchOriginalPosition.x  > Screen.width/2? 1: -1;
					break;
				case TouchPhase.Ended:
					touchEndPosition = myTouch.position;
					Debug.Log("touchEndPosition: " + touchOriginalPosition);
					float x = touchEndPosition.x - touchOriginalPosition.x;
					float y = touchEndPosition.y - touchOriginalPosition.y;
					if (Mathf.Abs(y) > Mathf.Abs(x) && canJump) {
						rigidb.AddForce(new Vector3 (0, jumpForce, 0));
						Debug.Log("Jumped");
					}
					break;
				case TouchPhase.Canceled:
					break;
				}
			}
		}

		#endif

		Vector3 movement = new Vector3 (horizontalMovement * horizontalSpeed, 0, 0);
		transform.position += movement * Time.deltaTime;
	}

	public void IncreaseSpeed() {
		verticalSpeed += 0.33f;
		if (verticalSpeed > maxSpeed) {
			verticalSpeed = maxSpeed;
		}
	}

	public void InitializeBall() {
		rigidb.velocity = Vector3.zero;
		rigidb.angularVelocity = Vector3.zero;
		transform.position = Vector3.zero;
		verticalSpeed = initialVerticalSpeed;
	}

	//BUG: When the corner of the platform hit, multiple points are awarded.
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Platform") {
			gameControllerSc.AddPoints();
			gameControllerSc.PlatformDestroyed();
			Destroy(other.gameObject, 4.0f);
		}
	}

	// Is this necessary? Maybe use OnCollisionEnter
	void OnCollisionStay(Collision other) {
		if (other.gameObject.tag == "Platform" 
			|| other.gameObject.tag == "Platform(Start)") {
			canJump = true;
		}
	}

	void OnCollisionExit(Collision other) {
		if (other.gameObject.tag == "Platform" 
			|| other.gameObject.tag == "Platform(Start)") {
			canJump = false;
		}
	}
		
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Collectible") {
			gameControllerSc.AddPoints();
			Destroy(other.gameObject);
		}
	}
		
}
