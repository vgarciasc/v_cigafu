using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Trappable, Killable, StopOnShot, Freezable {
	Rigidbody2D rb;
	SpriteRenderer sr;
	Player player;

	EnemyParticles particles;

	public GameObject bloodPrefab;

	bool trapped = false;
	float speed = 0.15f;

	void Start() {
		init();
	}

	void init() {
		rb = this.GetComponentInChildren<Rigidbody2D>();
		sr = this.GetComponentInChildren<SpriteRenderer>();
		particles = this.GetComponentInChildren<EnemyParticles>();

		player = (Player) HushPuppy.safeFindComponent("Player", "Player");
	}

	void Update() {
		Handle_Movement();
	}

	void Handle_Movement() {
		if (!Can_Move()) return;

		rb.velocity = (player.transform.position - this.transform.position).normalized * speed;
		sr.flipX = rb.velocity.x > 0;
	}

	//interfaces

	bool Can_Move() {
		return (!time_frozen && !trapped && !shot_frozen);
	}

	public void On_Trap() {
		trapped = true;
		rb.velocity = Vector2.zero;
		rb.bodyType = RigidbodyType2D.Static;
	}

	public void On_Untrap() {
		trapped = false;
		rb.bodyType = RigidbodyType2D.Dynamic;
	}

	public IEnumerator Die() {
		sr.enabled = false;
		GameObject blood = Instantiate(bloodPrefab, this.transform.position, Quaternion.identity);
		blood.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 180f));
		yield return particles.death_explosion();

		if (GameObject.FindGameObjectsWithTag("Enemy").Length == 1) {
			//last one to die
			ExplorerManager.Get_Explorer_Manager().Clear_Current_Room();
		}

		Destroy(this.gameObject);
	}

	public static void Reset_Frozen() {
		shot_frozen = false;
		time_frozen = false;
	}

	public static bool shot_frozen = false;
	public static bool time_frozen = false;
	public void Stop_On_Shot() {
		shot_frozen = true;
		rb.velocity = Vector2.zero;
		rb.bodyType = RigidbodyType2D.Static;
	}

	public void Continue_After_Shot() {
		shot_frozen = false;
		rb.bodyType = RigidbodyType2D.Dynamic;
	}

	public void Start_Freeze() {
		time_frozen = true;
		rb.velocity = Vector2.zero;
		rb.bodyType = RigidbodyType2D.Static;
	}

	public void Stop_Freeze() {
		time_frozen = false;
		rb.bodyType = RigidbodyType2D.Dynamic;
	}
}
