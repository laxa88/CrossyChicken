using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class PlayServiceManagerScript : MonoBehaviour {

	public void ShowLeaderboard ()
	{
		// This shows ALL leaderboard by default.
		// Social.ShowLeaderboardUI();

		// This will show specific leaderboard...
		PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIyp6nm4kREAIQBg");
	}

	public void ShowAchievement ()
	{
		Social.ShowAchievementsUI();
	}

	public void ReportProgress (string achievementId, float val)
	{
		// according to the documentation (https://github.com/playgameservices/play-games-plugin-for-unity)
		// if val == 0.0f, it means you REVEAL the achievement (if it is hidden), but you don't unlock it
		// if val == 100.0f, it means you REVEAL and UNLOCK the achievement.

	    Social.ReportProgress(achievementId, val, (bool success) => {
	    	// handle success or failure
	    });
	}

	public void ReportIncrement (string achievementId, int step)
	{
		// This is used for achievements that can be accumulated.
		// E.g. If you need to die 10 times before achievement is unlocked, you can
		// call ReportIncrement(achievemendId, 1) to increase it +1 everytime you die.
		// Don't forget to set the step counter in your play.google.com/apps/publish page

		PlayGamesPlatform.Instance.IncrementAchievement(achievementId, step, (bool success) => {
	    	// handle success or failure
	    });
	}

	public void ReportScore (string leaderboardId, int score)
	{
		// Just report hi-score whenever possible. If the score is higher,
		// it will update. If the score is lower, nothing will happen. so don't worry!

		Social.ReportScore(score, leaderboardId, (bool success) => {
			// handle success or failure
	    });
	}
}
