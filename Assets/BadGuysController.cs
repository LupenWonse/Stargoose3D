using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuysController : MonoBehaviour {

	public enum enemyTypes {Mine,Turret,RocketLauncher}

	public enemyTypes type;
	public float totalHealth = 100;
	public int hitDamage = 25;

	public void takeDamage(int damage){
		totalHealth -= damage;

		if (totalHealth <= 0) {
			Destroy (this.gameObject);
		}
	}

	public void OnTriggerEnter (Collider collider){
		if (collider.GetComponent<StargooseController> ()) {
			collider.GetComponent<StargooseController> ().takeDamage (hitDamage);
		}
	}

}
