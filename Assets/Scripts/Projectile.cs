using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	ParticleSystem trail;
	SpriteRenderer sr;

	LineOfShot line = null;
	Rigidbody2D rb;
	float velocity_multiplier = 15f;

	void Start() {
		rb = this.GetComponent<Rigidbody2D>();
		sr = this.GetComponentInChildren<SpriteRenderer>();
		trail = this.GetComponentInChildren<ParticleSystem>();
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
	
	public IEnumerator Set_Direction(List<Vector2> points) {
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
			go.GetComponentInChildren<StopOnShot>().Stop_On_Shot();
		}
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {
			go.GetComponentInChildren<StopOnShot>().Stop_On_Shot();
		}
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Trap")) {
			go.GetComponentInChildren<StopOnShot>().Stop_On_Shot();
		}

		rb = this.GetComponent<Rigidbody2D>();

		for (int i = 0; i < points.Count - 1; i++) {
			rb.velocity = (points[i + 1] - (Vector2) this.transform.position).normalized * velocity_multiplier;
			yield return new WaitUntil(() => Are_We_There_Yet(points[i + 1]));
			yield return new WaitUntil(() => rb.velocity != Vector2.zero);
			line.Next_Point_At_Tracking();
		}

		line.Deactivate();

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
			go.GetComponentInChildren<StopOnShot>().Continue_After_Shot();
		}
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {
			go.GetComponentInChildren<StopOnShot>().Continue_After_Shot();
		}
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Trap")) {
			go.GetComponentInChildren<StopOnShot>().Continue_After_Shot();
		}

		StartCoroutine(Destroy());
	}

	IEnumerator Destroy() {
		sr.enabled = false;
		trail.Stop();

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
		if (go.tag == "Enemy") {
			Enemy enemy = go.GetComponentInChildren<Enemy>();
			StartCoroutine(Kill_Killable(enemy));
		}
	}

	IEnumerator Kill_Killable(Killable target) {
		Vector2 rb_aux = rb.velocity;
		rb.velocity = Vector2.zero;

		yield return target.Die();

		rb.velocity = rb_aux;
	}

	bool Are_We_There_Yet(Vector2 destination) {
		return Vector2.Distance(destination, this.transform.position) < 0.25f;
	}
}
