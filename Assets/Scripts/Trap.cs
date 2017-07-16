using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, StopOnShot {
	bool closed = false;
	Animator anim;

	void Start() {
		anim = this.GetComponent<Animator>();
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (closed) return;

		GameObject go = coll.gameObject;
		if (go.tag == "Player") {
			Player player = go.GetComponentInChildren<Player>();
			StartCoroutine(Trap_Cooldown(player));
		}
		else if (go.tag == "Enemy") {
			Enemy enemy = go.GetComponentInChildren<Enemy>();
			StartCoroutine(Trap_Cooldown(enemy));
		}
	}

	IEnumerator Trap_Cooldown(Trappable trappable) {
		trappable.On_Trap();
		closed = true;
		anim.SetBool("closed", closed);

		yield return new WaitForSeconds(5f);

		if (trappable != null) {
			trappable.On_Untrap();
		}
		anim.SetBool("closed", false);

		yield return new WaitForSeconds(0.5f);

		yield return new WaitUntil(() => !shot_freeze);
		closed = false;
	}

	bool shot_freeze = false;

	public void Stop_On_Shot() {
		shot_freeze = true;
	}

	public void Continue_After_Shot() {
		shot_freeze = false;
	}
}
