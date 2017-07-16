using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
	public void Reset_Scene() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
	}
}
