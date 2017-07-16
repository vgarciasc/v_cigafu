using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	bool used = false;

	public LevelEnemyData levelEnemyData;

	public static LevelManager getLevelManager() {
		return (LevelManager) HushPuppy.safeFindComponent("LevelManager", "LevelManager");
	}

	void Start() {
		DontDestroyOnLoad(this.gameObject);

		used = true;
		SceneManager.sceneLoaded += Check_If_Destroy;
	}

	void Check_If_Destroy(Scene scene, LoadSceneMode mode) {
		if (scene.name == "Title" && used) {
			SceneManager.sceneLoaded -= Check_If_Destroy;
			Destroy(this.gameObject);
		}
	}

	public void Set_Data(LevelEnemyData levelEnemyData) {
		this.levelEnemyData = levelEnemyData;
		SceneManager.LoadScene("Main");
	}
}
