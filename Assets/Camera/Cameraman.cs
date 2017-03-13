using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour {

	private StargooseController stargoose;
	[SerializeField] private LayerMask floor = new LayerMask();

	private Vector3 targetCameraLookAt;
	private Vector3 targetCameraPosition;
	private float cameraElevation;
	private float cameraSlide;


	private Vector3 currentLookAt = new Vector3();
	[SerializeField] private float smoothTime = 0.3f;
	[SerializeField] private Vector3 cameraPositionVelocity = new Vector3();
	[SerializeField] private Vector3 cameraRotationVelocity = new Vector3();
	[SerializeField] private float cameraDistance = 20.0f;

	// Use this for initialization
	void Start () {
		// Initialize player object
		stargoose = GameObject.FindGameObjectWithTag ("Player").GetComponent<StargooseController>();
		if (stargoose == null) {
			Debug.LogError ("No Player Game Object Was Found");
		} else {
			// Initialiaze camera position
			targetCameraLookAt = (stargoose.transform.position - transform.position + Vector3.forward*30);
			//cameraElevation = (stargoose.transform.position.z - transform.position.z) / 3;
			//cameraSlide = (stargoose.transform.position.x);
			targetCameraPosition = transform.position;
		}
	}
	// Update is called once per frame
	
	void Update(){
		// Calculate new camera position
		// If not in a Tunnel
		if (!stargoose.isInTunnel){
			surfaceCameraPositionUpdate();
		} else {
			tunnelCameraPositionUpdate();
		}
	}

    private void tunnelCameraPositionUpdate()
    {
        cameraSlide =  stargoose.transform.position.x;
		cameraElevation = stargoose.transform.position.y;
		cameraDistance = stargoose.transform.position.z - 10.0f;
		targetCameraPosition = new Vector3 (cameraSlide, cameraElevation, cameraDistance);
    }

    private void surfaceCameraPositionUpdate()
    {
        // New Camera Target
		//Vector3 newTarget = cameraTarget;
		// Raycast mouse loaction to the world
		Ray screenRay = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		RaycastHit hitObject;
		if (Physics.Raycast (screenRay, out hitObject, 100, floor)) {
			// New target is the raycast point clamped
			targetCameraLookAt.x = Mathf.Clamp (hitObject.point.x, -5, 5);
		}

		cameraElevation = (stargoose.transform.position.z - transform.position.z) / 3;
		cameraSlide =  stargoose.transform.position.x;
		cameraDistance = stargoose.getCurrentForwardLocation() - 40.0f;

		targetCameraPosition = new Vector3 (cameraSlide, cameraElevation, cameraDistance);
    }

    void LateUpdate () {
		// Move camera smoothly to the target location and rotation
		transform.position = Vector3.SmoothDamp(transform.position, targetCameraPosition, ref cameraPositionVelocity, smoothTime);
		currentLookAt = Vector3.SmoothDamp(currentLookAt, targetCameraLookAt, ref cameraRotationVelocity, smoothTime);
		transform.rotation = Quaternion.LookRotation(currentLookAt);
	}
}
