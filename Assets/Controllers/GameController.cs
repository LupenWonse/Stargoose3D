using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public float gameSpeed = 0.1f;

	[SerializeField] private Text ammoText =null, shieldText=null, fuelText=null, rocketsText=null ;

	//TODO Check here for opt
	public StargooseController stargoose;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = transform.position + Vector3.forward * gameSpeed * Time.deltaTime;

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
