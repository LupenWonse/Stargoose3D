using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MachineGunBullet : MonoBehaviour {

	private int bulletDamage = 10;

	// Use this for initialization
	void Awake () {
		gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider collider){
		print ("Bullet Hits");
		// Perform any effects of hitting something

		// Deal my damage
		if (collider.GetComponent<BadGuysController> ()) {
			collider.GetComponent<BadGuysController> ().takeDamage (bulletDamage);
		}

		//Go back to the stack
		goToAmmoHolder();
	}


	private void goToAmmoHolder(){
		AmmoHolder.holder.retrieveBullet(this);
	}
}
