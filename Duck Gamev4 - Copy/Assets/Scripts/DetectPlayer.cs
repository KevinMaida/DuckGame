using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectPlayer : MonoBehaviour {
	//This script is attached to a child object of each enemy instance.
	//The purpose of this child object is to just detect if the player collides
	//with it. If player collides with it the enemy
	//will attack.

	public Animator attackAnimator;
	//ASR = attack sprite renderer. Should have made it camel case like all the other variables.
	public SpriteRenderer ASR;
	public GameObject clapSFX;
	public bool collided;
	//xPos = position of the x axis.
	public float xPos;
	//The number used to identify each instance of the enemy game object.
	public int parentNumber;
	public int flipCount = 0;
	//animCount = animation counter.
	public int animCount;

	//Initialize variables.
	void Start () 
	{
		animCount = 0;
		attackAnimator = gameObject.GetComponent<Animator> ();
		ASR = gameObject.GetComponent<SpriteRenderer> ();
		attackAnimator.ResetTrigger ("Fail");
		attackAnimator.ResetTrigger ("Success");
		parentNumber = gameObject.GetComponentInParent<Enemy> ().number;
		xPos = transform.localPosition.x;
	}
	//This method is to determine what actions to take every frame of the game.
	void Update () 
	{
		//This if, else-if block determines what direction (left or right)
		//this object faces based on what direciton the parent enemy object faces.
		if (gameObject.GetComponentInParent<Enemy>().canFlip == true && flipCount < 1) 
		{
			transform.localPosition = new Vector2(-xPos, transform.localPosition.y);
			flipCount++;
		}
		else if (gameObject.GetComponentInParent<Enemy>().SR.flipX == false)
		{
			transform.localPosition = new Vector2(xPos, transform.localPosition.y);
		}
		//This if statement handles the enemy hands' animation. The hands animations and
		//this script are attached to a child object of each enemy instance.
		if (gameObject.GetComponentInParent<Enemy>().attacking == true && parentNumber == gameObject.GetComponentInParent<Enemy>().who && animCount < 1 && GetComponentInParent<Enemy>().canHands == true) 
		{
			//animCount variable used to make sure hands attacking animation only plays once.
			animCount++;
			//If this object's box collider collides with the player's
			//box collider, turn off player's ability to duck underwater,
			//and activate the "success" attack animation of the enemy's hands.
			if (collided == true) 
			{
				GameControl.instance.player.GetComponent<GyroDuck> ().canDuck = false;
				GameControl.instance.player.GetComponent<GyroDuck> ().duckingCounter = 1;
				attackAnimator.ResetTrigger ("Fail");
				attackAnimator.SetTrigger ("Success");
				//GameControl.instance.DuckDied ();
			}
			//If player doesn't collide with this game object, then play
			//the "fail" animation of enemy's hands.
			else 
			{
				attackAnimator.ResetTrigger ("Success");
				attackAnimator.SetTrigger ("Fail");
			}
		} 
	}
	//Method that gets called when this object's box collider in Unity
	//collides with another box collider in the game. This method updates the collided variable.
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			collided = true;
		} 
	}
	//On exiting a collision between box colliders, reset the collided variable.
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			collided = false;
		}
	}
	//Method that gets called from the last frame of the attacking hands animation within Unity.
	//Resets variables related to attacking.
	void attackEnded()
	{
		GameControl.instance.player.GetComponent<SpriteRenderer>().color = Color.white;
		gameObject.GetComponentInParent<Enemy>().idle = true;
		gameObject.GetComponentInParent<Enemy>().attacking = false;
		gameObject.GetComponentInParent<Enemy> ().canHands = false;
		gameObject.GetComponentInParent<ScrollingObject> ().useAnticipationSpeed = false;
		EnemyPool.instance.slowDown = false;
		attackAnimator.ResetTrigger ("Fail");
		attackAnimator.ResetTrigger ("Success");
		clapSFX.SetActive (false);
	}
	//If enemy attack was successful, this method gets called from the attacking
	//hands animation within Unity. Successful attack means game over.
	void callDuckDied()
	{
		GameControl.instance.DuckDied ();
	}
	//If enemy attack was successful, this method gets called from the attacking
	//hands animation within Unity and activates a game object in Unity that
	//plays the hand clap sound effect.
	void callClapSFX()
	{
		if (GlobalControl.Instance.SFXOn == true) 
		{
			clapSFX.SetActive (true);
		}
	}
}
