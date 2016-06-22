using UnityEngine;
using System.Collections;
using System.Collections.Generic; // don't forget to include this, which is needed for Lists.
using UnityEngine.Advertisements;

public class LaneManagerScript : MonoBehaviour {

	// public Material roadMaterial;
	// public Material grassMaterial;
	public GameObject lanePrefab;
	public GameObject spawnerPrefab;
	public int laneCount = 20;
	public float scrollSpeed = 2.5f; // scroll at 2.5 unit per second
	public int playerStartLane = 5; // player starts on lane 6 (remember, index starts from 0)

	// this is the original scroll speed when the player
	// is not too far in front. The value is based on the
	// starting value of scrollSpeed.
	public float oriScrollSpeed;

	// if the player's z-position is more than this value,
	// the camera will scroll faster.
	public float forwardBound = 10f;

	List<GameObject> lanes;

	bool isPaused = false;

	void Awake () {

		// Make sure the global score is reset whenever playing a new game
		if (!Global.isResume)
		{
			Global.score = 0;
		}

	}

	void Start () {

		// CONGRATS, YOU PLAYED THE GAME ONCE!
		FindObjectOfType<PlayServiceManagerScript>().ReportProgress("CgkIyp6nm4kREAIQAQ", 100.0f);

		FindObjectOfType<AdManagerScript>().hideBanner(); // hide banner at beginning. only show when game over.
		FindObjectOfType<UIManagerScript>().showGameUI();
		FindObjectOfType<UIManagerScript>().hideGameOverUI();

		// At start of game, reset this value
		Global.isResume = false;

		// At the beginning of the game, play background music!
		AudioManagerScript.playMusic("maingame");

		// At the beginning of the game, remember to set the original scroll speed
		oriScrollSpeed = scrollSpeed;

		// When the game starts, don't forget to load the previous hiscore
		// and update the UI!
		Global.loadScore();
		Global.updateScore();

		lanes = new List<GameObject>();

		float currZ = 0.0f;
		for (int lane = 0; lane < laneCount; lane++)
		{
			// Create the new lane and set the position
			GameObject newLane = GameObject.Instantiate(lanePrefab);
			newLane.transform.position = new Vector3(0, 0, currZ);

			// Attach this new lane to this TileManagerScript's GameObject.
			// You can use this technique like a "folder" for your Hierarchy
			// view in Unity, so it's easier to manage groups of objects.
			newLane.transform.SetParent(transform);

			// Add the new lane to the list so we can scroll it later
			lanes.Add(newLane);

			// Add the player if this the correct lane
			if (lane == playerStartLane) {
				GameObject.Find("Player").GetComponent<PlayerScript>().currLane = newLane;
				Vector3 playerPos = GameObject.Find("Player").transform.position;
				playerPos.x = newLane.transform.position.x;
				playerPos.z = newLane.transform.position.z;
			}

			if (lane > playerStartLane)
			{
				// Try to create spawner, and update lane type
				TryCreateSpawner(newLane);
			}
			else
			{
				// default to grass lane type
				newLane.GetComponent<LaneScript>().setLaneType("grass");
			}

			// Move Z position to next lane
			currZ += Global.tileDistance;
		}

	}

	void Update () {

		if (!Global.isScrolling)
			return;

		// Get the player's position (note: this is not the best
		// code, but it is the easiest to write)
		Vector3 playerPos = FindObjectOfType<PlayerScript>().gameObject.transform.position;

		// Every frame, reset the scroll speed
		scrollSpeed = oriScrollSpeed;

		// If the player position is more than the bound, increase scroll speed
		if (playerPos.z > forwardBound)
		{
			scrollSpeed = scrollSpeed + (playerPos.z - forwardBound);
		}

		// the distance that is moved between each frame during the 60fps.
		float distance = Time.deltaTime * scrollSpeed;
		bool canCreateSpawner = false;
		GameObject canCreateSpawnerAtLane = null;

		foreach (GameObject lane in lanes)
		{
			Vector3 currPos = lane.transform.position;

			// remember, we use negative distance because it is scrolling backwards.
			currPos.z -= distance;

			// if the tile scrolled too far away, cycle it to the front
			if (currPos.z < -Global.zBound) {
				currPos.z += Global.tileDistance * laneCount;
				canCreateSpawner = true;
				canCreateSpawnerAtLane = lane;
			}

			// remember to set the position.
			lane.transform.position = currPos;
		}

		if (canCreateSpawner == true) {
			TryCreateSpawner(canCreateSpawnerAtLane);
		}

	}

	void TryCreateSpawner (GameObject laneToCreateSpawner)
	{
		if (Random.Range(0.0f, 1.0f) >= 0.5f) {
			// Create the spawner...
			GameObject spawner = GameObject.Instantiate(spawnerPrefab);
			spawner.transform.SetParent(transform);

			// Enemy spawners can appear on LEFT or RIGHT side of the map.
			int enemyDirection = Random.Range(0, 2); // returns random number 0 or 1.
			spawner.GetComponent<SpawnerScript>().InitSpawner(laneToCreateSpawner, enemyDirection);

			laneToCreateSpawner.GetComponent<LaneScript>().setLaneType("road");
		}
		else
		{
			laneToCreateSpawner.GetComponent<LaneScript>().setLaneType("grass");
		}
	}

	public void PauseGame ()
	{
		// If game is not paused, pause the game.
		// If game is paused, unpause the game.

		if (!isPaused)
		{
			isPaused = true;
			Time.timeScale = 0f;
			FindObjectOfType<UIManagerScript>().toggleButton("play");
		}
		else
		{
			isPaused = false;
			Time.timeScale = 1f;
			FindObjectOfType<UIManagerScript>().toggleButton("pause");
		}
	}

	public void ReloadGame ()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");

		// Report progress every time player plays
		FindObjectOfType<PlayServiceManagerScript>().ReportIncrement("CgkIyp6nm4kREAIQBw", 1);
	}

	public void RevivePlayerAndResumeGame ()
	{
		// Show the unity video ad with an extra option
		// to handle "on complete" event

		ShowOptions options = new ShowOptions();
		options.resultCallback = OnShowAd;
        Advertisement.Show(null, options);
	}

	void OnShowAd (ShowResult result)
	{
		if (result == ShowResult.Failed)
		{
			// Refresh the gameover UI and do nothing
			FindObjectOfType<UIManagerScript>().showGameOverUI();
		}
		else
		{
			// If the ad didn't fail, that means the player
			// watched the ad successfully. So resume the game.

			Global.isResume = true;
			UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");

			// If player watched the ad, unlock achievement!
			FindObjectOfType<PlayServiceManagerScript>().ReportProgress("CgkIyp6nm4kREAIQBQ", 100.0f);

			// Report progress every time player plays
			FindObjectOfType<PlayServiceManagerScript>().ReportIncrement("CgkIyp6nm4kREAIQBw", 1);
		}
	}
}
