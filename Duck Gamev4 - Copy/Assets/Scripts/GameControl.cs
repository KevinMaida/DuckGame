using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour 
{
	//This class is a singleton used to manage most game mechanics and UI elements.
	public static GameControl instance;         //A reference to our game control script so we can access it statically.
	public Text scoreText;                      //A reference to the UI text component that displays the player's score.
	public Text highScoreText;
	public GameObject gameOvertext;             //A reference to the object that displays the text which appears when the player dies.
	public Button Options;
	public Button PurchasedMusicToggle;
	public Text DefaultSong2Text;
	public Text Song2MustPurchaseText;
	public Button DefaultMusicToggle;
	public Text DefaultMustPurchaseText;
	public Text DefaultSongText;
	public Button MusicToggleButton;
	public Button SFXToggleButton;
	public Button retryButton;
	public Button goBackButton;
	public GameObject Feathers0;
	public GameObject Feathers1;
	public GameObject Feathers2;
	public GameObject Feathers3;
	public GameObject startImage;
	public GameObject startArt;
	public GameObject player;
	public GameObject One;
	public GameObject Two;
	public GameObject Three;
	public GameObject continueTexts;
	public GameObject smallFeatherAnim;
	public GameObject mediumFeatherAnim;
	public GameObject largeFeatherAnim;
	public GameObject lightningAnim;
	public GameObject RedImage;
	public GameObject GameOverAnim;
	public GameObject DeathHands;
	public GameObject MainMusic;
	public GameObject GameMusic1;
	public GameObject GameMusic2;
	public GameObject GameMusic3;
	public GameObject GeeseIdle;
	public GameObject Quack;
	public GameObject gameOverMusic;
	public GameObject onePointSFX;
	public GameObject yellowPointsSFX;
	public GameObject orangePointsSFX;
	public GameObject redPointsSFX;
	public GameObject lightningSFX;
	public GameObject minusPointsSFX;
	public GameObject failureSFX;
	public GameObject alert;
	public GameObject highScoreStuff;
	public GameObject trophyScores;
	public GameObject wood;
	public GameObject bronze;
	public GameObject silver;
	public GameObject gold;
	public GameObject pro;
	public GameObject titleYellow;
	public GameObject titleBlack;
	public GameObject hand1;
	public GameObject arrow;
	public GameObject tapText1;
	public GameObject tapText2;
	public Button RestoreButton;
	public Button PurchaseButton;
	public Button HighScoreButton;
	public Sprite OffSprite;
	public Sprite OnSprite;
	public Sprite GraySprite;
	public int whichFeathers;
	public int playerSpeed;
	public bool tempBool;
	public int scoreDeathCount;

	public int score = 0;                      //The player's score.
	public int adCount;
	public int almostDeadCount;
	public float currentVolume;
	public bool gameOver = false;               //Is the game over?
	public float scrollSpeed;
	public bool ducked;
	public bool valid;
	public bool stayValid;
	public bool startScreen = false;
	public bool startArtBool = false;
	public bool soundOptionsBool;

	//This method is called even before the start() method so that important
	//game variables are initialized immediately.
	void Awake()
	{
		//If we don't currently have a game control...
		if (instance == null)
			//...set this one to be it...
			instance = this;
		//...otherwise...
		else if(instance != this)
			//...destroy this one because it is a duplicate.
			Destroy (gameObject);
		score = 0;
		ducked = false;
		valid = false;
		stayValid = false;
		scrollSpeed = -4f;
		Time.timeScale = 0;
		//Determines whether the music toggle button image in the options menu is turned
		//on or off.
		if (GlobalControl.Instance.musicOn == false) {
			MusicToggleButton.image.sprite = OffSprite;
		} else {
			MusicToggleButton.image.sprite = OnSprite;
		}
		//Determines whether the sound effects toggle button image in the options menu
		//is turned on or off.
		if (GlobalControl.Instance.SFXOn == false) {
			SFXToggleButton.image.sprite = OffSprite;
		} else {
			SFXToggleButton.image.sprite = OnSprite;
		}
		//Determines whether the song toggle buttons images in the options menu
		//are turned on or off.
		if (GlobalControl.Instance.purchasedMusicBool == true)
		{
			PurchasedMusicToggle.image.sprite = OnSprite;
		}
		else{
			PurchasedMusicToggle.image.sprite = OffSprite;
		}
		////Determines whether the restore purchase button image in the options menu
		//is turned on or off.
		if (PlayerPrefs.GetInt("gamePurchased") == 0)
		{
			RestoreButton.image.sprite = OffSprite;
		}
		//The starting screen shouldn't show if the player clicks the restart button.
		if (GlobalControl.Instance.restart == true)
		{
			GlobalControl.Instance.iphoneXModeCount = 0;
			startArt.SetActive (false);
			startImage.SetActive (false);
		}
		//If restart is false this means this is the first time this script is running, so
		//the starting screen needs to be activated.
		if (GlobalControl.Instance.restart == false)
		{
			startArtBool = true;
			//the "startScreen" is the second screen where it shows the player
			//how to play the game.
			startScreen = false;
			startImage.SetActive (false);
			player.SetActive (false);
			//Makes sure enemies don't spawn until player clicks through starting screens
			//and gets to the actual game screen.
			gameObject.GetComponent<EnemyPool>().enabled = false;
			StopCoroutine ("tapToContinue");
			StartCoroutine ("tapToContinue");
		}
		currentVolume = MainMusic.GetComponent<AudioSource> ().volume;
	}
	//Determines what to do every frame of the game.
	void Update()
	{
		//Determines what to do if game was restarted and an ad hasn't been shown.
		if (GlobalControl.Instance.restart == true && adCount == 0)
		{
			//If player is playing the free version of the game, set
			//these variables to false and pause time.
			if (PlayerPrefs.GetInt("gamePurchased") == 0)
			{
				player.SetActive(false);
				GameMusic1.SetActive(false);
				GameMusic3.SetActive(false);
				Time.timeScale = 0;
				adCount++;
				GlobalControl.Instance.adDone = false;
			}
			//Checks if full game was purchased.
			if (PlayerPrefs.GetInt("gamePurchased") == 1)
			{
				player.SetActive(true);
				if (GlobalControl.Instance.purchasedMusicBool == true)
				{
					GameMusic3.SetActive(true);
				}
				else if (GlobalControl.Instance.purchasedMusicBool == false)
				{
					GameMusic1.SetActive(true);
				}
				adCount++;
			}

		}
		//If this isn't a restart and we're at the very first screen of the game and there is no ad running...
		if (GlobalControl.Instance.restart == false && startArtBool == true && startScreen == false && adCount == 0)
		{
			//If this is the free version of the game...
			if (PlayerPrefs.GetInt ("gamePurchased") == 0) 
			{
				//turn off game music since we're not on the main gameplay screen yet
				MainMusic.GetComponent<AudioSource> ().Pause();
				//Increment ad counter. This is just to move things along and act as if an ad finished playing.
				//This is confusing but it is this way because initially the game was meant to play ads not just when
				//the player died but at various points of the game like when first launching it. 
				//Instead of going through everything to remove this variable from certain places, I 
				//decided to just work with it so that I could complete the project sooner.
				adCount++;
				GlobalControl.Instance.adDone = false;
			}

			if (PlayerPrefs.GetInt("gamePurchased") == 1)
			{
				adCount++;
			}
		}
		//Next step game takes after adCount has been incremented by 1.
		if (GlobalControl.Instance.restart == false && startArtBool == true && startScreen == false && adCount == 1)
		{

			//gamePurchased value of 0 means free version of game.
			if (PlayerPrefs.GetInt("gamePurchased") == 0)
			{
				//this logic is from old game version where ads were played everywhere including at
				//the beginning of the game. I kept it just to keep things moving along since I was
				//already used to it.
				if (GlobalControl.Instance.adDone == false)
				{
					MainMusic.GetComponent<AudioSource>().UnPause();
					//If player touches screen, increment the adCount in order to move the game to the next screen.
					if (Input.GetMouseButtonDown(0))
					{
						adCount++;
					}
				}
			}
			//value of 1 means purchased version of game.
			if (PlayerPrefs.GetInt("gamePurchased") == 1)
			{
				if (Input.GetMouseButtonDown(0))
				{
					adCount++;
				}
			}
		}
		//Next step game takes after adCount has been incremented by 1.
		if (GlobalControl.Instance.restart == false && startArtBool == false && startScreen == true && adCount == 2)
		{

			//Using same old game version logic as above where we're assuming an ad has to play at
			//various points in the game and not just when the player dies.
			if (PlayerPrefs.GetInt ("gamePurchased") == 0)
			{
				MainMusic.GetComponent<AudioSource> ().Pause();
				adCount++;
				GlobalControl.Instance.adDone = false;
			}
			if (PlayerPrefs.GetInt ("gamePurchased") == 1) 
			{
				adCount++;
			}
		}
		//Next step game takes after adCount has been incremented by 1.
		if (GlobalControl.Instance.restart == false && startArtBool == false && startScreen == true && adCount == 3)
		{
			//Using same old game version logic as above.
			if (PlayerPrefs.GetInt("gamePurchased") == 0)
			{
				if (GlobalControl.Instance.adDone == false)
				{
					MainMusic.GetComponent<AudioSource>().UnPause();
					if (Input.GetMouseButtonDown(0))
					{
						adCount++;
					}
				}
			}
			if (PlayerPrefs.GetInt("gamePurchased") == 1)
			{
				if (Input.GetMouseButtonDown(0))
				{
					adCount++;
				}
			}
		}
		//This nested if statement will play a quack sound effect when the
		//player touches the screen.
		if (startArtBool == true || startScreen == true)
		{
			if (Input.GetMouseButtonDown(0))
			{
				StopCoroutine ("QuackSound");
				StartCoroutine ("QuackSound");
			}
		}
		//Using same old game versino logic as above assuming an ad will play. Still moves game along by incrementing adCount.
		if (startScreen == false && adCount == 4 && PlayerPrefs.GetInt("gamePurchased") == 0)
		{
			if (GlobalControl.Instance.adDone == false)
			{
				adCount++;
			}
		}
		//Once player is in one of the game over screens, every touch will trigger a 
		//quack sound effect.
		if (gameOver == true && Input.GetMouseButtonDown(0))
		{
			StopCoroutine ("QuackSound");
			StartCoroutine ("QuackSound");
		}
		//When player is 2 score points away from death (-10 score means game over)
		//then this will activate a warning UI animation on screen for the player.
		if (score <= -8 && almostDeadCount < 1) 
		{
			StopCoroutine ("almostDead");
			StartCoroutine ("almostDead");
			almostDeadCount++;
		}
		//If player reaches score of -10, it's gameover.
		if (score <= -10 && scoreDeathCount < 1)
		{
			DuckDied ();
			scoreDeathCount++;
		}
		//This statement uses the old game version logic & assumes an ad will play when the player
		//touches the restart button.
		if (GlobalControl.Instance.restart == true)
		{
			MainMusic.SetActive (false);
		}
		//Below: turns off start art screen and sets start instructions screen "startScreen" on
		if (GlobalControl.Instance.restart == false && startArtBool == true && adCount == 2)
		{
			continueTexts.SetActive (false);
			startArtBool = false;
			startArt.SetActive (false);
			startScreen = true;
			startImage.SetActive (true);
			//activates the tap to continue UI on screen.
			StopCoroutine ("tapToContinue");
			StartCoroutine ("tapToContinue");
		}
		//This whole if statement uses the old game version logic and assumes an ad will play.
		//Only thing of importance that happens here is adCount is incremented to move things along.
		if (GlobalControl.Instance.restart == false && startScreen == true && adCount == 4 && PlayerPrefs.GetInt("gamePurchased") == 0)
		{
			//If sound effects are turned on in settings, activate goose enemies.
			if (GlobalControl.Instance.SFXOn == true)
			{
				GeeseIdle.SetActive (true);
			}
			GeeseIdle.GetComponent<AudioSource> ().Pause();
			StopCoroutine ("tapToContinue");
			StopCoroutine ("VolumeFadeOut");
			StartCoroutine ("VolumeFadeOut");
			MainMusic.SetActive (false);
			continueTexts.SetActive (false);
			startScreen = false;
			startImage.SetActive (false);
			Time.timeScale = 0;
			GlobalControl.Instance.adDone = false;
		}
		//Using same old game version logic as above, this time for the purchased game version.
		if (GlobalControl.Instance.restart == false && startScreen == true && adCount == 4 && PlayerPrefs.GetInt ("gamePurchased") == 1)
		{
			if (GlobalControl.Instance.SFXOn == true) 
			{
				GeeseIdle.SetActive (true);
			}
			StopCoroutine ("tapToContinue");
			StopCoroutine ("VolumeFadeOut");
			StartCoroutine ("VolumeFadeOut");
			startScreen = false;
			startImage.SetActive (false);
			continueTexts.SetActive (false);
			MainMusic.SetActive (false);
			player.SetActive (true);
			gameObject.GetComponent<EnemyPool> ().enabled = true;
			Time.timeScale = 1;
			adCount += 2;
		}
		//Logic for transitioning between instructions screen to gameplay screen.
		if (GlobalControl.Instance.restart == false && adCount == 5 && PlayerPrefs.GetInt("gamePurchased") == 0)
		{
			GeeseIdle.GetComponent<AudioSource> ().UnPause();
			player.SetActive (true);
			gameObject.GetComponent<EnemyPool>().enabled = true;
			Time.timeScale = 1;
			adCount++;
		}
		//This section decides what music to play based on if the game version is free or purchased.
		if (GlobalControl.Instance.restart == false && startScreen == false && startArtBool == false && GameMusic2.activeSelf == false && adCount == 6)
		{
			if (PlayerPrefs.GetInt ("gamePurchased") == 0 && GlobalControl.Instance.musicOn == true && GlobalControl.Instance.purchasedMusicBool == false) 
			{
				GameMusic1.SetActive (true);
			}
			if (PlayerPrefs.GetInt ("gamePurchased") == 1 && GlobalControl.Instance.musicOn == true)
			{
				if (GlobalControl.Instance.purchasedMusicBool == true)
				{
					GameMusic3.SetActive (true);
				}
				if (GlobalControl.Instance.purchasedMusicBool == false)
				{
					GameMusic1.SetActive (true);
				}
			}
			adCount++;
		}
		//This section decides what game music to play when the player restarts.
		if (GlobalControl.Instance.restart == true && adCount == 2)
		{
			if (PlayerPrefs.GetInt("gamePurchased") == 0 && GlobalControl.Instance.musicOn == true && GlobalControl.Instance.purchasedMusicBool == false)
			{
				GameMusic1.SetActive (true);
			}
			if (PlayerPrefs.GetInt("gamePurchased") == 1 && GlobalControl.Instance.musicOn == true)
			{
				if (GlobalControl.Instance.purchasedMusicBool == true)
				{
					GameMusic3.SetActive (true);
				}
				if (GlobalControl.Instance.purchasedMusicBool == false)
				{
					GameMusic1.SetActive (true);
				}
			}
		}
		//This section turns off any game music currently playing so that game music 2 plays.
		//GameMusic2 is activated whenever the player taps the screen to swim faster.
		if (gameOver == false && startArtBool == false && startScreen == false && GameMusic2.activeSelf == true)
		{
			GameMusic1.SetActive(false);
			GameMusic3.SetActive(false);
		}
		//If player isn't swimming fast, then continue playing normal game music depending on 
		//whether this is free or purchased game version and what options the player set for music.
		else if (gameOver == false && startArtBool == false && startScreen == false && GameMusic2.activeSelf == false)
		{
			if (PlayerPrefs.GetInt("gamePurchased") == 0 && GlobalControl.Instance.musicOn == true && GlobalControl.Instance.purchasedMusicBool == false)
			{
				GameMusic1.SetActive(true);
			}
			if (PlayerPrefs.GetInt("gamePurchased") == 1 && GlobalControl.Instance.musicOn == true)
			{
				if (GlobalControl.Instance.purchasedMusicBool == true)
				{
					GameMusic3.SetActive(true);
				}
				if (GlobalControl.Instance.purchasedMusicBool == false)
				{
					GameMusic1.SetActive(true);
				}
			}
		}
		//If it's game over, turn off game music.
		if (gameOver == true)
		{
			GameMusic1.SetActive (false);
			GameMusic3.SetActive (false);
		}
		//This section checks if after restarting, the ad has finished playing in order to 
		//activate everything for the gameplay screen again.
		if (GlobalControl.Instance.restart == true && adCount == 1 && PlayerPrefs.GetInt("gamePurchased") == 0)
		{
			if (GlobalControl.Instance.adDone == false)
			{ 
				player.SetActive(true);
				Time.timeScale = 1;
				if (GlobalControl.Instance.SFXOn == true)
				{
					GeeseIdle.SetActive(true);
				}
				adCount++;
			}
		}
		//This section does the same as above but for purchased game version.
		if (GlobalControl.Instance.restart == true && adCount == 1 && PlayerPrefs.GetInt("gamePurchased") == 1)
		{
			Time.timeScale = 1;
			if (GlobalControl.Instance.SFXOn == true) 
			{
				GeeseIdle.SetActive (true);
			}
			adCount++;
		}
	}
	//Method called when player taps the restart button.
	public void Restart()
	{
		GlobalControl.Instance.restart = true;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	//Method called when player scores points. Different scenarios lead to different score increments.
	public void DuckScored()
	{
		//The bird can't score if the game is over.
		if (gameOver)   
			return;
		//If the game is not over, increase the score...
		if (valid == true)
		{

			if (playerSpeed == 3)
			{
				One.SetActive (false);
				Two.SetActive (false);
				score += 1;
				StopCoroutine ("extraPoints");
				StartCoroutine ("extraPoints");
				playerSpeed = 0;
			}
			else if (playerSpeed == 2)
			{
				One.SetActive (false);
				Three.SetActive (false);
				score += 1;
				StopCoroutine ("extraPoints");
				StartCoroutine ("extraPoints");
				playerSpeed = 0;
			}
			else if (playerSpeed == 1)
			{
				Two.SetActive (false);
				Three.SetActive (false);
				score += 1;
				StopCoroutine ("extraPoints");
				StartCoroutine ("extraPoints");
				playerSpeed = 0;
			}
			else if ( playerSpeed == 0)
			{
				One.SetActive (false);
				Two.SetActive (false);
				Three.SetActive (false);
				score += 1;
				if (GlobalControl.Instance.SFXOn == true)
				{
					onePointSFX.SetActive (true);
				}
			}
			stayValid = false;
			StopCoroutine ("waitToScore");
		}

		if (valid == false)
		{
			player.GetComponent<SpriteRenderer>().color = Color.white;
			if (GlobalControl.Instance.SFXOn == true) 
			{
				minusPointsSFX.SetActive (true);
				failureSFX.SetActive (true);
			}
			score -= 2;
		}

		//...and adjust the score text.
		scoreText.text = score.ToString();
	}
	//Method called when player dies in game.
	public void DuckDied()
	{
		//Set the game to be over.
		GeeseIdle.SetActive(false);
		if (GlobalControl.Instance.musicOn == true) 
		{
			gameOverMusic.SetActive (true);
		}
		StopCoroutine ("QuackSound");
		StartCoroutine ("QuackSound");
		DeathHands.SetActive(true);
		StopCoroutine ("BlackScreenWait");
		StartCoroutine ("BlackScreenWait");
		gameOver = true;
		GlobalControl.Instance.iphoneXModeCount = 0;
		GameMusic2.SetActive(false);
		player.SetActive (false);
		Feathers0.SetActive (false);
		Feathers1.SetActive (false);
		Feathers2.SetActive (false);
		Feathers3.SetActive (false);
		//Activate the game over text.
		//gameOvertext.SetActive (true);
		StopCoroutine("GameOverWait");
		StartCoroutine ("GameOverWait");

	}
	//Obsolete coroutine. Was used in previous game version.
	public IEnumerator waitToScore ()
	{
		yield return new WaitForSeconds (2f);
		stayValid = true;
		DuckScored ();
	}
	//Coroutine used to determine how many extra points to give player.
	//Depends on speed of player and whether player ducked successfully.
	public IEnumerator extraPoints ()
	{
		if (playerSpeed == 1)
		{
			One.SetActive (true);
			if (GlobalControl.Instance.SFXOn == true)
			{
				yellowPointsSFX.SetActive (true);
			}
			smallFeatherAnim.SetActive (true);
			yield return new WaitForSecondsRealtime (1f);
			Color scoreTemp = scoreText.color;
			scoreTemp.r = 247/255f;
			scoreTemp.g = 220/255f;
			scoreTemp.b = 114/255f;
			scoreTemp.a = 1.0f;
			scoreText.color = scoreTemp;
			score += 1;
			scoreText.text = score.ToString();
			smallFeatherAnim.SetActive (false);
			while ((scoreTemp.r + scoreTemp.g + scoreTemp.b) > 0f)
			{
				float tempR = scoreTemp.r;
				tempR -= 0.1f;
				if (tempR <= 0f) 
				{
					scoreTemp.r = 0f;
				} else if (tempR > 0f)
				{
					scoreTemp.r -= 0.1f;
				}

				float tempG = scoreTemp.g;
				tempG -= 0.1f;
				if (tempG <= 0f)
				{
					scoreTemp.g = 0f;
				} else if (tempG > 0f)
				{
					scoreTemp.g -= 0.1f;
				}

				float tempB = scoreTemp.b;
				tempB -= 0.1f;
				if (tempB <= 0f)
				{
					scoreTemp.b = 0f;
				} else if (tempB > 0f)
				{
					scoreTemp.b -= 0.1f;
				}
				scoreText.color = scoreTemp;
				yield return new WaitForSecondsRealtime (0.2f);
			}
			One.SetActive(false);
			yellowPointsSFX.SetActive (false);
		}
		else if (playerSpeed == 2)
		{
			Two.SetActive(true);
			if (GlobalControl.Instance.SFXOn == true) 
			{
				orangePointsSFX.SetActive (true);
			}
			mediumFeatherAnim.SetActive (true);
			yield return new WaitForSecondsRealtime (1f);
			Color scoreTemp = scoreText.color;
			scoreTemp.r = 255/255f;
			scoreTemp.g = 154/255f;
			scoreTemp.b = 53/255f;
			scoreTemp.a = 1.0f;
			scoreText.color = scoreTemp;
			score += 2;
			scoreText.text = score.ToString();
			mediumFeatherAnim.SetActive (false);
			while ((scoreTemp.r + scoreTemp.g + scoreTemp.b) > 0f)
			{
				float tempR = scoreTemp.r;
				tempR -= 0.1f;
				if (tempR <= 0f) 
				{
					scoreTemp.r = 0f;
				} else if (tempR > 0f)
				{
					scoreTemp.r -= 0.1f;
				}

				float tempG = scoreTemp.g;
				tempG -= 0.1f;
				if (tempG <= 0f)
				{
					scoreTemp.g = 0f;
				} else if (tempG > 0f)
				{
					scoreTemp.g -= 0.1f;
				}

				float tempB = scoreTemp.b;
				tempB -= 0.1f;
				if (tempB <= 0f)
				{
					scoreTemp.b = 0f;
				} else if (tempB > 0f)
				{
					scoreTemp.b -= 0.1f;
				}
				scoreText.color = scoreTemp;
				yield return new WaitForSecondsRealtime (0.2f);
			}
			Two.SetActive(false);
			orangePointsSFX.SetActive (false);
		}
		else if (playerSpeed == 3)
		{
			if (GlobalControl.Instance.SFXOn == true)
			{
				lightningSFX.SetActive (true);
			}
			lightningAnim.SetActive (true);
			yield return new WaitForSecondsRealtime (0.5f);
			lightningAnim.SetActive (false);
			yield return new WaitForSecondsRealtime (0.25f);
			Three.SetActive(true);
			if (GlobalControl.Instance.SFXOn == true) 
			{
				redPointsSFX.SetActive (true);
			}
			largeFeatherAnim.SetActive (true);
			yield return new WaitForSecondsRealtime (1f);
			Color scoreTemp = scoreText.color;
			scoreTemp.r = 255/255f;
			scoreTemp.g = 1/255f;
			scoreTemp.b = 1/255f;
			scoreTemp.a = 1.0f;
			scoreText.color = scoreTemp;
			score += 3;
			scoreText.text = score.ToString();
			largeFeatherAnim.SetActive (false);
			while ((scoreTemp.r + scoreTemp.g + scoreTemp.b) > 0f)
			{
				float tempR = scoreTemp.r;
				tempR -= 0.1f;
				if (tempR <= 0f) 
				{
					scoreTemp.r = 0f;
				} else if (tempR > 0f)
				{
					scoreTemp.r -= 0.1f;
				}

				float tempG = scoreTemp.g;
				tempG -= 0.1f;
				if (tempG <= 0f)
				{
					scoreTemp.g = 0f;
				} else if (tempG > 0f)
				{
					scoreTemp.g -= 0.1f;
				}

				float tempB = scoreTemp.b;
				tempB -= 0.1f;
				if (tempB <= 0f)
				{
					scoreTemp.b = 0f;
				} else if (tempB > 0f)
				{
					scoreTemp.b -= 0.1f;
				}
				scoreText.color = scoreTemp;
				yield return new WaitForSecondsRealtime (0.2f);
			}
			Three.SetActive(false);
			redPointsSFX.SetActive (false);
			lightningSFX.SetActive (false);
		}
	}
	//Coroutine used to display UI in instructions screen to guide player.
	//Displays after a short delay. Has a blinking effect.
	public IEnumerator tapToContinue ()
	{
		yield return new WaitForSecondsRealtime (5f);
		while (Input.GetMouseButtonDown(0) == false)
		{
			continueTexts.SetActive (true);
			yield return new WaitForSecondsRealtime (0.5f);
			continueTexts.SetActive (false);
			yield return new WaitForSecondsRealtime (0.25f);
		}

	}
	//Coroutine used to display a red warning UI icon. Has blinking effect.
	public IEnumerator almostDead()
	{
		while (score <= -8)
		{
			alert.SetActive (true);
			yield return new WaitForSecondsRealtime (0.25f);
			alert.SetActive (false);
			yield return new WaitForSecondsRealtime (0.25f);
		}
		almostDeadCount--;
	}
	//Coroutine used to change player sprite's color to red in order to warn them.
	public IEnumerator Red()
	{
		RedImage.SetActive (true);
		yield return new WaitForSecondsRealtime (0.25f);
		RedImage.SetActive (false);
	}
	//Coroutine used to wait before displaying gameover UI elements on screen.
	//If this is free game version, wait a bit longer in order for ad to finish.
	public IEnumerator GameOverWait()
	{
		if (PlayerPrefs.GetInt ("gamePurchased") == 0) {
			yield return new WaitForSecondsRealtime (3.5f);
		} else {
			yield return new WaitForSecondsRealtime (2.0f);
		}
		gameOvertext.SetActive (true);
		Options.gameObject.SetActive (true);
		PurchaseButton.gameObject.SetActive(true);
		HighScoreButton.gameObject.SetActive(true);
	}
	//Coroutine used to wait before displaying a game over animation on screen.
	public IEnumerator BlackScreenWait()
	{
		yield return new WaitForSecondsRealtime (1f);
		tempBool = true;
		GameOverAnim.SetActive (true);
	}
	//Coroutine used to fade out the game music.
	public IEnumerator VolumeFadeOut()
	{
		while (MainMusic.GetComponent<AudioSource> ().volume > 0) 
		{
			MainMusic.GetComponent<AudioSource> ().volume -= 0.1f;
		}
		yield return new WaitForSecondsRealtime (0.2f);
	}
	//Coroutine used to play a quack sound effect.
	public IEnumerator QuackSound()
	{
		if (GlobalControl.Instance.SFXOn == true)
		{
			Quack.SetActive (true);
			yield return new WaitForSecondsRealtime (0.12f);
			Quack.SetActive (false);
		}
	}
	//Method called when the music button in settings is tapped. This includes toggle functionality.
	public void MusicButton()
	{
		GlobalControl.Instance.musicOn = !GlobalControl.Instance.musicOn;
		if (GlobalControl.Instance.musicOn == false) 
		{
			MusicToggleButton.image.sprite = OffSprite;
		} else {
			MusicToggleButton.image.sprite = OnSprite;
		}
		GlobalControl.Instance.musicOnCount++;
	}
	//Method called when Sound effects button in settings is tapped. Includes toggle functionality.
	public void SFXButton()
	{
		GlobalControl.Instance.SFXOn = !GlobalControl.Instance.SFXOn;
		if (GlobalControl.Instance.SFXOn == false) 
		{
			SFXToggleButton.image.sprite = OffSprite;
		} else {
			SFXToggleButton.image.sprite = OnSprite;
		}
		GlobalControl.Instance.SFXOnCount++;
	}
	//Method called when options button is tapped.
	public void OptionsButton()
	{
		DefaultSongText.gameObject.SetActive (true);
		DefaultSong2Text.gameObject.SetActive (true);
		DefaultMustPurchaseText.gameObject.SetActive (false);
		Song2MustPurchaseText.gameObject.SetActive (false);
		gameOvertext.SetActive (false);
		highScoreStuff.SetActive (false);
		retryButton.gameObject.SetActive(false);
		PurchaseButton.gameObject.SetActive(false);
		HighScoreButton.gameObject.SetActive(false);
		goBackButton.gameObject.SetActive (true);
		if (GlobalControl.Instance.trophyInt == 1)
		{
			wood.SetActive (false);
		}
		else if(GlobalControl.Instance.trophyInt == 2)
		{
			bronze.SetActive (false);
		}
		else if(GlobalControl.Instance.trophyInt == 3)
		{
			silver.SetActive (false);
		}
		else if(GlobalControl.Instance.trophyInt == 4)
		{
			gold.SetActive (false);
		}
		else if(GlobalControl.Instance.trophyInt == 5)
		{
			pro.SetActive (false);
		}
		trophyScores.SetActive (false);
		MusicToggleButton.gameObject.SetActive(true);
		SFXToggleButton.gameObject.SetActive(true);
		PurchasedMusicToggle.gameObject.SetActive(true);
		DefaultMusicToggle.gameObject.SetActive(true);
		RestoreButton.gameObject.SetActive (true);
		if (PlayerPrefs.GetInt("gamePurchased") == 0)
		{
			PurchasedMusicToggle.image.sprite = GraySprite;
			DefaultMusicToggle.image.sprite = GraySprite;
		}
		if (PlayerPrefs.GetInt("gamePurchased") == 1)
		{
			if (GlobalControl.Instance.purchasedMusicBool == true) {
				PurchasedMusicToggle.image.sprite = OnSprite;
				DefaultMusicToggle.image.sprite = OffSprite;
			} else {
				PurchasedMusicToggle.image.sprite = OffSprite;
				DefaultMusicToggle.image.sprite = OnSprite;
			}
		}
		Options.gameObject.SetActive(false);
	}
	//Method called when the back button is tapped.
	public void GoBackButton()
	{
		RestoreButton.gameObject.SetActive (false);
		MusicToggleButton.gameObject.SetActive(false);
		SFXToggleButton.gameObject.SetActive(false);
		PurchasedMusicToggle.gameObject.SetActive(false);
		DefaultMusicToggle.gameObject.SetActive(false);
		trophyScores.gameObject.SetActive(true);
		gameOvertext.gameObject.SetActive(true);
		highScoreStuff.gameObject.SetActive(true);
		retryButton.gameObject.SetActive(true);
		PurchaseButton.gameObject.SetActive(true);
		HighScoreButton.gameObject.SetActive(true);
		Options.gameObject.SetActive(true);
		if (GlobalControl.Instance.trophyInt == 1)
		{
			wood.SetActive (true);
		}
		else if(GlobalControl.Instance.trophyInt == 2)
		{
			bronze.SetActive (true);
		}
		else if(GlobalControl.Instance.trophyInt == 3)
		{
			silver.SetActive (true);
		}
		else if(GlobalControl.Instance.trophyInt == 4)
		{
			gold.SetActive (true);
		}
		else if(GlobalControl.Instance.trophyInt == 5)
		{
			pro.SetActive (true);
		}
		goBackButton.gameObject.SetActive (false);
	}
	//Method called when the song1 button is tapped.
	public void Song1Button()
	{
		if (PlayerPrefs.GetInt ("gamePurchased") == 0) 
		{
			DefaultSongText.gameObject.SetActive(false);
			DefaultMustPurchaseText.gameObject.SetActive(true);
		} 
		if(GlobalControl.Instance.purchasedMusicBool == true && PlayerPrefs.GetInt ("gamePurchased") == 1) 
		{
			GlobalControl.Instance.purchasedMusicBool = false;
			PlayerPrefs.SetInt ("purchasedMusic", 0);
			DefaultMusicToggle.image.sprite = OnSprite;
			PurchasedMusicToggle.image.sprite = OffSprite;
		}
		else if (GlobalControl.Instance.purchasedMusicBool == false && PlayerPrefs.GetInt ("gamePurchased") == 1)
		{
			GlobalControl.Instance.purchasedMusicBool = true;
			PlayerPrefs.SetInt ("purchasedMusic", 1);
			DefaultMusicToggle.image.sprite = OffSprite;
			PurchasedMusicToggle.image.sprite = OnSprite;
		}

	}
	//Method called when the song2 button is tapped.
	public void Song2Button()
	{
		if (PlayerPrefs.GetInt ("gamePurchased") == 0) 
		{
			DefaultSong2Text.gameObject.SetActive(false);
			Song2MustPurchaseText.gameObject.SetActive(true);
		} 
		if (GlobalControl.Instance.purchasedMusicBool == false && PlayerPrefs.GetInt ("gamePurchased") == 1)
		{
			GlobalControl.Instance.purchasedMusicBool = true;
			PlayerPrefs.SetInt ("purchasedMusic", 1);
			PurchasedMusicToggle.image.sprite = OnSprite;
			DefaultMusicToggle.image.sprite = OffSprite;
		}
		else if (GlobalControl.Instance.purchasedMusicBool == true && PlayerPrefs.GetInt ("gamePurchased") == 1)
		{
			GlobalControl.Instance.purchasedMusicBool = false;
			PlayerPrefs.SetInt ("purchasedMusic", 0);
			DefaultMusicToggle.image.sprite = OnSprite;
			PurchasedMusicToggle.image.sprite = OffSprite;
		}
	}
}