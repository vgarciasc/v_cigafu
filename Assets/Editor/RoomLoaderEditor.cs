using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomLoader))]
public class RoomLoaderEditor : Editor {

	public override void OnInspectorGUI() {

		base.DrawDefaultInspector();

		RoomLoader loader = (RoomLoader) target;

		if (GUILayout.Button("Unserialize")) {
			Undo.RecordObject(loader, "Unserialize Room");
			loader.Unserialize(loader.currentRoom);
		}

		if (GUILayout.Button("Serialize")) {
			EditorUtility.SetDirty(loader.currentRoom);

			Undo.RecordObject(loader.currentRoom, "Serialize Room");
			loader.Serialize();

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}
}
