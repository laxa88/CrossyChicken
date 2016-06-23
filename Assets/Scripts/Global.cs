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
	public static bool adsRemoved = false; // this is static so it can be used across the whole game



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

	public static void saveAdData ()
	{
		// PlayerPrefs can only store Int, Float or String
		// So, convert to bool manually.
		int v = (adsRemoved) ? 1 : 0; // 1 = true, 0 = false
		PlayerPrefs.SetInt("adremoved", v);
	}

	public static void loadAdData ()
	{
		// PlayerPrefs can only store Int, Float or String
		// So, convert to bool manually.
		int v = PlayerPrefs.GetInt("adremoved", 0);
		adsRemoved = (v == 1) ? true : false;
	}
}
