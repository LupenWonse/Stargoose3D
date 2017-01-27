using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHolder : MonoBehaviour {

	public static AmmoHolder holder = null;

	private Stack<MachineGunBullet> bullets = new Stack<MachineGunBullet>();
	public int ammoLeft = 100;
	public MachineGunBullet machineGunBullet;

	void Awake() {
		if (holder == null) {
			holder = this;
		} else {
			Destroy (this);
		}
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < ammoLeft; i++) {
			addAmmoToStack ();
			print ("Creating Ammo");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void addAmmoToStack(){
		MachineGunBullet bullet = GameObject.Instantiate (machineGunBullet,transform);
		bullets.Push (bullet);
	}

	public MachineGunBullet giveBullet(){
		MachineGunBullet newBullet = bullets.Pop ();
		newBullet.gameObject.SetActive (true);
		return newBullet;
	}

	public void retrieveBullet(MachineGunBullet bullet){
		// Disable the bullet
		bullet.gameObject.SetActive (false);

		// Push it onto our stack
		bullets.Push (bullet);

		// Set it as our child so it is removed from the scene hierarchy
		// This is only organizationin in Unity
		bullet.transform.SetParent(transform);
	}

}
