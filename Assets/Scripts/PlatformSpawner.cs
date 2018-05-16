using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour {

	public GameObject[] platforms;
	public GameObject collectible;
	public GameObject platformLocation;
	public GameObject gameController;

	public int platformMaxCount;
	private float initialPlatformDistance = 8;

	private int platformCount = 0;
	private int currentPlatformIndex = 0;
	private int totalPlatformCount = 0;
	private float platformDistance;
	private GameObject newPlatform;
	private GameObject newCollectible;
	private GameController gameControllerSc;

	void Awake() {
		gameControllerSc = gameController.GetComponent<GameController>();
	}

	void Update () {
		CreatePlatforms();
	}

	public void InitializeSession() {
		platformDistance = initialPlatformDistance;
		CreateStartPlatform();
		CreatePlatforms();
	}

	public void SessionEnd() {
		platformLocation.transform.position = Vector3.zero;

		//Clear all platforms and remaining collectibles.
		GameObject[] allChildren = new GameObject[transform.childCount];
		int i = 0;

		foreach (Transform child in transform) {
			allChildren[i] = child.gameObject;
			i += 1;
		}

		foreach (GameObject child in allChildren) {
			Destroy(child.gameObject);
			platformCount--;
		}
		platformCount = 0;
		currentPlatformIndex = 0;
		totalPlatformCount = 0;
	}

	public void PlatformDestroyed() {
		platformCount--;
		currentPlatformIndex++;
		if (currentPlatformIndex % 10 == 0) {
			gameControllerSc.IncreaseSpeed();
			platformDistance += 0.75f;
		}
	}

	void CreateStartPlatform() {
		newPlatform = Instantiate(platforms[0], platformLocation.transform);
		newPlatform.transform.parent = transform;
		platformCount++;
		totalPlatformCount++;

		//Rotate platformLocation so that it would be ready for the next platform.
		platformLocation.transform.position += new Vector3(Random.Range(-4, 6), 
			0, Random.Range(platformDistance + 2, platformDistance + 3));
	}

	void CreatePlatforms() {
		while (platformCount < platformMaxCount) {
			newPlatform = Instantiate(platforms[Random.Range(2, 6)], platformLocation.transform);
			newPlatform.transform.parent = transform;
			platformCount++;
			totalPlatformCount++;

			if (totalPlatformCount % 10 == 0) {
				Transform collectibleLocation = platformLocation.transform;
				collectibleLocation.transform.position += 
					new Vector3(Random.Range(-1, 1), 0f, Random.Range(-1, 1));
				newCollectible = Instantiate(collectible, collectibleLocation);
				newCollectible.transform.parent = transform;
			}

			//Rotate platformLocation so that it would be ready for the next platform.
			platformLocation.transform.position += new Vector3(Random.Range(-4, 6), 
				0, Random.Range(platformDistance, platformDistance + 2));
		}
	}
}
