using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Level Enemy Data", order = 1)]
public class LevelEnemyData : ScriptableObject {
	public List<Vector2> enemies = new List<Vector2>();
}