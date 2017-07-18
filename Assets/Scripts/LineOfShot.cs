using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfShot : MonoBehaviour {
	LineRenderer line;
	public int number;
	List<Vector2> now_tracking = new List<Vector2>();

	void Start() {
		line = this.GetComponent<LineRenderer>();
	}
	
	public static List<Vector2> Get_Trajectory(
		Vector2 position,
		Vector2 direction,
		float distance) {

		List<Vector2> points = new List<Vector2>() { position };
		Vector2 point_aux;
		Vector2 reflection;
		RaycastHit2D hit;
		float distance_aux;

		hit = Physics2D.Raycast(
			position,
			direction,
			distance,
			(1 << LayerMask.NameToLayer("Ricochettable"))
		);

		if (hit.collider == null) {
			points.Add(
				position + direction.normalized * distance
			);
			return points;
		}

		points.Add(hit.point);

		distance_aux = Vector2.Distance(
			points[points.Count - 1],
			hit.point
		);

		distance -= distance_aux;
		point_aux = hit.point;

		reflection = Vector2.Reflect(
			hit.point - position,
			hit.normal
		);

		while (true) {
			hit = Physics2D.Raycast(
				point_aux,
				reflection,
				distance,
				(1 << LayerMask.NameToLayer("Ricochettable"))
			);

			if (hit.collider == null) {
				points.Add(points[points.Count - 1] + reflection.normalized * distance);
				break;
			}

			distance_aux = Vector2.Distance(
				points[points.Count - 1],
				hit.point
			);

			points.Add(hit.point);
			distance -= distance_aux;

			reflection = Vector2.Reflect(
				hit.point - point_aux,
				hit.normal
			);

			point_aux = hit.point;
		}

		return points;
	}

	public void Set_Line(List<Vector2> points) {
		line.enabled = true;
		line.positionCount = points.Count;
		for (int i = 0; i < points.Count; i++) {
			line.SetPosition(i, points[i]);
		}
	}

	// public void Set_Tracking(Vector2 position) {
	// 	now_tracking[0] = position;
	// 	// Set_Line(now_tracking);
	// }

	// public void Next_Point_At_Tracking() {
	// 	now_tracking.RemoveAt(0);
	// }

	public void Deactivate() {
		line.enabled = false;
	}
}
