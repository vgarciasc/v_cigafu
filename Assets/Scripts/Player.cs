using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, Trappable, StopOnShot {
	public Transform cat;
	public LineOfShot line;
	public GameObject projectilePrefab;
	public GameObject trapPrefab;
	public Rigidbody2D rb;
	public SpriteRenderer sr;
	float speed = 10f;
	bool can_move = true;
	bool is_dead = false;

	[SerializeField]
	PlayerItemManager itemManager;

	public Image deathScreen;
	public GameObject freezeScreen;

	void Update() {
		if (is_dead) return;

		Handle_Cannon_Rotation();
		Handle_Line_Of_Shot();	
		Handle_Movement();
		Handle_Shooting();
		Handle_Put_Trap();
		Handle_Freeze();
	}

	bool freezing = false;

	void Handle_Freeze() {
		if (Input.GetKeyDown(KeyCode.F)) {
			if (!freezing) AllMessager.Start_Freeze();
			else AllMessager.Stop_Freeze();
			freezing = !freezing;
			freezeScreen.SetActive(freezing);
		}
	}

	void Handle_Movement() {
		if (!can_move) return;

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

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
			line.Set_Line(
				LineOfShot.Get_Trajectory(cat.position,
				Camera.main.ScreenToWorldPoint(Input.mousePosition) - cat.position,
				30f
			));
		}
		if (Input.GetMouseButtonUp(0)) {
			line.Deactivate();
		}
	}

	void Handle_Shooting() {
		if (Input.GetMouseButtonUp(0) && itemManager.arrowCount > 0) {
			GameObject go = Instantiate(projectilePrefab, cat.position, Quaternion.identity);
			Projectile prj = go.GetComponent<Projectile>();
			List<Vector2> aux = new List<Vector2>();
			aux.AddRange(
				LineOfShot.Get_Trajectory(cat.position,
				Camera.main.ScreenToWorldPoint(Input.mousePosition) - cat.position,
				30f
			));
			prj.Set_Line_Of_Shot(line);
			itemManager.arrowCount --;

			line.Deactivate();
			prj.Start_Shot(
				Camera.main.ScreenToWorldPoint(Input.mousePosition) - cat.position
			);
		}
	}

	void Handle_Put_Trap() {
		if (Input.GetMouseButtonDown(1) && itemManager.trapCount > 0) {
			GameObject go = Instantiate(trapPrefab,
				(Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition),
				Quaternion.identity);
			itemManager.trapCount--;
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

	public void Stop_On_Shot() {
		can_move = false;
		rb.velocity = Vector2.zero;
		rb.bodyType = RigidbodyType2D.Static;
	}

	public void Continue_After_Shot() {
		can_move = true;
		rb.bodyType = RigidbodyType2D.Dynamic;
	}

	void OnTriggerEnter2D(Collider2D target) {
		if (target.gameObject.tag == "Enemy") {
			sr.enabled = false;
			Death();
		}
	}

	void Death() {
		deathScreen.enabled = true;
		is_dead = true;
	}

	public bool can_change_rooms = true;
	public IEnumerator Cooldown_Change_Rooms() {
		can_change_rooms = false;

		yield return new WaitForSeconds(0.25f);

		can_change_rooms = true;
	}
}
