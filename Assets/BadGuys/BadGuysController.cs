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

			aim();

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

	void aim ()
	{
		Transform playerLocation = GameObject.FindObjectOfType<StargooseController> ().transform;
		Vector3 fromToVector = playerLocation.position - transform.position;
		fromToVector.y = 0;

		//print(fromToVector);
		//print(playerLocation.position);
		//print(Vector3.Angle(fromToVector,Vector3.forward));


		float aimAngle = Vector3.Angle (fromToVector, Vector3.forward);
		aimAngle *= Mathf.Sign (Vector3.Cross (fromToVector, Vector3.forward).y);
		if (aimAngle < 0) {
			aimAngle += 360;
		}

		print (aimAngle);
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





		turretNozzle.rotation = Quaternion.Euler(0,-aimAngle,0);
		//turretNozzle.rotation = Quaternion.Euler(0,Vector3.Angle(fromToVector,Vector3.forward),0);
		//turretNozzle.r(0,Vector3.Angle(fromToVector,Vector3.forward),0,Space.World);

	}


	public void explode(){
		Destroy (gameObject);
	}



}
