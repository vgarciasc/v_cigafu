using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, Trappable {
	public Transform cat;
	public LineOfShot line;
	public GameObject projectilePrefab;
	public GameObject trapPrefab;
	public Rigidbody2D rb;
	float speed = 10f;
	bool can_move = true;

	void Update() {
		Handle_Cannon_Rotation();
		Handle_Line_Of_Shot();	
		Handle_Movement();
		Handle_Shooting();
		Handle_Put_Trap();
	}

	void Handle_Movement() {
		if (!can_move) return;

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		// cat.position += new Vector3(
		// 	horizontal,
		// 	vertical
		// ) * speed;

		rb.velocity = new Vector2(horizontal, vertical) * speed;
	}

	void Handle_Cannon_Rotation() {
		Vector2 cannon_pos = Camera.main.WorldToScreenPoint(cat.transform.position);
		Vector2 mouse_pos = Input.mousePosition;

		mouse_pos.x = mouse_pos.x - cannon_pos.x;
		mouse_pos.y = mouse_pos.y - cannon_pos.y;
		float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
		
		cat.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
	}

	void Handle_Line_Of_Shot() {
		if (Input.GetMouseButton(0)) {
			line.Set_Line(line.Activate(cat.position));
		}
		if (Input.GetMouseButtonUp(0)) {
			line.Deactivate();
		}
	}

	void Handle_Shooting() {
		if (Input.GetMouseButtonUp(0)) {
			GameObject go = Instantiate(projectilePrefab, cat.position, Quaternion.identity);
			Projectile prj = go.GetComponent<Projectile>();
			List<Vector2> aux = new List<Vector2>();
			aux.AddRange(line.Activate(cat.position));
			prj.Set_Line_Of_Shot(line);

			StartCoroutine(prj.Set_Direction(aux));
		}
	}

	void Handle_Put_Trap() {
		if (Input.GetMouseButtonDown(1)) {
			GameObject go = Instantiate(trapPrefab,
				(Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition),
				Quaternion.identity);
		}
	}

	public void On_Trap() {
		can_move = false;
		rb.velocity = Vector2.zero;
		rb.bodyType = RigidbodyType2D.Static;
	}

	public void On_Untrap() {
		can_move = true;
		rb.bodyType = RigidbodyType2D.Dynamic;
	}
}
