using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfShot : MonoBehaviour {
	LineRenderer line;
	Vector3 mouse_pos;
	public int number;
	List<Vector2> now_tracking = new List<Vector2>();

	void Start() {
		line = this.GetComponent<LineRenderer>();
	}
	
	public List<Vector2> Activate(Vector2 position) {
		List<Vector2> points = new List<Vector2>() { position };
		Vector2 point_aux;
		Vector2 reflection;
		RaycastHit2D hit;
		float distance = 30f;
		Vector2 mouse_pos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition) - position;

		hit = Physics2D.Raycast(
			position,
			mouse_pos,
			Mathf.Infinity
		);

		points.Add(hit.point);
		point_aux = hit.point;

		reflection = Vector2.Reflect(
			hit.point - position,
			hit.normal
		);

		while (true) {
			hit = Physics2D.Raycast(
				hit.point,
				reflection,
				Mathf.Infinity
			);

			float distance_aux = Vector2.Distance(
				points[points.Count - 1],
				hit.point
			);
			if (distance > distance_aux) {
				points.Add(hit.point);
				distance -= distance_aux;
			}
			else {
				Vector2 aux;
				aux = points[points.Count - 1];
				points.Add(new Vector2(
					points[points.Count - 1].x + (hit.point.x - points[points.Count - 1].x) * (distance / distance_aux),
					points[points.Count - 1].y + (hit.point.y - points[points.Count - 1].y) * (distance / distance_aux)
				));
				break;
			}

			reflection = Vector2.Reflect(
				hit.point - point_aux,
				hit.normal
			);

			point_aux = hit.point;
		}

		now_tracking = points;
		return points;
	}

	public void Set_Line(List<Vector2> points) {
		line.enabled = true;
		line.positionCount = points.Count;
		for (int i = 0; i < points.Count; i++) {
			line.SetPosition(i, points[i]);
		}
	}

	public void Set_Tracking(Vector2 position) {
		now_tracking[0] = position;
		Set_Line(now_tracking);
	}

	public void Next_Point_At_Tracking() {
		now_tracking.RemoveAt(0);
	}

	public void Deactivate() {
		line.enabled = false;
	}
}
