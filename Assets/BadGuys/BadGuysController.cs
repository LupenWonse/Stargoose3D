using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuysController : MonoBehaviour {

	public enum enemyTypes {Mine,Turret,RocketLauncher}

	public enemyTypes type;
	public float totalHealth = 100;
	public int hitDamage = 25;

	public MachineGunBullet machineGunBullet;

	private bool isEnemyInRange = true;
	private float fireDelay = 1.0f;
	private float firingTime = 0.0f;
	public Vector3 firingVelocity;

	public Transform turretNozzle;

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

		if (type == enemyTypes.Mine) {
			// Mines get destroyed when the stargoose hits them
			explode();
		}
	}

	void Update(){
		if (type == enemyTypes.Turret) {
			turretBehave ();
		}

	}


	void turretBehave(){
		if (isEnemyInRange) {

			if (Time.time > firingTime) {
				MachineGunBullet bullet = AmmoHolder.holder.giveBullet ();

				bullet.gameObject.layer = machineGunBullet.gameObject.layer;
				bullet.gameObject.GetComponent<Renderer> ().sharedMaterials = machineGunBullet.gameObject.GetComponent<Renderer> ().sharedMaterials;

				bullet.transform.position = turretNozzle.transform.position;
				bullet.transform.SetParent (null);
				bullet.GetComponent<Rigidbody> ().velocity = firingVelocity;
				firingTime = Time.time + fireDelay;
			}
		}
	}


	public void explode(){
		Destroy (gameObject);
	}



}
