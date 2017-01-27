using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour {


	private GameObject stargoose;
	[SerializeField] LayerMask floor;

	private Vector3 cameraTarget;
	private float cameraElevation;
	private float cameraSlide;


	public float smoothTime = 0.3f;
	private float cameraSmoothVelocity = 0.0f;
	private float cameraElevationVelocity = 0.0f;
	private float cameraSlideVelocity = 0.0f;

	// Use this for initialization
	void Start () {
		// Initialize player object
		stargoose = GameObject.FindGameObjectWithTag ("Player");
		if (stargoose == null) {
			Debug.LogError ("No Player Game Object Was Found");
		} else {
			// Initialiaze camera position
			cameraTarget = (stargoose.transform.position - transform.position + Vector3.forward*30);
			cameraElevation = (stargoose.transform.position.z - transform.position.z) / 3;
			cameraSlide = (stargoose.transform.position.x);
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		// Calculate the camera orientation
		// New Camera Target
		Vector3 newTarget = cameraTarget;
		// Raycast mouse loaction to the world
		Ray screenRay = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		RaycastHit hitObject;
		if (Physics.Raycast (screenRay, out hitObject, 100, floor)) {
			// New target is the raycast point clamped
			newTarget.x = Mathf.Clamp (hitObject.point.x, -5, 5);
		}

		// Calculate camera elevation
		float newElevation = (stargoose.transform.position.z - transform.position.z) / 3;

		//Calculate camera slide
		float newSlide = (stargoose.transform.position.x);

		// Smoothing the x movement
		cameraTarget.x = Mathf.SmoothDamp (cameraTarget.x, newTarget.x, ref cameraSmoothVelocity, smoothTime);
		cameraElevation = Mathf.SmoothDamp (cameraElevation, newElevation, ref cameraElevationVelocity, smoothTime);
		cameraSlide =  Mathf.SmoothDamp (cameraSlide, newSlide, ref cameraSlideVelocity, smoothTime);

		transform.rotation = Quaternion.LookRotation (cameraTarget);
		transform.position = new Vector3 (cameraSlide, cameraElevation, transform.position.z);
		//stargoose.transform.position = hitObject.point;

	}
}
