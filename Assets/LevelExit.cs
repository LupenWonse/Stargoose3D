using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour {


	public bool enabled = false;
	// If the player enters this volume the level is complete and we can move to the next level
	public void OnTriggerEnter(Collider collider){
		if (enabled && collider.gameObject.CompareTag("Player")){
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex+1);
		}
	}

}
