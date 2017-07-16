using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Trappable, Killable {
	Rigidbody2D rb;
	SpriteRenderer sr;
	Player player;

	EnemyParticles particles;

	float speed = 1f;
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

	void OnTriggerEnter2D(Collider2D coll) {
		GameObject go = coll.gameObject;
		if (go.tag == "Arrow") {
			Die();
		}
	}

	//interfaces

	public void On_Trap() {
		can_move = false;
		rb.velocity = Vector2.zero;
		rb.bodyType = RigidbodyType2D.Static;
	}

	public void On_Untrap() {
		can_move = true;
		rb.bodyType = RigidbodyType2D.Dynamic;
	}

	public void Die() {
		sr.enabled = false;
		On_Trap();
		StartCoroutine(particles.death_explosion());
	}
}
