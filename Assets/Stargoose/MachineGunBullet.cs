using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBullet : MonoBehaviour {

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


		//Go back to the stack
		AmmoHolder.holder.retrieveBullet(this);
	}
}
