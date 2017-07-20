using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direcao { NONE, UP, DOWN, LEFT, RIGHT };
public class ExplorerManager : MonoBehaviour {

	public List<string> cleared_rooms = new List<string>();
	
	[SerializeField]
	RoomLoader roomLoader;
	[SerializeField]
	LevelEnemyLoader enemyLoader;

	Player player;

	public GameObject dialogGirl;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
	}

	public static ExplorerManager Get_Explorer_Manager() {
		return (ExplorerManager) HushPuppy.safeFindComponent("GameController", "ExplorerManager");
	}

	public void Change_Room(Vector2 new_pos, Direcao dir) {
		if (!player.can_change_rooms) {
			return;
		}
		else {
			StartCoroutine(player.Cooldown_Change_Rooms());
		}

		RoomData next_room = null;
		Vector3 offset = Vector2.zero;
		
		switch (dir) {
			case Direcao.UP:
				next_room = roomLoader.currentRoom.roomUp;
				new_pos = new Vector2(new_pos.x, -new_pos.y);
				offset = Vector2.down;
				break;
			case Direcao.RIGHT:
				next_room = roomLoader.currentRoom.roomToTheRight;
				new_pos = new Vector2(-new_pos.x, new_pos.y);
				offset = Vector2.left;
				break;
			case Direcao.LEFT:
				next_room = roomLoader.currentRoom.roomToTheLeft;
				new_pos = new Vector2(-new_pos.x, new_pos.y);
				offset = Vector2.left;
				break;
			case Direcao.DOWN:
				next_room = roomLoader.currentRoom.roomDown;
				new_pos = new Vector2(new_pos.x, -new_pos.y);
				offset = Vector2.down;
				break;
		}

		if (next_room == null) {
			// Debug.Log("Trying to access room " + dir + " but none found.");
			return;
		}

		if (!cleared_rooms.Contains(next_room.name)) {
			enemyLoader.Spawn(next_room.levelEnemyData);
		}


		roomLoader.Unserialize(next_room);

		roomLoader.currentRoom = next_room;

		Update_Player_New_Room(offset);

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("RoomAlteration")) {
			Destroy(go);
		}

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Arrow")) {
			StartCoroutine(go.GetComponentInChildren<Projectile>().Destroy());
		}

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Trap")) {
			Destroy(go);
		}

		Enemy.Reset_Frozen();

		// Load_Room_Alterations(next_room.id);
	}
	
	public void Clear_Current_Room() {
		cleared_rooms.Add(roomLoader.currentRoom.name);
	}

	void Update_Player_New_Room(Vector2 new_pos) {
		player.transform.position = new Vector2(
			new_pos.x * player.transform.position.x,
			new_pos.y * player.transform.position.y
		);	

		PlayerItemManager.Get_Player_Item_Manager().Reset_Arrows();
	}

	// void Load_Room_Alterations(int room_ID) {
	// 	//this will only work for blood and needs to be redone.


	// }

	// void Store_Room_Alterations(int room_ID) {
	// 	foreach (GameObject go in GameObject.FindGameObjectsWithTag("RoomAlteration")) {
	// 		RoomAlteration aux = new RoomAlteration();
	// 		aux.room_ID = room_ID;
	// 		aux.scale = go.transform.localScale;
	// 		aux.position = go.transform.position;
	// 		aux.rotation = go.transform.rotation;
	// 		aux.sprite_name = go.GetComponentInChildren<SpriteRenderer>().sprite.name;
	// 		aux.sprite_color = go.GetComponentInChildren<SpriteRenderer>().color;

	// 	}
	// }
}
