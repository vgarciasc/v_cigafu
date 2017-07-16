using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticles : MonoBehaviour {
	public ParticleSystem explosion;

	public IEnumerator death_explosion() {
		explosion.Play();

		yield return new WaitForSeconds(
			explosion.main.duration +
			explosion.main.startLifetime.constant
		);

		explosion.Stop();
	}
}
