using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRepeater : MonoBehaviour {


	[SerializeField] private float levelLength = 100.0f;
	// Use this for initialization

	public void OnTriggerExit(Collider collider){
		print("Hit");
		collider.gameObject.transform.position += Vector3.forward * levelLength;
	}
}
