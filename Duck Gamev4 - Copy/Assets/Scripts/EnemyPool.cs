using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour {
	//This class is a singleton used to spawn all enemy instances in the game.
	public static EnemyPool instance;

	public int enemyPoolSize = 10;
	public GameObject enemyPrefab;
	public float timeVariable;
	public float startingSpawnRate1 = 1f;
	public float startingSpawnRate2 = 1f;
	public float spawnRate1 = 4f;
	public float spawnRate2 = 2f;
	public float spawnDoubleRate = 1f;
	public float spawnTripleRate = 1f;
	public float enemyMin = -1f;
	public float enemyMax = 3.5f;
	public bool slowDown;

	private GameObject[] enemies;
	private Vector2 objectPoolPosition = new Vector2 (-15, -25);
	private float timeSinceLastSpawned1;
	private float timeSinceLastSpawned2;
	private int currentEnemy = 0;
	private int enemySpacing = 0;

	void Awake()
	{

		//If we don't currently have an EnemyPool instance...
		if (instance == null)
			//...set this one to be it...
			instance = this;
		//...otherwise...
		else if(instance != this)
			//...destroy this one because it is a duplicate.
			Destroy (gameObject);
	}
	//Initialize variables here.
	void Start () 
	{
		slowDown = false;
		// Time.timescale is variable used to determine the scale at which time passes. if
		//set to 1, time passes as fast as realtime. If set to 0.5, time passes twice as slow.
		//This will be used to pause the game by setting it to 0 when certain things trigger it.
		timeVariable = Time.timeScale;
		enemyPoolSize = 10;
		enemies = new GameObject[enemyPoolSize];
		//Populate the enemies array with enemy object instances.
		for (int i = 0; i < enemyPoolSize; i++) 
		{
			//spawns an enemy instance at a certain position on the screen.
			enemies [i] = (GameObject)Instantiate (enemyPrefab, objectPoolPosition, Quaternion.identity);
			//assigns a number to each enemy instance to identify them.
			enemies [i].GetComponent<Enemy> ().number = i;
		}
		//Spawn the first left-side enemy immediately. StartingSpawnRate1 is for left side,
		//2 is for right side of screen. These are used just for the first 2 enemies
		//that are spawned inside Start method. Enemies then infinitely spawn inside Update method.
		startingSpawnRate1 = Random.Range(1f, 4f) / timeVariable;
		//Spawns the first left-side enemy if it isn't game over.
		//Initializes some of the variables of this enemy instance and its children objects.
		if (GameControl.instance.gameOver == false) 
		{
			enemies [currentEnemy].transform.position = new Vector2 (-2.05f, 14f);
			enemies [currentEnemy].GetComponent<Enemy>().canFlip = false;
			enemies [currentEnemy].GetComponent<Enemy> ().attacking = false;
			enemies [currentEnemy].GetComponent<Enemy> ().idle = true;
			enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().flipCount = 0;
			enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().animCount = 0;
			enemies [currentEnemy].GetComponentInChildren<Attack> ().HFlipCount = 0;
			currentEnemy++;
			//resets currentEnemy counter if it's equal or greater than enemyPoolSize.
			if (currentEnemy >= enemyPoolSize) 
			{
				currentEnemy = 0;
			}
		}
		//Spawn the first right-side enemy immediately
		startingSpawnRate2 = Random.Range(1f, 4f) / timeVariable;
		//Spawns the first left-side enemy if it isn't game over.
		//Initializes some of this enemy instance's and its children objects' variables.
		if (GameControl.instance.gameOver == false) 
		{
			enemies [currentEnemy].transform.position = new Vector2 (2.05f, 14f);
			enemies [currentEnemy].GetComponent<Enemy>().canFlip = true;
			enemies [currentEnemy].GetComponent<Enemy> ().attacking = false;
			enemies [currentEnemy].GetComponent<Enemy> ().idle = true;
			enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().flipCount = 0;
			enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().animCount = 0;
			enemies [currentEnemy].GetComponentInChildren<Attack> ().HFlipCount = 0;
			currentEnemy++;
			//resets currentEnemy counter if it's equal or greater than enemyPoolSize.
			if (currentEnemy >= enemyPoolSize) 
			{
				currentEnemy = 0;
			}
		}

	}
	//This method is called every frame & used to determine what actions to take per frame.
	void Update () 
	{
		//the variable names ending with "1" are for left side of screen and "2"
		//for right side of screen. Enemies spawn from both sides in this game.
		timeSinceLastSpawned1 += Time.deltaTime;
		timeSinceLastSpawned2 += Time.deltaTime;
		spawnRate1 = Random.Range (8f, 15f) / timeVariable;
		spawnRate2 = Random.Range (8f, 15f) / timeVariable;
		//Spawns based on time passed and spawnRate variables for left side of screen.
		if (GameControl.instance.gameOver == false && timeSinceLastSpawned1 >= spawnRate1) 
		{
			timeSinceLastSpawned1 = 0;
			//Randomly chooses at what rate to spawn double or triple enemies consecutively.
			spawnDoubleRate = Random.Range(1f, 3f);
			spawnTripleRate = Random.Range (1f, 3f);
			enemies [currentEnemy].transform.position = new Vector2 (-2.05f, 14f);
			enemies [currentEnemy].GetComponent<Enemy>().canFlip = false;
			enemies [currentEnemy].GetComponent<Enemy> ().attacking = false;
			enemies [currentEnemy].GetComponent<Enemy> ().idle = true;
			enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().flipCount = 0;
			enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().animCount = 0;
			enemies [currentEnemy].GetComponentInChildren<Attack> ().HFlipCount = 0;
			currentEnemy++;
			//Resets currentEnemy counter once it iterates through all enemies
			//in the enemy array.
			if (currentEnemy >= enemyPoolSize) 
			{
				currentEnemy = 0;
			}
			//spawns a second enemy positioned a certain distance away from first enemy spawned.
			if (spawnDoubleRate >= 2f)
			{
				enemySpacing = Random.Range (22, 25);
				enemies [currentEnemy].transform.position = new Vector2 (-2.05f, enemySpacing);
				enemies [currentEnemy].GetComponent<Enemy>().canFlip = false;
				enemies [currentEnemy].GetComponent<Enemy> ().attacking = false;
				enemies [currentEnemy].GetComponent<Enemy> ().idle = true;
				enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().flipCount = 0;
				enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().animCount = 0;
				enemies [currentEnemy].GetComponentInChildren<Attack> ().HFlipCount = 0;
				currentEnemy++;
				//Resets currentEnemy counter once it iterates through all enemies
				//in the enemy array.
				if (currentEnemy >= enemyPoolSize) 
				{
					currentEnemy = 0;
				}
			}
			//spawns a third enemy positioned a certain distance away from first enemy spawned.
			if (spawnTripleRate == 1f)
			{
				enemySpacing = Random.Range (30, 33);
				enemies [currentEnemy].transform.position = new Vector2 (-2.05f, enemySpacing);
				enemies [currentEnemy].GetComponent<Enemy>().canFlip = false;
				enemies [currentEnemy].GetComponent<Enemy> ().attacking = false;
				enemies [currentEnemy].GetComponent<Enemy> ().idle = true;
				enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().flipCount = 0;
				enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().animCount = 0;
				enemies [currentEnemy].GetComponentInChildren<Attack> ().HFlipCount = 0;
				currentEnemy++;
				//Resets currentEnemy counter once it iterates through all enemies
				//in the enemy array.
				if (currentEnemy >= enemyPoolSize) 
				{
					currentEnemy = 0;
				}
			}
		}
		//Spawns based on time passed and spawnRate variables for right side of screen.
		//Same function as above section.
		if (GameControl.instance.gameOver == false && timeSinceLastSpawned2 >= spawnRate2) 
		{
			timeSinceLastSpawned2 = 0;
			spawnDoubleRate = Random.Range(1f, 3f);
			spawnTripleRate = Random.Range (1f, 3f);
			enemies [currentEnemy].transform.position = new Vector2 (2.05f, 14f);
			enemies [currentEnemy].GetComponent<Enemy>().canFlip = true;
			enemies [currentEnemy].GetComponent<Enemy> ().attacking = false;
			enemies [currentEnemy].GetComponent<Enemy> ().idle = true;
			enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().flipCount = 0;
			enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().animCount = 0;
			enemies [currentEnemy].GetComponentInChildren<Attack> ().HFlipCount = 0;
			currentEnemy++;
			if (currentEnemy >= enemyPoolSize) 
			{
				currentEnemy = 0;
			}
			if (spawnDoubleRate >= 2f)
			{
				enemySpacing = Random.Range (22, 25);
				enemies [currentEnemy].transform.position = new Vector2 (2.05f, enemySpacing);
				enemies [currentEnemy].GetComponent<Enemy>().canFlip = true;
				enemies [currentEnemy].GetComponent<Enemy> ().attacking = false;
				enemies [currentEnemy].GetComponent<Enemy> ().idle = true;
				enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().flipCount = 0;
				enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().animCount = 0;
				enemies [currentEnemy].GetComponentInChildren<Attack> ().HFlipCount = 0;
				currentEnemy++;
				if (currentEnemy >= enemyPoolSize) 
				{
					currentEnemy = 0;
				}
			}
			if (spawnTripleRate == 1f)
			{
				enemySpacing = Random.Range (30, 33);
				enemies [currentEnemy].transform.position = new Vector2 (2.05f, enemySpacing);
				enemies [currentEnemy].GetComponent<Enemy>().canFlip = true;
				enemies [currentEnemy].GetComponent<Enemy> ().attacking = false;
				enemies [currentEnemy].GetComponent<Enemy> ().idle = true;
				enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().flipCount = 0;
				enemies [currentEnemy].GetComponentInChildren<DetectPlayer> ().animCount = 0;
				enemies [currentEnemy].GetComponentInChildren<Attack> ().HFlipCount = 0;
				currentEnemy++;
				if (currentEnemy >= enemyPoolSize) 
				{
					currentEnemy = 0;
				}
			}
		}

	}

}
