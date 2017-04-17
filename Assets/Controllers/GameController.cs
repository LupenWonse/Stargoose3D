using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	static public GameController controller;
	[SerializeField] private Text ammoText =null, shieldText=null, fuelText=null, rocketsText=null ;
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
	}

	// Update is called once per frame
	void Update () {
		updateUI();
	}

	void updateUI ()
	{
		ammoText.text = "Ammo: " + stargoose.ammo.ToString ();
		fuelText.text = "Fuel: " + stargoose.fuel.ToString ();
		shieldText.text = "Shield: " + stargoose.shield.ToString();
		rocketsText.text = "Rockets: " + stargoose.rockets.ToString();
	}

	public void collect (Collectible item){
		switch (item.type) {
			case Collectible.COLLECTIBLE.AMMO:
				stargoose.ammo += 10;
				break;
			case Collectible.COLLECTIBLE.GEM1:
				print ("GEM1 collected");
				break;
		}
	}
}
