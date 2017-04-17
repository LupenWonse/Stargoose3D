using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoType {playerMachineGun, enemyMachineGun, enemyMortar}

[System.Serializable]
public struct Ammo{
	public GameObject prefab;
	public int stock;
}

public class AmmoHolder : MonoBehaviour {

	public Ammo playerMachineGun;
	public Ammo enemyMachineGun;
	public Ammo enemyMortar;

	public static AmmoHolder holder = null;

	private Stack<MachineGunBullet> playerMachineGunBullets = new Stack<MachineGunBullet>();
	private Stack<MachineGunBullet> enemnyMachineGunBullets = new Stack<MachineGunBullet>();
	private Stack<MachineGunBullet> enemyMortarBullets = new Stack<MachineGunBullet>();


	public int ammoLeft = 100;

	void Awake() {
		if (holder == null) {
			holder = this;
		} else {
			Destroy (this);
		}
	}

	// Use this for initialization
	void Start () {
		// playerMachineGun
		for (int i = 0; i < playerMachineGun.stock; i++) {
			playerMachineGunBullets.Push( GameObject.Instantiate (playerMachineGun.prefab,transform).GetComponent<MachineGunBullet>());
		}

		// enemyMachineGun
		for (int i = 0; i < enemyMachineGun.stock; i++) {
			enemnyMachineGunBullets.Push( GameObject.Instantiate (enemyMachineGun.prefab,transform).GetComponent<MachineGunBullet>());
		}

		// enemyMortar
		for (int i = 0; i < enemyMortar.stock; i++) {
			enemyMortarBullets.Push( GameObject.Instantiate (enemyMortar.prefab,transform).GetComponent<MachineGunBullet>());
		}

	}

	public MachineGunBullet giveBullet(AmmoType ammoType){
		MachineGunBullet newBullet = null;

		switch (ammoType) {
		case AmmoType.playerMachineGun:
			newBullet = playerMachineGunBullets.Pop ();
			break;
		case AmmoType.enemyMachineGun:
			newBullet = enemnyMachineGunBullets.Pop ();
			break;
		case AmmoType.enemyMortar:
			newBullet = enemyMortarBullets.Pop ();
			break;
		}

		newBullet.gameObject.SetActive (true);
		return newBullet;
	}

	public void retrieveBullet(MachineGunBullet bullet){
		// Disable the bullet
		bullet.gameObject.SetActive (false);

		switch (bullet.type) {
		case AmmoType.playerMachineGun:
			playerMachineGunBullets.Push(bullet);
			break;
		case AmmoType.enemyMachineGun:
			enemnyMachineGunBullets.Push(bullet);
			break;
		case AmmoType.enemyMortar:
			enemyMortarBullets.Push(bullet);
			break;
		}

		// Set it as our child so it is removed from the scene hierarchy
		// This is only organizationin in Unity
		bullet.transform.SetParent(transform);
	}

}
