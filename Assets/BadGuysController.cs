using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuysController : MonoBehaviour {

	public float totalHealth = 100;

	public void takeDamage(int damage){
		totalHealth -= damage;

		if (totalHealth <= 0) {
			Destroy (this.gameObject);
		}
	}

}
