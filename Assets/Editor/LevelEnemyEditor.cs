using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelEnemyLoader))]
public class LevelEnemyEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		
		LevelEnemyLoader loader = (LevelEnemyLoader) target;
		
		if (GUILayout.Button("Load")) {
			Undo.RecordObject(loader.toLoad, "Loaded LevelEnemyData");
			loader.toLoad.enemies = new List<Vector2>();
			
			foreach (Transform child in loader.transform) {
				loader.toLoad.enemies.Add(new Vector2(child.position.x,
					child.position.y));
			}
		}
	}
}
