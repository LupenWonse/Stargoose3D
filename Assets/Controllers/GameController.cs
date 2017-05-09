using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public LevelExit exit = null;
	private int gemsCollected = 0;
	static public GameController controller;
	[SerializeField] private Text ammoText, shieldText, fuelText, rocketsText, gemsText, pauseMenuMessage ;
	[SerializeField] private Canvas pauseMenu;
	[SerializeField] private Button resumeButton;
	private StargooseController stargoose;

	void Start(){
		// Singleton with static accessor
		if (controller == null){
			controller = this;
		} else {
			Destroy(gameObject);
		}
		// Find player
		stargoose = GameObject.FindObjectOfType<StargooseController> ();
		transform.position = new Vector3(transform.position.z, transform.position.y, stargoose.transform.position.z);
		exit.enabled = false;

		// Hide pause menu
		startMenu();


		// Reset Time scale
		Time.timeScale = 0;
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x, transform.position.y, stargoose.transform.position.z);
		updateUI();

		// Pause handling
		if (Input.GetButton("Pause")){
			pauseGame();
		}
	}

	void updateUI ()
	{
		ammoText.text = "Ammo: " + stargoose.ammo.ToString();
		fuelText.text = "Fuel: " + stargoose.fuel.ToString("###");
		shieldText.text = "Shield: " + stargoose.shield.ToString();
		rocketsText.text = "Rockets: " + stargoose.rockets.ToString();
		gemsText.text = "Gems: " + gemsCollected + " / 6";
	}

	public void collect (Collectible item){
		switch (item.type) {
			case Collectible.COLLECTIBLE.AMMO:
				stargoose.ammo += 10;
				if(stargoose.ammo > 100){
					stargoose.ammo = 100;
				}
				break;
			case Collectible.COLLECTIBLE.FUEL:
				stargoose.fuel += 10;
				if(stargoose.fuel > 100){
					stargoose.fuel = 100;
				}
				break;
			case Collectible.COLLECTIBLE.SHIELD:
				stargoose.shield += 10;
				if(stargoose.shield > 100){
					stargoose.shield = 100;
				}
				break;
			case Collectible.COLLECTIBLE.ROCKETS:
				stargoose.rockets = 6;
				break;
			case Collectible.COLLECTIBLE.GEM1:
				print ("GEM1 collected");
				gemsCollected++;
				break;
			case Collectible.COLLECTIBLE.GEM2:
				print ("GEM2 collected");
				gemsCollected++;
				break;
			case Collectible.COLLECTIBLE.GEM3:
				print ("GEM3 collected");
				gemsCollected++;
				break;
			case Collectible.COLLECTIBLE.GEM4:
				print ("GEM4 collected");
				gemsCollected++;
				break;
			case Collectible.COLLECTIBLE.GEM5:
				print ("GEM5 collected");
				gemsCollected++;
				break;
			case Collectible.COLLECTIBLE.GEM6:
				print ("GEM6 collected");
				gemsCollected++;
				break;
		}

		if (gemsCollected >= 6){
			enableExit();
		}

	}

	public void enableExit(){
		exit.enabled = true;
	}

	public void pauseGame(){
		Time.timeScale = 0;
		pauseMenuMessage.text = "Game Paused";
		resumeButton.enabled = true;
		pauseMenu.gameObject.SetActive(true);
	}

	public void resumeGame(){
		Time.timeScale = 1;
		pauseMenu.gameObject.SetActive(false);
	}

	public void restartGame(){
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
	}

	public void endGame(){
		pauseMenuMessage.text = "Game Over";
		resumeButton.enabled = false;
		pauseMenu.gameObject.SetActive(true);
	}

	public void winGame(){
		pauseMenuMessage.text = "You Win";
		resumeButton.enabled = false;
		pauseMenu.gameObject.SetActive(true);
	}

	public void startMenu(){
		pauseMenuMessage.text = "Stargoose 3D";
		resumeButton.enabled = true;
		pauseMenu.gameObject.SetActive(true);
	}
}
