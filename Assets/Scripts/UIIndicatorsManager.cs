using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIIndicatorsManager : MonoBehaviour {

	Player player;

	public Text	trapCount;
	public Text	enemiesCount;
	public Text	arrowCount;

	void Start() {
		player = (Player) HushPuppy.safeFindComponent("Player", "Player");
	}

	void Update() {
		trapCount.text = "x" + 
			(player.trapMaxCount -
			GameObject.FindGameObjectsWithTag("Trap").Length).ToString();
		enemiesCount.text = "x" + 
			(GameObject.FindGameObjectsWithTag("Enemy").Length).ToString();
		arrowCount.text = "x" + 
			(player.arrowCount).ToString();
	}

}
