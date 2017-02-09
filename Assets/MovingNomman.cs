using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingNomman : BadGuysController {

	public Vector3 movementVector;

	private new Rigidbody rigidbody;

	void Awake(){
		rigidbody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate(){
		float currentYSpeed = rigidbody.velocity.y;
		rigidbody.velocity = movementVector + new Vector3 (0, Mathf.Min (currentYSpeed, 0), 0);
	}

	void LateUpdate ()
	{
		// Hack to freeze rotation
		Vector3 currentRotation = rigidbody.rotation.eulerAngles;
		currentRotation.y = 0;
		rigidbody.rotation = Quaternion.Euler(currentRotation);
	}


}
