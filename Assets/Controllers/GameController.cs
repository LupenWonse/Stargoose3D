﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public LevelExit exit = null;
	private int gemsCollected = 0;
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
		transform.position = new Vector3(transform.position.z, transform.position.y, stargoose.transform.position.z);
		exit.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x, transform.position.y, stargoose.transform.position.z);
		updateUI();
	}

	void updateUI ()
	{
		ammoText.text = "Ammo: " + stargoose.ammo.ToString();
		fuelText.text = "Fuel: " + stargoose.fuel.ToString("###");
		shieldText.text = "Shield: " + stargoose.shield.ToString();
		rocketsText.text = "Rockets: " + stargoose.rockets.ToString();
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
}
