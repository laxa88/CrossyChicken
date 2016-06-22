using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInputScript : MonoBehaviour {

	public PlayerScript player;
	Vector2 touchStartPos;
	Vector2 touchEndPos;
	bool isTouchDown = false;

	void Update () {

		// In this script, we only check for player's inputs!
		// If the player is DEAD or MOVING, then no need to check for inputs.

		// We have two checks here: One for keyboard, one for touch.

		// It is a good idea to have more than one input check, so that
		// you can easily debug on different device (keyboard for desktop,
		// touch for mobile device).



		// Don't check for player movement if player is dead
		if (!player.isAlive)
			return;

		// If player is moving, don't check for input
		if (player.isMoving)
			return;

		CheckKeyboardInput();

		CheckTouchInput();

	}

	void CheckKeyboardInput ()
	{
		// This code is cut-and-pasted from PlayerScript

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			player.MovePlayer("up");
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			player.MovePlayer("down");
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			player.MovePlayer("left");
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			player.MovePlayer("right");
		}
	}

	void CheckTouchInput ()
	{
		// What happens here?:
		// 1) If touchCount is more than zero (there is one or more fingers touching the screen),
		// get the first finger's position data and store to touchStartPos
		// 2) While the finger is still touching the screen, store all latest finger position
		// data into touchEndPos.
		// 3) When there is no more finger touching the screen, then we compare
		// the touchStartPos and touchEndPos and move the player based on the direction
		// of the swipe.
		// 4) If the swipe movement is too small, don't do anything. This helps prevent
		// players from moving if they pressed the screen by accident.

		if (Input.touchCount > 0)
		{
			if (!isTouchDown)
			{
				// If there is one or more touch input detected, start tracking
				// the first touch's position.
				touchStartPos = Input.touches[0].position;

				// Flag the bool so we know the input is being tracked
				isTouchDown = true;
			}
			else
			{
				// store the latest position
				touchEndPos = Input.touches[0].position;
			}
		}
		else
		{
			// NOTE:
			// we cannot get touchEndPos during the finger release, because
			// when the finger is released, the Input.touches[0] will not
			// have anymore value. That is why we get touchEndPos above instead.

			// If there is no touch input detected, and we were tracking
			// the input just now, then it means we released the finger.
			if (isTouchDown)
			{
				isTouchDown = false;

				// delta is "the difference between two values"
				Vector2 delta = touchEndPos - touchStartPos;

				// magnitude is the distance of the delta. If the distance
				// is too short, maybe it means the player accidentally
				// tapped the screen instead of swiping.
				if (delta.magnitude > 10f)
				{
					// Mathf.Abs() is a function that forces the value to POSITIVE
					float dx = Mathf.Abs(delta.x);
					float dy = Mathf.Abs(delta.y);

					// if delta.x is bigger than delta.y, that means we swipe LEFT or RIGHT
					// if delta.y is bigger than delta.x, that means we swipe UP or DOWN
					if (dx > dy) {
						if (delta.x < 0) // if x is negative, that means player swiped left
							player.MovePlayer("left");
						else
							player.MovePlayer("right");
					}
					else {
						if (delta.y < 0) // if y is negative, that means player swiped down
							player.MovePlayer("down");
						else
							player.MovePlayer("up");
					}
				}
			}
		}
	}
}
