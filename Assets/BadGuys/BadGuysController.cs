using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy types
public enum enemyTypes {Mine,Turret,Mortar,RocketLauncher}

public class BadGuysController : MonoBehaviour {

	// Inspector variables
	public enemyTypes type;
	public float totalHealth = 100;
	public int hitDamage = 25;

	// Bullet that we will use
	[SerializeField] private MachineGunBullet machineGunBullet;
	[SerializeField] private float fireDelay = 1.0f;
	[SerializeField] private float firingVelocity = 10.0f;

	public Transform turretHead;
	public Transform turretNozzle;

	// Internal variabls
	private bool isEnemyInRange = true;
	private float firingTime = 0.0f;

	void Update ()
	{
		if (type == enemyTypes.Turret) {
			turretBehave ();
		} else if (type == enemyTypes.Mortar) {
			mortarBehave();
		}

	}

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

	void turretBehave(){
		if (isEnemyInRange) {
			aim();
			fire();
		}
	}

	void mortarBehave ()
	{
		if (isEnemyInRange) {
			aim();
			mortarFire();
		}

	}

	void mortarFire ()
	{
		// Check if turret is ready to fire
		if (Time.time > firingTime) {
			print("Firing Mortar");
			// WARNING Instantiating is expensive. This is a temp solution
			// FIX FIX FIX FIX FIX FIX
			MachineGunBullet bullet = GameObject.Instantiate(machineGunBullet);
			bullet.gameObject.SetActive(true);

			// Set it to be same as defined bullet
			//bullet.gameObject.layer = machineGunBullet.gameObject.layer;
			//bullet.gameObject.GetComponentInChildren<Renderer> ().sharedMaterials = machineGunBullet.gameObject.GetComponent<Renderer> ().sharedMaterials;

			// Place the bullet at the nozzle position & orientation
			bullet.transform.position = turretNozzle.transform.position;
			bullet.transform.rotation = turretNozzle.transform.rotation;
			// Free the bullet
			bullet.transform.SetParent (null);
			// Fire
			bullet.GetComponent<Rigidbody> ().velocity = turretNozzle.transform.forward * firingVelocity;
			// Reset firing time
			firingTime = Time.time + fireDelay;
		}
	}

	void fire ()
	{
		// Check if turret is ready to fire
		if (Time.time > firingTime) {
			// Get a bullet from the cache
			MachineGunBullet bullet = AmmoHolder.holder.giveBullet ();

			// Set it to be same as defined bullet
			bullet.gameObject.layer = machineGunBullet.gameObject.layer;
			bullet.gameObject.GetComponentInChildren<Renderer> ().sharedMaterials = machineGunBullet.gameObject.GetComponent<Renderer> ().sharedMaterials;

			// Place the bullet at the nozzle position & orientation
			bullet.transform.position = turretNozzle.transform.position;
			bullet.transform.rotation = turretNozzle.transform.rotation;
			// Free the bullet
			bullet.transform.SetParent (null);
			// Fire
			bullet.GetComponent<Rigidbody> ().velocity = turretNozzle.transform.forward * firingVelocity;
			// Reset firing time
			firingTime = Time.time + fireDelay;
		}
	}

	void aim ()
	{
		// Get player position
		Transform playerLocation = GameObject.FindObjectOfType<StargooseController> ().transform;
		// Find the vector from this object to the player
		Vector3 fromToVector = playerLocation.position - transform.position;
		// Project onto y = plane
		fromToVector.y = 0;

		// Get the angle of this vector and convert to a 360 degree representation
		float aimAngle = Vector3.Angle (fromToVector, Vector3.forward);
		if (Vector3.Cross (fromToVector, Vector3.forward).y < 0) {
			aimAngle = (aimAngle * -1) + 360;
		}

		// Find which quadrant the player is in
		if (aimAngle <= 45 / 2) {
			aimAngle = 0;
		} else if (aimAngle <= 45 + 45 / 2) {
			aimAngle = 45;
		} else if (aimAngle <= 90 + 45 / 2) {
			aimAngle = 90;
		} else if (aimAngle >= 360 - 45 / 2) {
			aimAngle = 0;
		} else if (aimAngle >= 315 - 45 / 2) {
			aimAngle = 315;
		} else if (aimAngle >= 270 - 45 / 2) {
			aimAngle = 270;
		} else {
			aimAngle = 0;
		}

		// Store temporarily the current rotation
		Vector3 currentRotation = turretHead.rotation.eulerAngles;

		// Aim the turret towards player
		turretHead.rotation = Quaternion.Euler(currentRotation.x,-aimAngle,currentRotation.z);
	}


	public void explode(){
		Destroy (gameObject);
	}



}
