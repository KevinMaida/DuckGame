using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

//This class is attached to a child object of the "enemy" game object.
	public Animator a_Animator;
	public SpriteRenderer HSR;
	public int HFlipCount; // hand flip count
	public float HXPos; // hand x position in 2D space
	public SpriteRenderer SR2;
	public GameObject attackSFX;

	// Use this for initialization
	void Start () {

		a_Animator = GetComponent<Animator> ();
		SR2 = GetComponent<SpriteRenderer> ();
		HSR = GetComponent<SpriteRenderer> ();
		HXPos = transform.localPosition.x;
		a_Animator.ResetTrigger ("CanIdle");
		a_Animator.ResetTrigger ("CanAttack");
	}

	// Update is called once per frame
	void Update () {
		//In these first if and else if statements, it's checking what direction (left or right)
		//the 2D enemy is facing so that the 2D attacking animation can face the same way.
		if (GetComponentInParent<Enemy>().canFlip == true && HFlipCount < 1)
		{
			transform.localPosition = new Vector2 (-HXPos, transform.localPosition.y);
			HFlipCount++;
		}
		else if (gameObject.GetComponentInParent<Enemy>().SR.flipX == false)
		{
			transform.localPosition = new Vector2(HXPos, transform.localPosition.y);
		}
		//In these if statements, it's setting what animation to play based on the parent object's flag. SFX = sound effects.
		if (GetComponentInParent<Enemy>().attacking == false)
		{
			a_Animator.ResetTrigger ("CanAttack");
			a_Animator.SetTrigger ("CanIdle");
			attackSFX.SetActive (false);
		}
		if (GetComponentInParent<Enemy>().attacking == true)
		{
			a_Animator.ResetTrigger ("CanIdle");
			a_Animator.SetTrigger ("CanAttack");
			if (GlobalControl.Instance.SFXOn == true) 
			{
				attackSFX.SetActive (true);
			}
		}
	}
	// This method, like a few others in the game gets called from an "animation event"
	//in Unity. A true canHands flag in parent object means enemy is able to attack player.
	void canHands()
	{
		GetComponentInParent<Enemy> ().canHands = true;
	}
	// Method called from "animation event" right before enemy attacks. 
	//Makes sure coroutine called "Red" is stopped before starting it to flash a red "warning" image on screen.
	void Warning()
	{
		GameControl.instance.StopCoroutine ("Red");
		GameControl.instance.StartCoroutine ("Red");
	}
}
