using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public float gameSpeed = 0.1f;

	public Text ammoLeftText;

	//TODO Check here for opt
	public StargooseController stargoose;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = transform.position + Vector3.forward * gameSpeed * Time.deltaTime;
		ammoLeftText.text = "Ammo Left: " + stargoose.ammo.ToString ();


	}
}
