using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	public float speed = 50.0f;

	private new Rigidbody rigidbody;


	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
	}

	public void fire ()
	{
		rigidbody.velocity = Vector3.forward * speed;
	}
}
