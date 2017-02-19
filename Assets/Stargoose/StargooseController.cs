using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StargooseController : MonoBehaviour {



	[Header ("Initialization")]
	public int rockets = 6;
	public int fuel = 100, shield = 100, ammo = 100;
	public float pushForce = 1000.0f;


	[Header ("Gameplay")]
	// Gameplay variables
	[SerializeField] private float forwardSpeed = 0.5f;
	[SerializeField] private float horizontalSpeed = 0.5f;
	[SerializeField] private float fireDelay = 0.1f;
	[SerializeField] private float refireTime = 0.1f;


	[Header ("Tunnel Physics")]
	public bool isInTunnel = false;
	[SerializeField] private float mass = 1000.0f;
	[SerializeField] private float angularMass = 20.0f;
	[SerializeField] private float angularDrag = 0.95f;
	[SerializeField] private float tunnelHorizontalThrust = 5.0f;
	private float gravity = 9.81f;

	[Header ("Connections")]
	// Rocket prefab
	[SerializeField] private Rocket rocket = null;
	// Rocket firing points
	[SerializeField] private Transform leftRocketPosition = null;
	[SerializeField] private Transform rightRocketPosition = null;
	// Rocket objects
	private Rocket leftRocket = null, rightRocket = null;

	// Machine Guns
	[SerializeField] private MachineGunBullet bullet = null;
	[SerializeField] private Transform machineGunLeftNozzle = null;
	[SerializeField] private Transform machineGunRightNozzle = null;

	// Flying contact points
	public Transform frontRightPad,frontLeftPad,rearRightPad,rearLeftPad;

	public LayerMask floorMask;


	[Header ("Sound FX")]
	[SerializeField] private AudioSource gunfireFX = null;

	// Internal variables - DO NOT SERIALIZE
	private float horizontalThrust = 0;
	private float forwardThrust = 0;

	private bool shootingLeft = true;
	private float forwardDistanceAllowed = 25.0f;

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
		//rigidbody = GetComponent<Rigidbody>();
		this.fuel = 100;

	}

	void FixedUpdate () {
		
	}

	void Update ()
	{


		// Movement handling
		horizontalThrust  = Input.GetAxis ("Horizontal");
		forwardThrust  = Input.GetAxis ("Vertical");

		if (isInTunnel) {
			inTunnelMovement ();
			return;
		}


		transform.position = transform.position + horizontalThrust * horizontalSpeed * Time.deltaTime * Vector3.right + forwardThrust*forwardSpeed*Time.deltaTime*Vector3.forward;
		transform.position = new Vector3(transform.position.x,transform.position.y,Mathf.Clamp(transform.position.z,gameField.transform.position.z - forwardDistanceAllowed, gameField.transform.position.z + forwardDistanceAllowed));

		//GetComponent<Rigidbody> ().MovePosition (transform.position + Vector3.forward);
		Ray frontLeftRay= new Ray(frontLeftPad.position,Vector3.down);
		Ray rearLeftRay= new Ray(rearLeftPad.position,Vector3.down);
		Ray frontRightRay = new Ray (frontRightPad.position, Vector3.down);
		Ray rearRightRay = new Ray (rearRightPad.position, Vector3.down);

		RaycastHit frontRightRaycastResult = new RaycastHit ();
		RaycastHit frontLeftRaycastResult = new RaycastHit ();
		RaycastHit rearLeftRaycastResult = new RaycastHit ();
		RaycastHit rearRightRaycastResult = new RaycastHit ();

		Physics.Raycast (rearLeftRay, out rearLeftRaycastResult, 15, floorMask);
		Physics.Raycast (frontLeftRay, out frontLeftRaycastResult, 15, floorMask);
		Physics.Raycast (frontRightRay, out frontRightRaycastResult, 15, floorMask);


		float height = (frontLeftRaycastResult.point.y + rearLeftRaycastResult.point.y)/2.0f + 1.0f;

		//float pitch = Mathf.Atan((frontRaycastResult.point.y - backRaycastResult.point.y) / (front.position.z - back.position.z));

		float pitch =  Mathf.Atan2((frontLeftRaycastResult.point.y - rearLeftRaycastResult.point.y), (frontLeftPad.position.z - rearLeftPad.position.z)) * Mathf.Rad2Deg;
		float roll = Mathf.Atan2((frontLeftRaycastResult.point.y - frontRightRaycastResult.point.y), (frontRightPad.position.x - frontLeftPad.position.x)) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.Euler (-pitch, 0, -roll);
		//GetComponent<Rigidbody> ().MovePosition (new Vector3(transform.position.x,height,transform.position.z));
		transform.position = new Vector3(transform.position.x,height,transform.position.z);


		//GetComponent<Rigidbody>().AddForce( new Vector3(0,0,5));

		//GetComponent<Rigidbody> ().AddForce (0, (2 - testHit.distance) * 100 / testHit.distance, 0);
		//GetComponent<Rigidbody>().velocity  = new Vector3(0,GetComponent<Rigidbody>().velocity.y,10);
		// Key Presses
		/*
		if (Input.GetMouseButton (0) && Time.time > refireTime) {
			shootMachineGun ();
			refireTime = Time.time + fireDelay;
		}
		*/
		if (Input.GetButton("Fire1") && Time.time > refireTime) {
			shootMachineGun ();
			refireTime = Time.time + fireDelay;
		}




	//	print (Input.GetAxis ("Fire1"));
			

		if (Input.GetButtonDown("ReloadLeft")) {
		print(leftRocket);
			if (leftRocket == null) {
				reloadLeftRocket ();
			} else {
				fireLeftRocket ();
			}
		}

		if (Input.GetButtonDown("ReloadRight")) {
			if (rightRocket == null) {
				reloadRightRocket ();
			} else {
				fireRightRocket ();
			}
		}

	}

	void LateUpdate ()
	{
		// Hack to freeze rotation
		//Vector3 currentRotation = rigidbody.rotation.eulerAngles;
		//currentRotation.y = 0;
		//rigidbody.rotation = Quaternion.Euler(currentRotation);
	}

	// Damage functions
	public void takeDamage(int damage){
		shield -= damage;
	}




	private void reloadLeftRocket ()
	{
		if (rockets > 0) {
			leftRocket = GameObject.Instantiate (rocket, leftRocketPosition, false);
			// We used one rocket
			rockets -= 1;
		}
	}

	private void reloadRightRocket ()
	{
		if (rockets > 0) {
			rightRocket = GameObject.Instantiate (rocket, rightRocketPosition, false);
			// We used one rocket
			rockets -= 1;
		}
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
			MachineGunBullet newBullet = ammoHolder.giveBullet (AmmoType.playerMachineGun);
			//newBullet.gameObject.layer = bullet.gameObject.layer;
			//newBullet.GetComponentInChildren<Renderer> ().sharedMaterial = bullet.GetComponentInChildren<Renderer> ().sharedMaterial;
			//newBullet.gameObject.GetComponentInChildren<MeshFilter> ().sharedMesh = bullet.gameObject.GetComponentInChildren<MeshFilter> ().sharedMesh;

			// Position and fire the bullet
			if (shootingLeft) {
				newBullet.transform.position = machineGunLeftNozzle.transform.position;
			} else {
				newBullet.transform.position = machineGunRightNozzle.transform.position;
			}
			// Switch shooting nozzle
			shootingLeft = !shootingLeft;
			//newBullet.transform.SetParent (null);
			newBullet.GetComponent<Rigidbody> ().velocity = firingVelocity;
			ammo -= 1;
			gunfireFX.Play ();
		} else {
			// Machine Gun Empty
		}
	}

	private float angularSpeed;
	private Vector2 thrustForce, gravityForce;

	public void inTunnelMovement(){


		Vector2 totalForce;
		Vector2 thrustForce = new Vector2 (transform.right.x, transform.right.y) * horizontalThrust * tunnelHorizontalThrust;
		Vector2 gravityForce = Vector2.down * gravity * mass;
		totalForce = thrustForce + gravityForce;

		float finalForce = Vector2.Dot (totalForce, transform.right);

		angularSpeed += finalForce * Time.deltaTime / angularMass;
		angularSpeed = angularSpeed - angularDrag * angularSpeed * Time.deltaTime;

		print ("Thrust Force: " + thrustForce);
		print ("Angular Speed: " + angularSpeed);
		print ("Horizontal Force: " + finalForce);
		Transform tunnel = GameObject.Find ("Tunnel").transform;
		transform.RotateAround (tunnel.position, tunnel.forward, angularSpeed*Time.deltaTime);
	}
}
