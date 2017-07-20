using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnemyLoader : MonoBehaviour {
	public LevelEnemyData toLoad;
	public GameObject enemyPrefab;
	public Transform enemyContainer;

	void Start () {
		// var levelManager = LevelManager.getLevelManager();
		// if (levelManager != null) {
		// 	toLoad = levelManager.levelEnemyData;
		// }
		Spawn(toLoad);
	}

	public void Spawn(LevelEnemyData enemyData) {
		Despawn_Enemies();
		
		if (enemyData == null) {
			return;
		}

		foreach (Vector2 vec in enemyData.enemies) {
			GameObject go = Instantiate(enemyPrefab, vec, Quaternion.identity);
			go.transform.SetParent(enemyContainer);
		}
	}

	public void Despawn_Enemies() {
		foreach (Transform tr in enemyContainer) {
			if (!Application.isPlaying) {
				DestroyImmediate(tr.gameObject);
			}
			else {
				Destroy(tr.gameObject);
			}
		}
	}
}
