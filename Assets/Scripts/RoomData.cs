using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Room Data", order = 1)]
public class RoomData : ScriptableObject {
	public LevelEnemyData levelEnemyData;
	public int id;

	public RoomData roomToTheLeft;
	public RoomData roomToTheRight;
	public RoomData roomUp;
	public RoomData roomDown;

	public List<Wall> wallsTop = new List<Wall>();
	public List<Wall> wallsBottom = new List<Wall>();
	public List<Wall> wallsLeft = new List<Wall>();
	public List<Wall> wallsRight = new List<Wall>();
	public List<Wall> wallsCorners = new List<Wall>();
}

[System.Serializable]
public class Wall {
	public Vector2 position;
	public Vector2 frontPosition;
	public Vector2 scale;

	public Quaternion rotation;

	public string tile_sprite_name;
	public Color tile_sprite_color;

	[Header("Component")]
	public string component_sprite_name;
	public Color component_sprite_color;
	public Direcao door;
}