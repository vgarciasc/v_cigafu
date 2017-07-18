using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Trappable, Killable, StopOnShot, Freezable {
	Rigidbody2D rb;
	SpriteRenderer sr;
	Player player;

	EnemyParticles particles;

	public GameObject bloodPrefab;

	float speed = 0.15f;
	bool can_move = true;

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
		if (!can_move) return;

		rb.velocity = (player.transform.position - this.transform.position).normalized * speed;
		sr.flipX = rb.velocity.x > 0;
	}

	//interfaces

	public void On_Trap() {
		can_move = false;
		rb.velocity = Vector2.zero;
		rb.bodyType = RigidbodyType2D.Static;
	}

	public void On_Untrap() {
		if (shot_freeze) return;
		can_move = true;

		if (rb != null) {
			rb.bodyType = RigidbodyType2D.Dynamic;
		}
	}

	public IEnumerator Die() {
		sr.enabled = false;
		GameObject blood = Instantiate(bloodPrefab, this.transform.position, Quaternion.identity);
		blood.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 180f));
		yield return particles.death_explosion();
		Destroy(this.gameObject);
	}

	bool shot_freeze = false;

	public void Stop_On_Shot() {
		can_move = false;
		shot_freeze = true;
		rb.velocity = Vector2.zero;
		rb.bodyType = RigidbodyType2D.Static;
	}

	public void Continue_After_Shot() {
		shot_freeze = false;
		can_move = true;
		rb.bodyType = RigidbodyType2D.Dynamic;
	}

	public void Start_Freeze() {
		Stop_On_Shot();
	}

	public void Stop_Freeze() {
		Continue_After_Shot();
	}
}
