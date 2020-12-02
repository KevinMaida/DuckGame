using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallAd : MonoBehaviour {

	//Used to display ads in the game.
	public void callAd()
	{
		if (PlayerPrefs.GetInt ("gamePurchased") == 0) 
		{
			GlobalControl.Instance.ShowDefaultAd ();
		}
	}
}
