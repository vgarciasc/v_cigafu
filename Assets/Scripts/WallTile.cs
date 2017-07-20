using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTile : MonoBehaviour {

	public SpriteRenderer wallSprite;
	public SpriteRenderer componentSprite;

	public Direcao doorDirection = Direcao.NONE;
	public Transform frontPosition;

	public void Apply_Data(Wall data) {
		doorDirection = data.door;
		transform.position = data.position;
		transform.rotation = data.rotation;
		transform.localScale = data.scale;
		frontPosition.position = data.frontPosition;

		wallSprite.sprite = Resources.Load<Sprite>("Sprites\\Tiles\\" + data.tile_sprite_name);
		wallSprite.color = data.tile_sprite_color;

		componentSprite.sprite = Resources.Load<Sprite>("Sprites\\Tiles\\" + data.component_sprite_name);
		componentSprite.color = data.component_sprite_color;		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		var target = coll.gameObject;
		if (coll.tag == "Player" && doorDirection != Direcao.NONE) {
			ExplorerManager.Get_Explorer_Manager().Change_Room(frontPosition.position, doorDirection);
		}
	}
}
