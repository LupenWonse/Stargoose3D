using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[SerializeField] private Text ammoText =null, shieldText=null, fuelText=null, rocketsText=null ;
	private StargooseController stargoose;

	void Start(){
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
}
