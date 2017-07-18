using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMessager : MonoBehaviour {
	public static void Stop_On_Shot() {
		// foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
		// 	go.GetComponentInChildren<StopOnShot>().Stop_On_Shot();
		// }
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {
			go.GetComponentInChildren<StopOnShot>().Stop_On_Shot();
		}
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Trap")) {
			go.GetComponentInChildren<StopOnShot>().Stop_On_Shot();
		}
	}

	public static void Continue_After_Shot() {
		// foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
		// 	go.GetComponentInChildren<StopOnShot>().Continue_After_Shot();
		// }
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {
			go.GetComponentInChildren<StopOnShot>().Continue_After_Shot();
		}
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Trap")) {
			go.GetComponentInChildren<StopOnShot>().Continue_After_Shot();
		}
	}

	public static void Start_Freeze() {
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {
			go.GetComponentInChildren<Freezable>().Start_Freeze();
		}
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Arrow")) {
			go.GetComponentInChildren<Freezable>().Start_Freeze();
		}
		// foreach (GameObject go in GameObject.FindGameObjectsWithTag("Trap")) {
		// 	go.GetComponentInChildren<Freezable>().Start_Freeze();
		// }
	}

	public static void Stop_Freeze() {
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {
			go.GetComponentInChildren<Freezable>().Stop_Freeze();
		}
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Arrow")) {
			go.GetComponentInChildren<Freezable>().Stop_Freeze();
		}
		// foreach (GameObject go in GameObject.FindGameObjectsWithTag("Trap")) {
		// 	go.GetComponentInChildren<Freezable>().Start_Freeze();
		// }
	}
}
