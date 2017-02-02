using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StargooseController : MonoBehaviour {

	[Header ("Initialization")]
	public int rockets = 6;
	public int fuel = 100, shield = 100, ammo = 100;


	[Header ("Gameplay")]
	// Gameplay variables
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



	[Header ("Sound FX")]
	[SerializeField] private AudioSource gunfireFX;

	// Internal variables - DO NOT SERIALIZE
	private float horizontalThrust = 0;
	private float forwardThrust = 0;

	private bool shootingLeft = true;
	private float maxForwardDistanceAllowed = 25.0f;
	private float minForwardDistanceAllowed = 25.0f;

	private Vector3 firingVelocity = new Vector3(0f,0f,50f);


	// Internal connections that will be setup during Start
	private AmmoHolder ammoHolder;
	private GameController gameField;
	// Internal holder variables for optimization purposes
	private new Rigidbody rigidbody;


	// Use this for initialization
	void Start () {
		ammoHolder = AmmoHolder.holder;

		if (ammoHolder == null) {
			Debug.LogError ("No Ammo Holder found on the player ship!");
		}

		gameField = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		// Initialize our rigidbody
		rigidbody = GetComponent<Rigidbody>();
		this.fuel = 100;

	}

	void FixedUpdate () {
		// Clamp by disallowing thrust in the direction where we hit the limit
		if (transform.position.z < gameField.transform.position.z - 25) {
			forwardThrust = Mathf.Max (0.0f, forwardThrust);
		} else if (transform.position.z > gameField.transform.position.z + 25) {
			forwardThrust = Mathf.Min (0.0f, forwardThrust);
		}

		if (transform.position.x < gameField.transform.position.x - 25) {
			horizontalThrust = Mathf.Max (0.0f, horizontalThrust);
		} else if (transform.position.x > gameField.transform.position.x + 25) {
			horizontalThrust = Mathf.Min (0.0f, horizontalThrust);
		}

		// Set our velocity based on the current thrust rates
		rigidbody.velocity = new Vector3(horizontalSpeed * horizontalThrust,rigidbody.velocity.y,forwardSpeed * forwardThrust + gameField.gameSpeed);
	}

	void Update ()
	{
		// Input Handling
		// Axes
		horizontalThrust = Input.GetAxis ("Horizontal");
		forwardThrust = Input.GetAxis ("Vertical");

		// Key Presses
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
			// We used one rocket
			rockets -= 1;
	}

	private void reloadRightRocket ()
	{
			rightRocket = GameObject.Instantiate (rocket, rightRocketPosition, false);
			// We used one rocket
			rockets -= 1;
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
