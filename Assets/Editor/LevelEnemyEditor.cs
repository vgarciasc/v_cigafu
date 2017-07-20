using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelEnemyLoader))]
public class LevelEnemyEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		
		LevelEnemyLoader loader = (LevelEnemyLoader) target;
		
		if (GUILayout.Button("Store Enemy Data")) {
			EditorUtility.SetDirty(loader.toLoad);
			Undo.RecordObject(loader.toLoad, "Loaded LevelEnemyData");

			loader.toLoad.enemies = new List<Vector2>();

			foreach (Transform child in loader.enemyContainer.transform) {
				loader.toLoad.enemies.Add(new Vector2(child.position.x,
					child.position.y));
			}

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		if (GUILayout.Button("Load Enemy Data")) {
			Undo.RecordObject(loader.toLoad, "Loaded Enemies");
			loader.Spawn(loader.toLoad);
		}

		if (GUILayout.Button("Clear Substitutes")) {
			Undo.RecordObject(loader.toLoad, "Cleared Substitutes");
			loader.Despawn_Enemies();
		}
	}
}
