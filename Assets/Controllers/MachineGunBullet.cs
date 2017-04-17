using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MachineGunBullet : MonoBehaviour {

	[SerializeField] private int bulletDamage = 10;
	public AmmoType type; 

	// Use this for initialization
	void Awake () {
		gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider collider){

		// Deal my damage
		if (collider.GetComponent<BadGuysController> ()) {
			collider.GetComponent<BadGuysController> ().takeDamage (bulletDamage);
		}

		if (collider.GetComponent<StargooseController> () ) {
			collider.GetComponent<StargooseController> ().takeDamage(bulletDamage);
		}


		//Go back to the stack
		goToAmmoHolder();
	}


	private void goToAmmoHolder(){
		AmmoHolder.holder.retrieveBullet(this);
	}
}
