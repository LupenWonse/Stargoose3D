using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

	public enum COLLECTIBLE {AMMO, FUEL, SHIELD}
	public COLLECTIBLE type;

	public void OnTriggerEnter(Collider collider){
		// If we hit the player

		// Player collects the item
		
		// Destroy the collectible object
	}
}
