using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class MainMenuScript : MonoBehaviour {

	public GameObject tapGO;
	public GameObject logoGO;
	bool canPressToPlay = false;

	void Start ()
	{
		// hide the tap message at beginning
		tapGO.SetActive(false);



		// For sample code n more info, check out:
		// https://github.com/playgameservices/play-games-plugin-for-unity
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
	    PlayGamesPlatform.InitializeInstance(config);
	    PlayGamesPlatform.Activate();

		Social.Active.localUser.Authenticate((bool success) => {
	        // handle success or failure
	    });
	}

	void Update ()
	{
		if (canPressToPlay)
		{
			if (Input.anyKeyDown || Input.touchCount > 0)
			{
				tapGO.SetActive(false); // hide the message
				canPressToPlay = false; // don't let the player press anymore
				GetComponent<Animator>().Play("MenuTransitionOut"); // play transition out animation
			}
		}
	}

	public void OnShowLogo ()
	{
		// Triggered at end of logo animation's "TransitionIn"
		tapGO.SetActive(true);
		canPressToPlay = true;
	}

	public void OnHideLogo ()
	{
		// Triggered at end of logo animation's "TransitionOut"
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
	}
}
