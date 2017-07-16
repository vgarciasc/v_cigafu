using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	LineOfShot line = null;
	Rigidbody2D rb;
	float velocity_multiplier = 15f;

	void Start() {
		rb = this.GetComponent<Rigidbody2D>();
	}

	void Update() {
		if (line != null) {
			line.Set_Tracking(this.transform.position);
		}
	}
	
	public IEnumerator Set_Direction(List<Vector2> points) {
		rb = this.GetComponent<Rigidbody2D>();

		for (int i = 0; i < points.Count - 1; i++) {
			rb.velocity = (points[i + 1] - (Vector2) this.transform.position).normalized * velocity_multiplier;
			yield return new WaitUntil(() => Are_We_There_Yet(points[i + 1]));
			line.Next_Point_At_Tracking();
		}

		line.Deactivate();
		Destroy(this.gameObject);
	}

	public void Set_Line_Of_Shot(LineOfShot line) {
		this.line = line;
	}

	bool Are_We_There_Yet(Vector2 destination) {
		return Vector2.Distance(destination, this.transform.position) < 0.25f;
	}
}
