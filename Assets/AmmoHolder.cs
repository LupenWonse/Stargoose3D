using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHolder : MonoBehaviour {

	private Stack<MachineGunBullet> bullets = new Stack<MachineGunBullet>();
	public int ammoLeft = 100;
	public MachineGunBullet machineGunBullet;

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
		bullet.gameObject.SetActive (false);
		bullets.Push (bullet);
	}

}
