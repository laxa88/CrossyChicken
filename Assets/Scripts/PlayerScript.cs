using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public bool isAlive = true;
	public float deathPositionZLandscape;
	public float deathPositionZPortrait;
	public GameObject prevLane;
	public GameObject currLane;
	public float prevX = 0f;
	public float currX = 0f;
	public float moveTime = 0.1f;

	Vector3 startPos = Vector3.zero;
	Vector3 endPos = Vector3.zero;
	public bool isStanding = true;
	public bool isMoving = false;
	float currTime = 0.0f;
	Animator animator;
	Collider collider;

	int currLaneCounter = 0; // keeps track of current lane
	int maxLaneCounter = 0; // keeps track of current hiscore

	// Keeps player from going out of LEFT or RIGHT bounds
	public float sideBound = 14f;

	void Awake () {

		if (Global.isResume)
		{
			currLaneCounter = Global.score;
			maxLaneCounter = Global.score;
		}

	}

	void Start () {

		// Instead of linking using a "public" variable, this is an
		// example of how we can use code to internally find the component.
		animator = GetComponentInChildren<Animator>();
		collider = GetComponentInChildren<Collider>();

		// Remember, if you dunno what the code is doing, just
		// highlight the name and press CTRL + ' or CMD + ' to open
		// the documentation. If it cannot open, check if you are
		// using MonoDevelop or whether you added the documentation
		// when you were installing Unity.

		// always start at same tile
		prevLane = currLane;

	}

	void Update () {

		if (isAlive == false) {

			// When the player is dead, check for SPACEBAR key so we can restart the game
			/*if (Input.GetKeyDown(KeyCode.Space) || 
				Input.touchCount > 0) {
				UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
			}*/

			// Stop the code here, since the player is dead
			return;
		}

		// The Update() function runs at about 60 frames per second (fps).
		// Every frame, we check the input for player's key press.

		if (isStanding == true)
		{
			if (currLane != null) {
				Vector3 newPos = currLane.transform.position;
				newPos.x = currX;
				newPos.y = Global.tileHeight;
				transform.position = newPos;
			}

			animator.Play("PlayerStand");
		}
		else if (isMoving == true)
		{
			animator.Play("PlayerMove");

			// Remember when we start moving, we set currTime = 0.0f
			// So, during every Update(), we add it by the delta time
			// (difference of time between each update), which should
			// be around 1/60 seconds.
			//
			// We divide the deltaTime with moveTime (0.2f) because
			// we want the player to reach the destination in 0.2 seconds.
			//
			// Example:
			// If moveTime = 1.0f, then the player will reach the destination in 1 second.
			// If moveTime = 2.0f, then the player will reach the destination in 2 seconds.
			// If moveTime = 0.5f, then the player will reach the destination in 0.5 seconds.
			currTime += Time.deltaTime / moveTime;

			// Get the newPos by calculating Lerp based on currTime.
			// Example: (we want the player to move 3 units in 1 second)
			// assume startPos = [0, 0, 0]
			// assume endPos = [3, 0, 0]
			// so...
			// when currTime = 0.0, then newPos = [0, 0, 0]
			// when currTime = 0.5, then newPos = [1.5, 0, 0]
			// when currTime = 1.0, then newPos = [3.0, 0, 0]
			startPos = prevLane.transform.position;
			startPos.x = prevX;
			endPos = currLane.transform.position;
			endPos.x = currX;
			Vector3 newPos = Vector3.Lerp(startPos, endPos, currTime);
			newPos.y = Global.tileHeight; // player is always above tile
			endPos.y = Global.tileHeight; // player is always above tile

			// Lerp values are calculated from 0.0 to 1.0, so the max is 1.0f
			// So, if the value is more or equal to 1.0f, that means the player
			// has reached the destination.
			if (currTime >= 1.0f)
			{
				// It is possible that the currTime becomes something
				// like "1.0123", so the newPos will not be [3.0, 0, 0] but rather
				// something like [3.0369, 0, 0], which is not accurate anymore.
				// So, when we reached the destination, we force the value to endPos.
				newPos = endPos;

				isMoving = false;
				isStanding = true;
			}

			// Finally, don't forget to update the player's actual position.
			transform.position = newPos;
		}

		// If the player's position is too far behind, kill the player.
		if (Screen.orientation == ScreenOrientation.Landscape)
		{
			if (transform.position.z < deathPositionZLandscape)
			{
				KillPlayer();
				FindObjectOfType<PlayServiceManagerScript>().ReportProgress("CgkIyp6nm4kREAIQAw", 100.0f);
			}
		}
		else if (Screen.orientation == ScreenOrientation.Portrait)
		{
			if (transform.position.z < deathPositionZPortrait)
			{
				KillPlayer();
				FindObjectOfType<PlayServiceManagerScript>().ReportProgress("CgkIyp6nm4kREAIQAw", 100.0f);
			}
		}

	}



	public void MovePlayer (string direction) {

		// When game starts, this value is false. When player
		// starts to move, then we start to scroll the map.
		Global.isScrolling = true;

		// "transform" contains the x/y/z position of the object
		// that this script is attached to. Since we attached this
		// script to the Player prefab, you can get the position
		// of the Player easily.

		Vector3 nextPos = transform.position;
		prevX = nextPos.x; // remember to save previous x position so we can lerp later
		prevLane = currLane; // remember to save previous lane before moving to new tile

		// Small bugfix - we only update the player
		// state (isStanding -> isMoving) if the player is able
		// to move to a new position.
		bool moved = false;

		// If the keypress is UP, the player is moving FORWARD by "moveDist"
		// If the keypress is DOWN, the player is moving BACKWARD by "moveDist"
		// If the keypress is LEFT, the player is moving LEFT by "moveDist"
		// If the keypress is RIGHT, the player is moving RIGHT by "moveDist"
		switch (direction)
		{

		// When moving forward or backward, check if next lane can be moved

		case "up":
			nextPos.z += Global.tileDistance;
			if (CheckNextLane(nextPos))
			{
				// If the lane in front can be moved, increase counter
				currLaneCounter += 1;

				// Update current hiscore and global hiscore
				if (currLaneCounter > maxLaneCounter)
				{
					Global.score = currLaneCounter;
					maxLaneCounter = currLaneCounter;
					if (maxLaneCounter > Global.hiscore)
					{
						Global.hiscore = maxLaneCounter;
						Global.saveScore();
					}
				}
				Global.updateScore();
				moved = true;
			}
		break;

		case "down":
			nextPos.z -= Global.tileDistance;
			if (CheckNextLane(nextPos))
			{
				// If the lane in front can be moved, decrease counter
				currLaneCounter -= 1;
				Global.updateScore();
				moved = true;
			}
		break;

		// When moving sideways, there's no need to check for lane anymore

		case "left":
			if (nextPos.x - Global.tileDistance >= -sideBound)
			{
				nextPos.x -= Global.tileDistance;
				moved = true;
			}
		break;

		case "right":
			if (nextPos.x + Global.tileDistance <= sideBound)
			{
				nextPos.x += Global.tileDistance;
				moved = true;
			}
		break;

		}

		currX = nextPos.x;

		// Remember to start the timer to 0.0, so that the
		// player can lerp correctly from 0.0 to 1.0
		if (moved)
		{
			currTime = 0.0f;

			// Remember to set the player's state so the
			// Update() function will run the correct code.
			isStanding = false;
			isMoving = true;
		}

		// Don't care if player can move or not,
		// just play the sound
		AudioManagerScript.playSound("hop");

	}

	bool CheckNextLane (Vector3 nextPos)
	{
		// Raycasts are a way for manually checking for collision.
		Ray ray = new Ray(nextPos, Vector3.down);
		RaycastHit hitData;

		// In the code below, e.g. if the player wants to move left,
		// a Ray is used at the nextPos and casted downward to check
		// if it hit a floor tile. If there is a hit, the data is
		// stored in "hitData", and the Physics.Raycast(...) function
		// will return "true" value.
		if (Physics.Raycast(ray, out hitData)) {
			currLane = hitData.collider.gameObject;
			return true;
		}
		else {
			return false;
		}
	}

	public void KillPlayer ()
	{
		// Just to be safe, don't run this code more than once
		if (!isAlive)
			return;

		// REPORT HISCORE!
		FindObjectOfType<PlayServiceManagerScript>().ReportScore("CgkIyp6nm4kREAIQBg", Global.hiscore);

		// When the player dies, pause the game scrolling
		Global.isScrolling = false;

		// disable collider so we don't repeatedly run this
		// code when the player hits other enemies.
		collider.enabled = false;

		isAlive = false;
		animator.Play("PlayerDie");

		// Play dying sound
		AudioManagerScript.playSound("smack");

		// Try to show ad after playing 3 times
		if (Global.playcount >= 3)
		{
			Global.playcount = 0;
			FindObjectOfType<AdManagerScript>().showInterstitial();
		}
		else
		{
			Global.playcount++;
		}

		FindObjectOfType<AdManagerScript>().showBanner();
		FindObjectOfType<UIManagerScript>().hideGameUI();
		FindObjectOfType<UIManagerScript>().showGameOverUI();
	}

	void OnTriggerEnter (Collider col)
	{
		// if the player hits anything that has the tag "enemy",
		// then the player will die.

		if (col.CompareTag("enemy")) {
			KillPlayer();

			FindObjectOfType<PlayServiceManagerScript>().ReportProgress("CgkIyp6nm4kREAIQAg", 100.0f);
		}
	}
}