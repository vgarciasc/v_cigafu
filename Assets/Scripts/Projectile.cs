using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, Freezable {

	ParticleSystem trail;
	SpriteRenderer sr;

	LineOfShot line = null;
	Rigidbody2D rb;
	float velocity_multiplier = 15f;
	float distance = 150f;

	Vector2 last_position;
	Coroutine current_path = null;

	void Start() {
		last_position = transform.position;

		rb = this.GetComponent<Rigidbody2D>();
		sr = this.GetComponentInChildren<SpriteRenderer>();
		trail = this.GetComponentInChildren<ParticleSystem>();
	}

	void FixedUpdate() {
		if (last_position.x != transform.position.x ||
			last_position.y != transform.position.y) {

			distance -= Vector2.Distance(
				last_position,
				transform.position
			);

			if (distance <= 0f) {
				StartCoroutine(Destroy());
			}
		}

		last_position = transform.position;
	}

	void Update() {
		if (line != null) {
			line.Set_Tracking(this.transform.position);
		}

		if (rb.velocity.magnitude != 0) {
			float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
		}
	}
	
	public void Set_Direction(List<Vector2> points) {
		current_path = StartCoroutine(Manage_Path(points));
	}

	IEnumerator Manage_Path(List<Vector2> points) {
		AllMessager.Stop_On_Shot();

		rb = this.GetComponent<Rigidbody2D>();

		for (int i = 0; i < points.Count - 1; i++) {
			rb.velocity = (points[i + 1] - (Vector2) this.transform.position).normalized * velocity_multiplier;
			yield return new WaitUntil(() => Are_We_There_Yet(points[i + 1]));
			yield return new WaitUntil(() => rb.velocity != Vector2.zero);
			line.Next_Point_At_Tracking();
		}

		AllMessager.Continue_After_Shot();

		StartCoroutine(Destroy());
	}

	IEnumerator Destroy() {
		if (current_path != null) {
			StopCoroutine(current_path);
		}

		sr.enabled = false;
		trail.Stop();
		this.GetComponentInChildren<Collider2D>().enabled = false;

		yield return new WaitForSeconds(
			trail.main.startLifetime.constant
		);

		Destroy(this.gameObject);
	}

	public void Set_Line_Of_Shot(LineOfShot line) {
		this.line = line;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		GameObject go = coll.gameObject;

		if (go.layer == LayerMask.NameToLayer("Ricochettable")) {
			RaycastHit2D hit = Physics2D.Raycast(
				transform.position,
				rb.velocity,
				Mathf.Infinity
			);

			float magnitude = rb.velocity.magnitude;
			rb.velocity = Vector2.Reflect(
				hit.point - (Vector2) transform.position,
				hit.normal
			).normalized * magnitude;
		}

		if (go.tag == "Enemy") {
			Enemy enemy = go.GetComponentInChildren<Enemy>();
			StartCoroutine(Kill_Killable(enemy));
		}
	}

	IEnumerator Kill_Killable(Killable target) {
		Vector2 rb_aux = rb.velocity;
		rb.velocity = Vector2.zero;

		yield return target.Die();
		yield return new WaitUntil(() => !freezing);

		rb.velocity = rb_aux;
	}

	bool Are_We_There_Yet(Vector2 destination) {
		return Vector2.Distance(destination, this.transform.position) < 0.25f;
	}

	Vector2 last_velocity;
	bool freezing = false;
	public void Start_Freeze() {
		freezing = true;
		last_velocity = rb.velocity;
		rb.velocity = Vector2.zero;
	}

	public void Stop_Freeze() {
		freezing = false;
		rb.velocity = last_velocity;
	}
}
