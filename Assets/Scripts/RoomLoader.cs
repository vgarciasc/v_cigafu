using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoader : MonoBehaviour {
	public LevelEnemyLoader enemyLoader;

	public RoomData currentRoom;
	public List<WallTile> wallsTop = new List<WallTile>();
	public List<WallTile> wallsBottom = new List<WallTile>();
	public List<WallTile> wallsLeft = new List<WallTile>();
	public List<WallTile> wallsRight = new List<WallTile>();
	public List<WallTile> wallsCorners = new List<WallTile>();

	void Start() {
		Unserialize(currentRoom);
	}

	public void Unserialize(RoomData room) {
		Unserialize_Wall_Tile_Data(wallsTop, room.wallsTop);
		Unserialize_Wall_Tile_Data(wallsBottom, room.wallsBottom);
		Unserialize_Wall_Tile_Data(wallsRight, room.wallsRight);
		Unserialize_Wall_Tile_Data(wallsLeft, room.wallsLeft);
		Unserialize_Wall_Tile_Data(wallsCorners, room.wallsCorners);

		ExplorerManager.Get_Explorer_Manager().dialogGirl.SetActive(room.name == "room_main");
	}

	void Unserialize_Wall_Tile_Data(
		List<WallTile> real_walls,
		List<Wall> data_walls) {

		for (int i = 0; i < real_walls.Count; i++) {
			real_walls[i].Apply_Data(data_walls[i]);
		}
	}

	public void Serialize() {
		currentRoom.wallsTop = 
			Serialize_Wall_Tile_Data(wallsTop, currentRoom.wallsTop, enemyLoader.toLoad);
		currentRoom.wallsBottom = 
			Serialize_Wall_Tile_Data(wallsBottom, currentRoom.wallsBottom, enemyLoader.toLoad);
		currentRoom.wallsRight = 
			Serialize_Wall_Tile_Data(wallsRight, currentRoom.wallsRight, enemyLoader.toLoad);
		currentRoom.wallsLeft = 
			Serialize_Wall_Tile_Data(wallsLeft, currentRoom.wallsLeft, enemyLoader.toLoad);
		currentRoom.wallsCorners = 
			Serialize_Wall_Tile_Data(wallsCorners, currentRoom.wallsCorners, enemyLoader.toLoad);
	}

	List<Wall> Serialize_Wall_Tile_Data(
		List<WallTile> real_walls,
		List<Wall> data_walls,
		LevelEnemyData level_enemy_data) {

		data_walls.Clear();

		for (int i = 0; i < real_walls.Count; i++) {
			Wall aux = new Wall();
			aux.position = real_walls[i].transform.position;
			aux.rotation = real_walls[i].transform.rotation;
			aux.frontPosition = real_walls[i].frontPosition.position;
			aux.scale = real_walls[i].transform.localScale;
			aux.tile_sprite_color = real_walls[i].wallSprite.color;
			aux.component_sprite_color = real_walls[i].componentSprite.color;
			aux.door = real_walls[i].doorDirection;

			if (real_walls[i].componentSprite.sprite != null) {
				aux.component_sprite_name = real_walls[i].componentSprite.sprite.name;
			}
			if (real_walls[i].wallSprite.sprite != null) {
				aux.tile_sprite_name = real_walls[i].wallSprite.sprite.name;
			}

			data_walls.Add(aux);
		}
		
		return data_walls;
	}
}