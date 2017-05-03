using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRepeater : MonoBehaviour {


	[SerializeField] private float levelLength = 100.0f;
	// Use this for initialization

	public void OnTriggerExit(Collider collider){
		if (collider.GetComponent<Terrain>() == null){
			collider.gameObject.transform.position += Vector3.forward * levelLength;
		} else {
			collider.gameObject.transform.position += Vector3.forward * collider.gameObject.GetComponent<Terrain>().terrainData.size.z*2;
		}
		
	}
}
