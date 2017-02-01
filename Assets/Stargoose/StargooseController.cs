using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StargooseController : MonoBehaviour {

	// Audio Object


	[Header ("Gameplay")]
	// Gameplay variables
	[SerializeField] public int ammo = 100;
	[SerializeField] private float forwardSpeed = 0.5f;
	[SerializeField] private float horizontalSpeed = 0.5f;
	[SerializeField] private float fireDelay = 0.1f;
	[SerializeField] private float forwardConstantSpeed = 0.1f;

	[SerializeField] private float refireTime = 0.1f;


	[Header ("Connections")]
	// Rocket prefab
	[SerializeField] private Rocket rocket;
	// Rocket firing points
	[SerializeField] private Transform leftRocketPosition;
	[SerializeField] private Transform rightRocketPosition;
	// Rocket objects
	private Rocket leftRocket = null, rightRocket = null;

	// Machine Guns
	[SerializeField] private MachineGunBullet bullet;
	[SerializeField] private Transform machineGunLeftNozzle = null;
	[SerializeField] private Transform machineGunRightNozzle = null;

	[SerializeField] private GameController gameField;

	// Internal variables - DO NOT SERIALIZE
	private float horizontalThrust = 0;
	private float forwardThrust = 0;

	private bool shootingLeft = true;
	private float maxForwardDistanceAllowed = 25.0f;
	private float minForwardDistanceAllowed = 25.0f;

	private Vector3 firingVelocity = new Vector3(0f,0f,50f);

	private AmmoHolder ammoHolder;


	[Header ("Sound FX")]
	[SerializeField] private AudioSource gunfireFX;

	// Use this for initialization
	void Start () {
		ammoHolder = AmmoHolder.holder;

		if (ammoHolder == null) {
			Debug.LogError ("No Ammo Holder found on the player ship!");
		}

		gameField = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}

	void Update ()
	{

		horizontalThrust = Input.GetAxis ("Horizontal");
		forwardThrust = Input.GetAxis ("Vertical");
			
		//print (gameField.z);
		float x = Mathf.Clamp (transform.position.x + (horizontalSpeed * horizontalThrust), gameField.transform.position.x - 25, gameField.transform.position.x + 25);
		float z = Mathf.Clamp (transform.position.z + (forwardConstantSpeed + forwardSpeed * forwardThrust), gameField.transform.position.z - minForwardDistanceAllowed, gameField.transform.position.z + maxForwardDistanceAllowed);
		float y = transform.position.y;

		//transform.position = new Vector3 (x, y, z);
		GetComponent<Rigidbody>().MovePosition(new Vector3(x,y,z));

		if (Input.GetMouseButton (0) && Time.time > refireTime) {
			shootMachineGun ();
			refireTime = Time.time + fireDelay;
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
		print(leftRocket);
			if (leftRocket == null) {
				reloadLeftRocket ();
			} else {
				fireLeftRocket ();
			}
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			if (rightRocket == null) {
				reloadRightRocket ();
			} else {
				fireRightRocket ();
			}
		}

	}

	private void reloadLeftRocket ()
	{
			leftRocket = GameObject.Instantiate (rocket, leftRocketPosition, false);
	}

	private void reloadRightRocket ()
	{
			rightRocket = GameObject.Instantiate (rocket, rightRocketPosition, false);
	}

	private void fireLeftRocket ()
	{
		// This code reads much better
		// Release the rocket
		leftRocket.transform.parent = null;
		// Fire
		leftRocket.fire();
		// Rocket is no longer ours
		leftRocket = null;
	}

	private void fireRightRocket ()
	{
		// Release the rocket
		rightRocket.transform.parent = null;
		// Fire
		rightRocket.fire();
		// Rocket is no longer ours
		rightRocket = null;

	}





	private void shootMachineGun(){
		if (ammo > 0) {
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
			bullet.transform.SetParent (null);
			bullet.GetComponent<Rigidbody> ().velocity = firingVelocity;
			ammo -= 1;
			gunfireFX.Play ();
		} else {
			// Machine Gun Empty
		}
	}
}
