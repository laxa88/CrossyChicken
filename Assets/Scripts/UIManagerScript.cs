using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class UIManagerScript : MonoBehaviour {

	// This script is only for updating the UI. You can actually
	// do all of this in other scripts like LaneManagerScript,
	// or even PlayerScript, but it is recommended to separate
	// your code into groups like this to make it easier to
	// manage your code in the future



	// don't forget to link these in the Inspector
	public Text scoreText;
	public Text hiscoreText;
	public GameObject gameUI;
	public GameObject gameOverUI;
	public GameObject continueButton;
	public Image buttonIcon;
	public Sprite pauseImage;
	public Sprite playImage;



	public void updateUI()
	{
		// score and hiscore are "int" type, so to display them as a string,
		// we can combine them with another string automatically convert
		// the int into strings

		scoreText.text = "" + Global.score;
		hiscoreText.text = "" + Global.hiscore;
	}

	public void hideGameUI ()
	{
		gameUI.SetActive(false);
	}

	public void showGameUI ()
	{
		gameUI.SetActive(true);
	}

	public void hideGameOverUI ()
	{
		gameOverUI.SetActive(false);
	}

	public void showGameOverUI ()
	{
		gameOverUI.SetActive(true);

		// Only show the button when the advertisement is available.
		if (Advertisement.IsReady())
			continueButton.SetActive(true);
		else
			continueButton.SetActive(false);
	}

	public void toggleButton (string buttonType)
	{
		if (buttonType == "play")
		{
			buttonIcon.sprite = playImage;
		}
		else if (buttonType == "pause")
		{
			buttonIcon.sprite = pauseImage;
		}
	}
}
