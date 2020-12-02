using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour {

	private Rigidbody2D rb2d;
	public bool useAnticipationSpeed;
	public float BG2SPeed;
	public float BG3Speed;
	public float BG4Speed;
	//MonoBehaviour callback. Used to initialize variables.
	void Start () 
	{
		rb2d = GetComponent<Rigidbody2D> ();
		useAnticipationSpeed = false;
		BG2SPeed = -4.5f;
		BG3Speed = -15.5f;
		BG4Speed = -7.5f;

	}
	//MonoBehaviour callback. In this method we update the object this script is attached to every frame
	//in order to make it have a scrolling effect.
	void Update () 
	{

		if (useAnticipationSpeed == false && gameObject.tag == "BG3")
		{
			rb2d.velocity = new Vector2 (0, BG3Speed);
		}
		if (useAnticipationSpeed == false && gameObject.tag == "Enemy")
		{
			rb2d.velocity = new Vector2 (0, GameControl.instance.scrollSpeed);
		}
		if (useAnticipationSpeed == false && gameObject.tag == "BG2")
		{
			rb2d.velocity = new Vector2 (0, BG2SPeed);
		}
		if (useAnticipationSpeed == false && gameObject.tag == "BG4")
		{
			rb2d.velocity = new Vector2 (0, BG4Speed);
		}
		if (useAnticipationSpeed == true)
		{
			rb2d.velocity = new Vector2 (0, gameObject.GetComponent<Enemy>().anticipationSpeed);
		}
		if (GameControl.instance.gameOver == true)
		{
			rb2d.velocity = Vector2.zero;

		}

	}
}
