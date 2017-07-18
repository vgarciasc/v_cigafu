using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDeluxe : MonoBehaviour, Freezable {

	List<Vector2> next_points = new List<Vector2>();
	Rigidbody2D rb;
	float speed = 15f;
	float max_distance = 150f;

	[SerializeField]
	LineRenderer preview_shot;

	#region initialization
		void Start() {
			init();
		}

		void init() {
			rb = GetComponentInChildren<Rigidbody2D>();
		}
	#endregion

	public void Shoot_Arrow(Vector2 direction) {
		if (rb == null) {
			init();
		}

		rb.velocity = (direction).normalized * speed;
		Handle_Ricochet();
	}

	IEnumerator Destroy() {
		ParticleSystem trail = this.GetComponentInChildren<ParticleSystem>();

		this.GetComponentInChildren<SpriteRenderer>().enabled = false;

		trail.Stop();
		yield return new WaitForSeconds(
			trail.main.startLifetime.constant
		);

		Destroy(this.gameObject);
	}

	#region Update Handles
		void FixedUpdate() {
			Handle_Max_Distance();
			// Handle_Ricochet();
			Handle_Point_In_Velocity_Direction();
			Handle_Preview_Shot();
		}

		void Handle_Preview_Shot() {
			preview_shot.enabled = true;
			preview_shot.positionCount = next_points.Count;
			for (int i = 0; i < next_points.Count; i++) {
				preview_shot.SetPosition(i, next_points[i]);
			}
		}

		void Handle_Point_In_Velocity_Direction() {
			if (rb.velocity.magnitude != 0) {
				float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
			}
		}

		void Handle_Ricochet() {
			next_points = LineOfShot.Get_Trajectory(
				this.transform.position,
				rb.velocity,
				10f
			);
		}

		Vector2 last_position;
		void Handle_Max_Distance() {
			if (last_position.x != transform.position.x ||
				last_position.y != transform.position.y) {

				max_distance -= Vector2.Distance(
					last_position,
					transform.position
				);

				if (max_distance <= 0f) {
					StartCoroutine(Destroy());
				}
			}

			last_position = transform.position;
		}
	#endregion

	#region colliders
		void OnTriggerEnter2D(Collider2D coll) {
			GameObject go = coll.gameObject;

			if (go.tag == "Wall" || go.tag == "Enemy") {
				if (next_points.Count > 2) {
					rb.velocity = (next_points[2] - next_points[1]).normalized * speed;
					Handle_Ricochet();
				}
			}
		}
	#endregion

	#region Freezable
		bool frozen = false;
		Vector2 frozen_velocity;

		public void Start_Freeze() {
			frozen = true;
			frozen_velocity = rb.velocity;
			rb.velocity = Vector2.zero;
		}

		public void Stop_Freeze() {
			frozen = false;
			rb.velocity = frozen_velocity;
		}
	#endregion
}
