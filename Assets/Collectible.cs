using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

	public enum COLLECTIBLE {AMMO, FUEL, SHIELD, ROCKETS, GEM1, GEM2, GEM3, GEM4, GEM5, GEM6}
	public COLLECTIBLE type;

	public void OnTriggerEnter(Collider collider){
		// If we hit the player

		// Player collects the item
		GameController.controller.collect(this);
		// Destroy the collectible object
		Destroy(gameObject);
	}
}
