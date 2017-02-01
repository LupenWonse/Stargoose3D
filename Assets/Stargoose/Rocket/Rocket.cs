using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	public float speed = 50.0f;
	public int damage = 100;

	private new Rigidbody rigidbody;


	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
	}

	public void fire ()
	{
		rigidbody.velocity = Vector3.forward * speed;
	}

	void OnTriggerEnter (Collider collider)
	{
		print ("Rocket Hit");
		if (collider.GetComponent<BadGuysController> ()) {
			collider.GetComponent<BadGuysController>().takeDamage(damage);
		}

		hit();
	}

	private void hit(){
		Destroy(gameObject);
	}
}
