using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour {

	public static PlayerMovement instance;
	public float speed;
	public bool triggered = false;
	public float halfPlayerWidth;
	public float halfPlayerHeight;
	public Vector2 screenSize;
	public float timeDifference;
	public bool canTap;
	public float timeCounter;
	public GameObject bigSplash;
	public GameObject fastSplash;
	public GameObject DuckUnderwaterSound;
	public GameObject DuckShakeWaterOffSound;
	public Animator p_Animator;

	private Rigidbody2D rb;

	void Awake()
	{
		//Variables initialized early.
		canTap = true;
		Time.timeScale = 1f;
		if (instance == null)
			instance = this;

		else if(instance != this)
			Destroy (gameObject);

	}

	void Start () 
	{
		//Variables initialized.
		p_Animator = gameObject.GetComponent<Animator> ();
		p_Animator.ResetTrigger ("Duck");
		speed = Time.timeScale;
		halfPlayerWidth = transform.localScale.x / -2;
		halfPlayerHeight = transform.localScale.y / -2;
		screenSize = new Vector2 (Camera.main.aspect * Camera.main.orthographicSize + halfPlayerWidth, Camera.main.orthographicSize + halfPlayerHeight);

		rb = GetComponent<Rigidbody2D> ();
	}

	void Update () 
	{
		if (GameControl.instance.gameOver == true)
		{
			canTap = false;
			speed = 0;
		}
		//The player moves at the same rate as the timescale (ex. 0.5 would be half as slow as normal passing time.)
		speed = Time.timeScale;
		timeCounter += Time.deltaTime;
		//For the purpose of the player being forced to deal with an enemy at the start,
		//the ability to tap the screen to swim faster has been delayed by 1 second.
		if (Time.timeScale < 8f && timeCounter > 1f) 
		{
			canTap = true;
			Move ();
		} 
		else 
		{
			canTap = false;
		}
		KeepWithinScreenBounds ();
		//This section deals with displaying the correct feather UI that corresponds to how fast
		//the player is moving.
		if (speed > 0f && speed <= 1f && GameControl.instance.gameOver == false)
		{
			GameControl.instance.Feathers0.SetActive (true);
			GameControl.instance.Feathers2.SetActive (false);
			GameControl.instance.Feathers3.SetActive (false);
			GameControl.instance.Feathers1.SetActive (false);
			GameControl.instance.whichFeathers = 0;
		}
		if (speed > 1f && speed <= 2f && GameControl.instance.gameOver == false)
		{
			GameControl.instance.Feathers0.SetActive (false);
			GameControl.instance.Feathers2.SetActive (false);
			GameControl.instance.Feathers3.SetActive (false);
			GameControl.instance.Feathers1.SetActive (true);
			GameControl.instance.whichFeathers = 1;
		}
		if (speed > 2f && speed <= 4f && GameControl.instance.gameOver == false)
		{
			GameControl.instance.Feathers0.SetActive (false);
			GameControl.instance.Feathers2.SetActive (true);
			GameControl.instance.Feathers3.SetActive (false);
			GameControl.instance.Feathers1.SetActive (false);
			GameControl.instance.whichFeathers = 2;
		}
		if (speed > 4f && GameControl.instance.gameOver == false)
		{
			GameControl.instance.Feathers0.SetActive (false);
			GameControl.instance.Feathers2.SetActive (false);
			GameControl.instance.Feathers3.SetActive (true);
			GameControl.instance.Feathers1.SetActive (false);
			GameControl.instance.whichFeathers = 3;
		}
	}
	//Method that deals with player movement.
	void Move()
	{

		if (canTap == true && Input.GetMouseButtonDown (0)) 
		{
			Time.timeScale = 6.0f;
			GameControl.instance.GameMusic1.SetActive (false);
			if (GlobalControl.Instance.musicOn == true) 
			{
				GameControl.instance.GameMusic2.SetActive (true);
			}
			if (GlobalControl.Instance.SFXOn == true) 
			{
				bigSplash.GetComponent<AudioSource> ().volume = 1;
			}
			if (GlobalControl.Instance.SFXOn == true) 
			{
				bigSplash.GetComponent<AudioSource> ().playOnAwake = true;
			}
			if (GlobalControl.Instance.SFXOn == false) 
			{
				bigSplash.GetComponent<AudioSource> ().playOnAwake = false;
			}
			bigSplash.SetActive (true);
			fastSplash.GetComponent<AudioSource> ().enabled = false;
			speed = Time.timeScale;
			canTap = false;
			StopCoroutine ("Cooldown");
			StartCoroutine ("Cooldown");
		}
	}
	//Method that gets called when player exits collision with another game object's collider box.
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.GetComponent<Enemy>() != null && other.gameObject.tag == "Enemy")
		{
			GameControl.instance.valid = false;
			GameControl.instance.player.GetComponent<SpriteRenderer>().color = Color.white;
		}
	}

	//Coroutine that deals with gradually slowing down player movement.
	public IEnumerator Cooldown()
	{
		StopCoroutine ("SplashVolumeFadeOut");
		StartCoroutine ("SplashVolumeFadeOut");
		canTap = false;
		timeDifference = speed - 1f;
		for (int i = 0; i < (5*timeDifference); i++) 
		{
			yield return new WaitForSecondsRealtime (0.01f);
			canTap = false;
			if (Time.timeScale >= 1.2f) 
			{
				Time.timeScale -= 0.2f;
			} 
			canTap = true;
			speed = Time.timeScale;
			yield return speed;
			yield return canTap;
		}
		bigSplash.SetActive (false);
		GameControl.instance.GameMusic2.SetActive (false);
		if (GlobalControl.Instance.musicOn == true) 
		{
			GameControl.instance.GameMusic1.SetActive (true);
		}
		if (GlobalControl.Instance.SFXOn == true) 
		{
			fastSplash.GetComponent<AudioSource> ().enabled = true;
		}
		Time.timeScale = 1f;
		canTap = true;
		speed = Time.timeScale;
		yield return speed;
		yield return canTap;

	}
	//Method to keep player always centered on screen.
	void KeepWithinScreenBounds()
	{
		if (transform.position.x < -screenSize.x) 
		{
			transform.position = new Vector2 (-screenSize.x, transform.position.y);
		} 
		else if (transform.position.x > screenSize.x) 
		{
			transform.position = new Vector2 (screenSize.x, transform.position.y);
		}

		if (transform.position.y < -screenSize.y) 
		{
			transform.position = new Vector2 (transform.position.x, -screenSize.y);
		} 
		else if (transform.position.y > screenSize.y) 
		{
			transform.position = new Vector2 (transform.position.x, screenSize.y);
		}
	}
	void callSplash()
	{
		StopCoroutine ("Splash");
		StartCoroutine ("Splash");
	}

	void callDuckUnderwater()
	{
		StopCoroutine ("DuckUnderwater");
		StartCoroutine ("DuckUnderwater");
	}

	void callDuckShakeOffWater()
	{
		StopCoroutine ("DuckShakeOffWater");
		StartCoroutine ("DuckShakeOffWater");
	}
	//Coroutine for enabling a sound effect then after a delay, disabling it.
	public IEnumerator Splash()
	{
		if (GlobalControl.Instance.SFXOn == true) 
		{
			fastSplash.GetComponent<AudioSource> ().enabled = true;
		}
		yield return new WaitForSecondsRealtime (0.4f);
		fastSplash.GetComponent<AudioSource> ().enabled = false;
	}
	//Coroutine for fading out a sound effect.
	public IEnumerator SplashVolumeFadeOut()
	{
		while (bigSplash.GetComponent<AudioSource>().volume > 0)
		{
			bigSplash.GetComponent<AudioSource> ().volume -= 0.1f;
			yield return new WaitForSecondsRealtime (0.2f);
		}
	}
	//Coroutine for enabling a sound effect then after a delay, disabling it. 
	public IEnumerator DuckUnderwater()
	{
		if (GlobalControl.Instance.SFXOn == true) 
		{
			DuckUnderwaterSound.SetActive (true);
		}
		yield return new WaitForSecondsRealtime (2f);
		DuckUnderwaterSound.SetActive (false);
	}
	//Coroutine for enabling a sound effect then after a delay, disabling it.
	public IEnumerator DuckShakeOffWater()
	{
		if (GlobalControl.Instance.SFXOn == true) 
		{
			DuckShakeWaterOffSound.SetActive (true);
		}
		yield return new WaitForSecondsRealtime (0.5f);
		DuckShakeWaterOffSound.SetActive (false);
	}
}
