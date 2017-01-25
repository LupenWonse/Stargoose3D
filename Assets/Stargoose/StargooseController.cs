using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StargooseController : MonoBehaviour {

	[SerializeField] private float forwardSpeed;
	[SerializeField] private float horizontalSpeed;
	private float horizontalThrust = 0;
	private float forwardThrust = 0;

	private float maxForwardDistanceAllowed = 0;
	private float minForwardDistanceAllowed = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {



		//transform.position += Vector3.forward * forwardSpeed * forwardThrust;
		//transform.position += Vector3.right * horizontalSpeed * horizontalThrust;
	}

	void Update(){

		horizontalThrust = Input.GetAxis ("Horizontal");
		forwardThrust = Input.GetAxis ("Vertical");
			

		float x = Mathf.Clamp (transform.position.x + (horizontalSpeed * horizontalThrust), -25, 25);
		float z = Mathf.Clamp (transform.position.z + (forwardSpeed * forwardThrust), -25, 25);
		float y = transform.position.y;

		transform.position = new Vector3 (x, y, z);

	}
}
