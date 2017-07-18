using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, Freezable {

	ParticleSystem trail;
	SpriteRenderer sr;

	LineOfShot line;
	Rigidbody2D rb;
	float velocity_multiplier = 15f;
	float distance = 30f;

	Vector2 last_position;
	Coroutine current_path;

	LineRenderer preview_trajectory;

	#region start
		void Start() {
			last_position = transform.position;

			init_references();
		}

		void init_references() {
			rb = this.GetComponent<Rigidbody2D>();
			sr = this.GetComponentInChildren<SpriteRenderer>();
			trail = this.GetComponentInChildren<ParticleSystem>();
			preview_trajectory = this.GetComponentInChildren<LineRenderer>();
		}
	#endregion

	#region update and handlers
		void FixedUpdate() {
			Handle_Max_Distance();
		}

		void Update() {
			Handle_Point_to_Velocity(rb.velocity);
		}

		void Handle_Point_to_Velocity(Vector2 velocity) {
			if (velocity.magnitude != 0) {
				float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
			}
		}

		void Handle_Max_Distance() {
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

		void Handle_Preview_Trajectory() {
			var points = LineOfShot.Get_Trajectory(
				this.transform.position,
				rb.velocity,
				2.5f
			);

			preview_trajectory.enabled = true;
			preview_trajectory.positionCount = points.Count;
			for (int i = 0; i < points.Count; i++) {
				preview_trajectory.SetPosition(i, points[i]);
			}
		}
	#endregion

	#region destroy
		IEnumerator Destroy() {
			if (current_path != null) {
				StopCoroutine(current_path);
			}

			AllMessager.Continue_After_Shot();

			sr.enabled = false;
			preview_trajectory.enabled = false;
			trail.Stop();
			this.GetComponentInChildren<Collider2D>().enabled = false;

			yield return new WaitForSeconds(
				trail.main.startLifetime.constant
			);

			Destroy(this.gameObject);
		}
	#endregion

	#region arrow
		public void Start_Shot(Vector2 direction) {
			AllMessager.Stop_On_Shot();

			if (rb == null) {
				init_references();
			}

			Set_Velocity(direction.normalized * velocity_multiplier);
		}

		public void Set_Line_Of_Shot(LineOfShot line) {
			this.line = line;
		}

		void Set_Velocity(Vector2 velocity) {
			if (freezing) {
				Handle_Point_to_Velocity(velocity);
				rb.velocity = Vector2.zero;
				last_velocity = velocity;
			}
			else {
				rb.velocity = velocity;
			}
		}
	#endregion

	#region collider
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
			Set_Velocity(Vector2.zero);

			yield return target.Die();

			Set_Velocity(rb_aux);
		}
	#endregion

	#region freeze
		Vector2 last_velocity;
		public static bool freezing = false;
		public void Start_Freeze() {
			freezing = true;
			last_velocity = rb.velocity;
			rb.velocity = Vector2.zero;
		}

		public void Stop_Freeze() {
			freezing = false;
			rb.velocity = last_velocity;
		}
	#endregion
}
