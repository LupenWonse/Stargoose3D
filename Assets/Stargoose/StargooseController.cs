using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StargooseController : MonoBehaviour {

	[Header ("Initialization")]
	public int rockets = 6;
	public int fuel = 100, shield = 100, ammo = 100;
	public float pushForce = 1000.0f;

	[Header ("Gameplay - Planet")]
	// Gameplay variables
	[SerializeField] private float forwardSpeed = 0.5f;
	[SerializeField] private float horizontalSpeed = 0.5f;
	[SerializeField] private float constantSpeed = 1.0f;
	[SerializeField] private float fireDelay = 0.1f;
	[SerializeField] private float refireTime = 0.1f;
	[SerializeField] private Vector3 firingVelocity = new Vector3(0f,0f,50f);


	[Header ("Gameplay - Tunnel")]
	public bool isInTunnel = false;
	[SerializeField] private float mass = 1000.0f;
	[SerializeField] private float angularMass = 20.0f;
	[SerializeField] private float angularDrag = 0.95f;
	[SerializeField] private float tunnelHorizontalThrust = 5.0f;
	[SerializeField] private float tunnelConstantForwardSpeed = 5.0f;
	[SerializeField] private float tunnelReverseSpeed = 5.0f;
	[SerializeField] private float tunnelForwardSpeed = 15.0f;
	private float gravity = 9.81f;
	[SerializeField] private Transform currentTunnel = null;

	[Header ("Connections")]
	// Rocket prefab
	[SerializeField] private Rocket rocket = null;
	// Rocket firing points
	[SerializeField] private Transform leftRocketPosition = null;
	[SerializeField] private Transform rightRocketPosition = null;
	// Rocket objects
	private Rocket leftRocket = null, rightRocket = null;
	// Flying contact points
	public Transform frontRightPad,frontLeftPad,rearRightPad,rearLeftPad;
	public LayerMask floorMask;

	// Machine Guns
	[SerializeField] private Transform machineGunLeftNozzle = null;
	[SerializeField] private Transform machineGunRightNozzle = null;

	[Header ("Sound FX")]
	[SerializeField] private AudioSource gunfireFX = null;


	// Internal variables - DO NOT SERIALIZE
	private float horizontalThrust = 0;
	private float forwardThrust = 0;
	private bool shootingLeft = true;
	private float forwardDistanceAllowed = 25.0f;
	// Internal connections that will be setup during Start
	private AmmoHolder ammoHolder;
	// private GameController gameField;
	private float currentForwardLocation;
	// Internal holder variables for optimization purposes

	// Locking of the player input for placement in the tunnels & entrances
	private Vector3 targetPosition, lockedVelocity;
	private bool playerControlLocked = false;



	private new Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
		ammoHolder = AmmoHolder.holder;

		if (ammoHolder == null) {
			Debug.LogError ("No Ammo Holder found on the player ship!");
		}

		// Store initial location
		currentForwardLocation = transform.position.z;

		// Setup rigidbody
		rigidbody = GetComponent<Rigidbody>();
			
	}

	void FixedUpdate () {
		
	}

	private float height;

	void Update ()
	{
		// Movement handling
		horizontalThrust  = Input.GetAxis ("Horizontal");
		forwardThrust  = Input.GetAxis ("Vertical");

		// Three modes of Movement
		// 1) Locked -> Controlled by script
		// 2) InTunnel -> Move around the axis of the tunnel
		// 3) OnPlanet -> Move on the planet surface
		if (playerControlLocked){
			print("Locked Movement");
			lockedMovement();
		} else if (isInTunnel) {
			inTunnelMovement ();
			print("Tunnel Movement");
		} else {
			onPlanetMovement ();
			print("Free Movement");
		}

	}

	private void onPlanetMovement(){
		currentForwardLocation += constantSpeed * Time.deltaTime;

		transform.position = transform.position + horizontalThrust * horizontalSpeed * Time.deltaTime * Vector3.right + ((forwardThrust*forwardSpeed)+constantSpeed)*Time.deltaTime*Vector3.forward;
		transform.position = new Vector3(transform.position.x,transform.position.y,Mathf.Clamp(transform.position.z,currentForwardLocation - forwardDistanceAllowed, currentForwardLocation + forwardDistanceAllowed));

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
		Physics.Raycast (rearRightRay, out rearRightRaycastResult, 15, floorMask);


		float height = (frontLeftRaycastResult.point.y + rearLeftRaycastResult.point.y + frontRightRaycastResult.point.y + rearRightRaycastResult.point.y)/4.0f + .1f;

		float pitch =  Mathf.Atan2((frontLeftRaycastResult.point.y - rearLeftRaycastResult.point.y), (frontLeftPad.position.z - rearLeftPad.position.z)) * Mathf.Rad2Deg;
		float roll1 = Mathf.Atan2((frontLeftRaycastResult.point.y - frontRightRaycastResult.point.y), (frontRightPad.position.x - frontLeftPad.position.x)) * Mathf.Rad2Deg;
		float roll2 = Mathf.Atan2((rearLeftRaycastResult.point.y - rearRightRaycastResult.point.y), (rearRightPad.position.x - rearLeftPad.position.x)) * Mathf.Rad2Deg;

		float roll = Mathf.Min (roll1, roll2);

		transform.rotation = Quaternion.Euler (-pitch, 0, -roll);
		transform.position = new Vector3(transform.position.x,height,transform.position.z);


		// INPUT HANDLING
		if (Input.GetButton("Fire1") && Time.time > refireTime) {
			shootMachineGun ();
			refireTime = Time.time + fireDelay;
		}

		if (Input.GetButtonDown("ReloadLeft")) {
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

	public float getCurrentForwardLocation(){
		return currentForwardLocation;
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
		float speed;

		thrustForce = new Vector2 (transform.right.x, transform.right.y) * horizontalThrust * tunnelHorizontalThrust;
		gravityForce = Vector2.down * gravity * mass;

		float finalForce = Vector2.Dot (thrustForce + gravityForce, transform.right);

		angularSpeed += finalForce * Time.deltaTime / angularMass;
		angularSpeed = angularSpeed - angularDrag * angularSpeed * Time.deltaTime;
		transform.RotateAround (currentTunnel.position, currentTunnel.forward, angularSpeed*Time.deltaTime);

		if (forwardThrust > 0){
			speed = tunnelConstantForwardSpeed + forwardThrust * tunnelForwardSpeed;
		} else {
			speed = tunnelConstantForwardSpeed + forwardThrust * tunnelReverseSpeed;
		}

		transform.position += Vector3.forward * speed * Time.deltaTime;
	}

	public void OnTriggerEnter(Collider collider){
		if (collider.gameObject.tag == "Tunnel")
        {
			EnterTunnel(collider.gameObject.transform);
        }
    }

	public void OnTriggerExit(Collider collider){
		if (collider.gameObject.tag == "Tunnel")
        {
			ExitTunnel();
        }
	}

    private void EnterTunnel(Transform tunnel)
    {
        isInTunnel = true;
        currentTunnel = tunnel;

		moveToXPosition(tunnel.position.x);
    }

	private void ExitTunnel(){
		isInTunnel = false;
		currentTunnel = null;

		// Restore the forward location
		currentForwardLocation = transform.position.z;
	}

    void moveToXPosition(float newX){
		// Ignore the player input until we reach destination
		playerControlLocked = true;
		
		// Set target position
		Vector3 newPosition = transform.position;
		newPosition.x = newX;
		targetPosition = newPosition;
		lockedVelocity = Vector3.zero;
	}

	void lockedMovement(){
		// When input is locked we drive our own Movement
		rigidbody.MovePosition(Vector3.SmoothDamp(transform.position, targetPosition, ref lockedVelocity, 0.5f));
		print(lockedVelocity);
		// When we reach destination release the control
		if ((transform.position - targetPosition).magnitude < 0.01f){
			transform.position = targetPosition;
			playerControlLocked = false;
		}
	}

}
