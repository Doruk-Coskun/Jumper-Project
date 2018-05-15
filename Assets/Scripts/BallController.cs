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

	private Vector2 touchOrigin = -Vector2.one;

	void Start() {
		gameControllerSc = gameController.GetComponent<GameController>();
		rigidb = GetComponent<Rigidbody>();
	}

	void Update() {
		if (Input.GetKeyDown("space") && canJump) {
			rigidb.AddForce(new Vector3 (0, jumpForce, 0));
		}
	}
		
	void FixedUpdate() {

		float horizontalMovement = 0;
		float verticalMovement = 0;

		transform.position += Vector3.forward * verticalSpeed * Time.deltaTime;

		//Clears velocity in case the ball gains speed because of physics events.
		Vector3 zVelocity = rigidb.velocity - new Vector3(rigidb.velocity.x, 0, rigidb.velocity.z);
		rigidb.velocity = zVelocity;

		//Check if the game is running on the editor.
		#if UNITY_EDITOR

		horizontalMovement = Input.GetAxisRaw("Horizontal");

		#endif
		//Check if the game is running on iOS or IPhone.
		#if UNITY_IOS || UNITY_IPHONE

		if (Input.touchCount > 0) {
			//Store the first touch detected.
			Touch myTouch = Input.touches[0];

		//Check if the phase of that touch equals Began
		if (myTouch.phase == TouchPhase.Began) {
			//If so, set touchOrigin to the position of that touch
			touchOrigin = myTouch.position;
		}

		//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
		else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {
			//Set touchEnd to equal the position of this touch
			Vector2 touchEnd = myTouch.position;

			//Calculate the difference between the beginning and end of the touch on the x axis.
			float x = touchEnd.x - touchOrigin.x;

			//Calculate the difference between the beginning and end of the touch on the y axis.
			float y = touchEnd.y - touchOrigin.y;

			//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
			touchOrigin.x = -1;

			//Check if the difference along the x axis is greater than the difference along the y axis.
			if (Mathf.Abs(x) > Mathf.Abs(y))
				//If x is greater than zero, set horizontal to 1, otherwise set it to -1
				horizontalMovement = x > 0 ? 1 : -1;
			else
				//If y is greater than zero, set horizontal to 1, otherwise set it to -1
				verticalMovement = y > 0 ? 1 : -1;
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
