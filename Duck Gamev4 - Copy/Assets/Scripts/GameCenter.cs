using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

//Class used to implement Apple's game center within the game in order to save high scores.
public class GameCenter : MonoBehaviour {

	public long scoreLong;
	public int socialCount;

	// Use this for initialization
	void Start () 
	{
		Social.localUser.Authenticate (ProcessAuthentication);
	}

	void ProcessAuthentication (bool success) 
	{
		if (success) {
			Debug.Log ("Authenticated, checking achievements");

			// Request loaded achievements, and register a callback for processing them
			Social.LoadAchievements (ProcessLoadedAchievements);
		}
		else
			Debug.Log ("Failed to authenticate");
	}

	void ProcessLoadedAchievements (IAchievement[] achievements) 
	{
		if (achievements.Length == 0)
			Debug.Log ("Error: no achievements found");
		else
			Debug.Log ("Got " + achievements.Length + " achievements");

		// You can also call into the functions like this
		Social.ReportProgress ("Achievement01", 100.0, result => {
			if (result)
				Debug.Log ("Successfully reported achievement progress");
			else
				Debug.Log ("Failed to report achievement");
		});
	}

	public void RetrieveHighScores ()
	{
		Social.ShowLeaderboardUI();
	}




	// Update is called once per frame
	void Update () 
	{
		//Sets game center high score to be the high score recorded in the game.
		if (GameControl.instance.gameOver == true && socialCount < 1)
		{
			scoreLong = GlobalControl.Instance.highScore;
			Social.ReportScore (scoreLong, "goldgorilla", HighScoreCheck);
			socialCount++;
		}
		if (GameControl.instance.gameOver == false)
		{
			socialCount = 0;
		}
	}

	static void HighScoreCheck(bool result)
	{
		if (result)
			Debug.Log ("score submission successful");
		else
			Debug.Log ("score submission failed");
	}
}
