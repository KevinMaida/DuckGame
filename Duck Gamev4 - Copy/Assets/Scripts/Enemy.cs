using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public Animator m_Animator;
	public SpriteRenderer SR;
	public BoxCollider2D box;
	public GameObject HTurnSFX;
	public GameObject CTurn1SFX;
	public GameObject CTurn2SFX;
	public GameObject CTurn3SFX;
	public GameObject CTurn4SFX;
	public GameObject CTurn5SFX;
	public float colliderX;
	//Next six lines from EnemyPool
	public bool isPlayerClose;
	public bool canFlip; // canFlip is a flag to determine what direction 2D enemy will face. All enemies have to face the center towards the player.
	public int number;
	public int who;
	public int odds = 0;
	public int oddsWait = 0;
	public int CTOdds = 0;
	public bool attacking;
	public bool idle;
	public bool withinRange;
	public bool canHands;
	public float offsetTimeCount;
	public float timeCount;
	public float anticipationSpeed;

	//Method called when the game program starts. 
	//Here it makes sure variables are initialized, and that animation triggers are reset.
	void Start()
	{
		m_Animator = gameObject.GetComponent<Animator> ();
		SR = gameObject.GetComponent<SpriteRenderer> ();
		box = gameObject.GetComponent<BoxCollider2D> ();
		idle = false;
		attacking = false;
		withinRange = false;
		anticipationSpeed = -0f;
		colliderX = box.offset.x;
		m_Animator.ResetTrigger ("CanCTurn");
		m_Animator.ResetTrigger ("CanHTurn");
		m_Animator.ResetTrigger ("Attack");
		m_Animator.ResetTrigger ("Idle");
		m_Animator.ResetTrigger ("CanCTurn2");
		m_Animator.ResetTrigger ("CanCTurn3");
		m_Animator.ResetTrigger ("CanCTurn4");
		m_Animator.ResetTrigger ("CanCTurn5");
	}
	//In this method I keep track of time in the variable timeCount and use it to determine what this enemy's actions are.
	void Update ()
	{
		timeCount += Time.deltaTime;
		//If it's game over, remove this enemy game object from the screen.
		if (GameControl.instance.gameOver == true)
		{
			gameObject.SetActive (false);
		}
		//This if & else-if section decides whether to pause the scrolling movement of everything on screen or not.
		//Scrolling pauses only when an enemy instance is attacking the player.
		if (EnemyPool.instance.slowDown == true)
		{
			gameObject.GetComponent<ScrollingObject> ().useAnticipationSpeed = true;
		}
		else if(EnemyPool.instance.slowDown == false)
		{
			gameObject.GetComponent<ScrollingObject> ().useAnticipationSpeed = false;
		}
		//This if, else-if block makes sure to space out enemy instance actions by 3 seconds.
		//If time passed is greater than 3 seconds, then enemy can 
		//randomly determine what action to take based on a range of 1 to 9.
		if (timeCount >= 3)
		{
			oddsWait = Random.Range (1, 9);
			offsetTimeCount += Time.deltaTime;
		}
		else if (timeCount < 3)
		{
			oddsWait = 0;
		}
		//These if, else-if statements determine what action this enemy instance
		//takes based on what random number "oddsWait" gets assigned during this frame.
		if (oddsWait >= 1 && oddsWait <= 3 && offsetTimeCount >= 0 && attacking == false)
		{
			attackOdds ();
			timeCount = 0;
			offsetTimeCount = 0;
		}
		else if (oddsWait > 3 && oddsWait <= 6 && offsetTimeCount >= 0.5f && attacking == false)
		{
			attackOdds ();
			timeCount = 0;
			offsetTimeCount = 0;
		}
		else if (oddsWait > 6 && oddsWait <= 9 && offsetTimeCount >= 1 && attacking == false)
		{
			attackOdds ();
			timeCount = 0;
			offsetTimeCount = 0;
		}
		//This if statement resets animation triggers so that they're ready to play again if needed.
		if (idle == true)
		{
			m_Animator.ResetTrigger ("CanCTurn");
			m_Animator.ResetTrigger ("CanCTurn2");
			m_Animator.ResetTrigger ("CanCTurn3");
			m_Animator.ResetTrigger ("CanCTurn4");
			m_Animator.ResetTrigger ("CanCTurn5");
			m_Animator.ResetTrigger ("CanHTurn");
			m_Animator.ResetTrigger ("Attack");
			m_Animator.SetTrigger("Idle");
			idle = false;
		}
		//These if statements determine whether this 2D enemy instance faces left or right.
		if (transform.position.x > 0)
		{
			SR.flipX = true;
			gameObject.GetComponentInChildren<DetectPlayer> ().ASR.flipX = true;
			gameObject.GetComponentInChildren<Attack> ().SR2.flipX = true;
			gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2(-colliderX, 0);
		}
		if (transform.position.x < 0 && SR.flipX == true)
		{
			SR.flipX = false;
			gameObject.GetComponentInChildren<DetectPlayer> ().ASR.flipX = false;
			gameObject.GetComponentInChildren<Attack> ().SR2.flipX = false;
			gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2(colliderX, 0);
		}

	}
	//Method that gets called when this enemy instance's box collider object
	//collides with another box collider object. If it collides with the player's
	//box collider, then it updates variables that help determine whether
	//this enemy instance attacks or not.
	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.GetComponent<PlayerMovement> () != null && other.gameObject.tag == "Player") 
		{
			isPlayerClose = true;
		}
		if (other.gameObject.tag == "Valid")
		{
			withinRange = true;
		}
	}
	//Method that gets called when this enemy instance's box collider object
	//is no longer colliding with another.
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{	
			isPlayerClose = false;
		}
		if (other.gameObject.tag == "Valid")
		{
			withinRange = false;
		}
	}

	//Method that determines whether this enemy instance attacks the player or not.
	void attackOdds ()
	{
		odds = Random.Range (1,18);
		//If odds variable falls in this range, then this enemy instance doesn't attack
		//the player and instead plays a harmless head turn animation.
		if (odds >= 1 && odds <= 3) 
		{
			
			m_Animator.ResetTrigger ("Attack");
			m_Animator.ResetTrigger ("CanCTurn");
			m_Animator.ResetTrigger ("CanCTurn2");
			m_Animator.ResetTrigger ("CanCTurn3");
			m_Animator.ResetTrigger ("CanCTurn4");
			m_Animator.ResetTrigger ("CanCTurn5");
			m_Animator.ResetTrigger ("Idle");
			m_Animator.SetTrigger("CanHTurn");
			//If the game allows sound effects to be played right now, then play
			//the sound effect for a harmless enemy head turn.
			if (GlobalControl.Instance.SFXOn == true) 
			{
				HTurnSFX.SetActive (true);
			}

		} 
		//If odds variable falls in this range, then this enemy instance doesn't attack
		//the player and instead calls the creepy head turn method which
		//determines randomly which of the many creepy head turn animations to play.
		else if (odds > 3 && odds <= 6) 
		{
			m_Animator.ResetTrigger ("Attack");
			m_Animator.ResetTrigger ("CanHTurn");
			m_Animator.ResetTrigger ("Idle");
			CTurn();
			//Don't play the harmless head turn sound effect AKA SFX.
			HTurnSFX.SetActive (false);
		}
		//If odds variable falls in this range AND the player is close enough, then this
		//enemy instance attacks the player.
		else if (odds > 6 && odds <= 15 && isPlayerClose == true) 
		{
			//Sets valid variable to true to let game know it's a valid attack.
			GameControl.instance.valid = true;
			//Turns the player sprite's color to red as a warning to the player.
			GameControl.instance.player.GetComponent<SpriteRenderer>().color = Color.red;
			//This will set the feathers UI to match the player's speed.
			GameControl.instance.playerSpeed = GameControl.instance.whichFeathers;
			//This will set the scrolling speed of the game to 0 only until the attack finishes.
			gameObject.GetComponent<ScrollingObject> ().useAnticipationSpeed = true;
			//This makes sure that every frame the scrolling of enemy object is stopped until this
			//variable is set to false;
			EnemyPool.instance.slowDown = true;
			//Animation triggers within Unity are reset here. Attack animation is triggered to play.
			m_Animator.ResetTrigger ("CanCTurn");
			m_Animator.ResetTrigger ("CanCTurn2");
			m_Animator.ResetTrigger ("CanCTurn3");
			m_Animator.ResetTrigger ("CanCTurn4");
			m_Animator.ResetTrigger ("CanCTurn5");
			m_Animator.ResetTrigger ("CanHTurn");
			m_Animator.ResetTrigger ("Idle");
			m_Animator.SetTrigger("Attack");
			//The who variable is an int that assigns numbers to each enemy instance
			//to keep track of them and help game manager scripts deal with them.
			who = number;
			//Calls the attack method.
			attack ();
			HTurnSFX.SetActive (false);
		} 
		//If odds variable falls in this range, this enemy just continues playing its
		//idle animation since everything else is reset.
		else if (odds > 15)
		{
			m_Animator.ResetTrigger ("CanCTurn");
			m_Animator.ResetTrigger ("CanCTurn2");
			m_Animator.ResetTrigger ("CanCTurn3");
			m_Animator.ResetTrigger ("CanCTurn4");
			m_Animator.ResetTrigger ("CanCTurn5");
			m_Animator.ResetTrigger ("CanHTurn");
			m_Animator.ResetTrigger ("Idle");
			m_Animator.ResetTrigger ("Attack");
			HTurnSFX.SetActive (false);
		}
	}
	//Method to determine which creepy head turn animation this enemy will play when called.
	//Works in the same way as attackOdds method.
	void CTurn ()
	{
		CTOdds = Random.Range (1,5);
		if (CTOdds == 1)
		{
			m_Animator.ResetTrigger ("CanCTurn2");
			m_Animator.ResetTrigger ("CanCTurn3");
			m_Animator.ResetTrigger ("CanCTurn4");
			m_Animator.ResetTrigger ("CanCTurn5");
			m_Animator.SetTrigger ("CanCTurn");
			//Makes sure to only play sound effect when player is close.
			if (isPlayerClose == true)
			{
				if (GlobalControl.Instance.SFXOn == true) 
				{
					CTurn1SFX.SetActive (true);
				}
				CTurn2SFX.SetActive (false);
				CTurn3SFX.SetActive (false);
				CTurn4SFX.SetActive (false);
				CTurn5SFX.SetActive (false);
			}
		}
		if (CTOdds == 2)
		{
			m_Animator.ResetTrigger ("CanCTurn");
			m_Animator.ResetTrigger ("CanCTurn3");
			m_Animator.ResetTrigger ("CanCTurn4");
			m_Animator.ResetTrigger ("CanCTurn5");
			m_Animator.SetTrigger ("CanCTurn2");
			if (isPlayerClose == true)
			{
				if (GlobalControl.Instance.SFXOn == true)
				{
					CTurn2SFX.SetActive (true);
				}
				CTurn1SFX.SetActive (false);
				CTurn3SFX.SetActive (false);
				CTurn4SFX.SetActive (false);
				CTurn5SFX.SetActive (false);
			}
		}
		if (CTOdds == 3)
		{
			m_Animator.ResetTrigger ("CanCTurn2");
			m_Animator.ResetTrigger ("CanCTurn");
			m_Animator.ResetTrigger ("CanCTurn4");
			m_Animator.ResetTrigger ("CanCTurn5");
			m_Animator.SetTrigger ("CanCTurn3");
			if (isPlayerClose == true)
			{
				if (GlobalControl.Instance.SFXOn == true) 
				{
					CTurn3SFX.SetActive (true);
				}
				CTurn2SFX.SetActive (false);
				CTurn1SFX.SetActive (false);
				CTurn4SFX.SetActive (false);
				CTurn5SFX.SetActive (false);
			}
		}
		if (CTOdds == 4)
		{
			m_Animator.ResetTrigger ("CanCTurn2");
			m_Animator.ResetTrigger ("CanCTurn3");
			m_Animator.ResetTrigger ("CanCTurn");
			m_Animator.ResetTrigger ("CanCTurn5");
			m_Animator.SetTrigger ("CanCTurn4");
			if (isPlayerClose == true)
			{
				if (GlobalControl.Instance.SFXOn == true) 
				{
					CTurn4SFX.SetActive (true);
				}
				CTurn2SFX.SetActive (false);
				CTurn3SFX.SetActive (false);
				CTurn1SFX.SetActive (false);
				CTurn5SFX.SetActive (false);
			}
		}
		if (CTOdds == 5)
		{
			m_Animator.ResetTrigger ("CanCTurn2");
			m_Animator.ResetTrigger ("CanCTurn3");
			m_Animator.ResetTrigger ("CanCTurn4");
			m_Animator.ResetTrigger ("CanCTurn");
			m_Animator.SetTrigger ("CanCTurn5");
			if (isPlayerClose == true)
			{
				if (GlobalControl.Instance.SFXOn == true) 
				{
					CTurn5SFX.SetActive (true);
				}
				CTurn2SFX.SetActive (false);
				CTurn3SFX.SetActive (false);
				CTurn4SFX.SetActive (false);
				CTurn1SFX.SetActive (false);
			}
		}
	}
	//Method that flags this enemy instance as being in attacking mode.
	//turns off the player's splashing animation. Sets the timescale
	//back to normal.
	void attack ()
	{
		attacking = true;
		PlayerMovement.instance.bigSplash.SetActive (false);
		Time.timeScale = 1f;
	}
	//Method that sets the timescale to be slower in order to give the player
	//anticipation to react to an attack.
	void anticipation ()
	{
		Time.timeScale = 0.5f;
	}

}
