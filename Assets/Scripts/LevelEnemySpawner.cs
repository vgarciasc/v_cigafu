using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnemySpawner : MonoBehaviour {
	public LevelEnemyData toSpawn;
	public GameObject enemyPrefab;

	void Start () {
		var levelManager = LevelManager.getLevelManager();
		if (levelManager != null) {
			toSpawn = levelManager.levelEnemyData;
		}
		Spawn();
	}

	public void Spawn() {
		HushPuppy.destroyChildren(this.gameObject);

		foreach (Vector2 vec in toSpawn.enemies) {
			Instantiate(enemyPrefab, vec, Quaternion.identity);
		}
	}
}
