using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StargooseController : MonoBehaviour {

	[SerializeField] private float forwardSpeed = 0.5f;
	[SerializeField] private float horizontalSpeed = 0.5f;

	[SerializeField] private Transform machineGunLeftNozzle = null;
	[SerializeField] private Transform machineGunRightNozzle = null;
	[SerializeField] private MachineGunBullet bullet;


	private float horizontalThrust = 0;
	private float forwardThrust = 0;

	private bool shootingLeft = true;

	private float maxForwardDistanceAllowed = 0;
	private float minForwardDistanceAllowed = 0;

	private Vector3 firingVelocity = new Vector3(0f,0f,50f);

	private AmmoHolder ammoHolder;

	// Use this for initialization
	void Start () {
		ammoHolder = GetComponentInChildren<AmmoHolder> ();

		if (ammoHolder == null) {
			Debug.LogError ("No Ammo Holder found on the player ship!");
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}

	void Update(){

		horizontalThrust = Input.GetAxis ("Horizontal");
		forwardThrust = Input.GetAxis ("Vertical");
			

		float x = Mathf.Clamp (transform.position.x + (horizontalSpeed * horizontalThrust), -25, 25);
		float z = Mathf.Clamp (transform.position.z + (forwardSpeed * forwardThrust), -25, 25);
		float y = transform.position.y;

		transform.position = new Vector3 (x, y, z);

		if (Input.GetMouseButton(0) ){
			shootMachineGun ();
		}

	}


	private void shootMachineGun(){
		// Get the next bullet from the ammo holder
		bullet = ammoHolder.giveBullet ();
		// Position and fire the bullet
		if (shootingLeft) {
			bullet.transform.position = machineGunLeftNozzle.transform.position;
		} else {
			bullet.transform.position = machineGunRightNozzle.transform.position;
		}
		// Switch shooting nozzle
		shootingLeft = !shootingLeft;
		bullet.transform.SetParent(null);
		bullet.GetComponent<Rigidbody> ().velocity = firingVelocity;
	}
}
