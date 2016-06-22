using UnityEngine;
using System.Collections;

public class Global {

	public static float xBound = 15f;
	public static float zBound = 0f;
	public static float tileDistance = 3.0f;
	public static float tileHeight = 2f;
	public static bool isScrolling = false;

	public static int score = 0;
	public static int hiscore = 0;
	public static bool isResume = false;
	public static int playcount = 0;



	public static void saveScore ()
	{
		// Save the player hiscore data. We usually do
		// this when the hiscore is updated.

		PlayerPrefs.SetInt("hiscore", hiscore);
	}

	public static void loadScore ()
	{
		// Load the hiscore data from saved data. Usually
		// we do this when the game starts.

		hiscore = PlayerPrefs.GetInt("hiscore", 0);
	}

	public static void updateScore ()
	{
		// Note:
		// If you dunno where this script is, right-click the "UIManagerScript"
		// word below and click "Go to Declaration" to jump to the code.

		GameObject.FindObjectOfType<UIManagerScript>().updateUI();
	}
}
