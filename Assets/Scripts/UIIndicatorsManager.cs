using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIIndicatorsManager : MonoBehaviour {

	PlayerItemManager playerItems;

	public Text	trapCount;
	public Text	enemiesCount;
	public Text	arrowCount;

	void Start() {
		playerItems = (PlayerItemManager) HushPuppy.safeFindComponent("Player", "PlayerItemManager");
	}

	void Update() {
		trapCount.text = "x" + 
			(playerItems.trapMaxCount -
			GameObject.FindGameObjectsWithTag("Trap").Length).ToString();
		enemiesCount.text = "x" + 
			(GameObject.FindGameObjectsWithTag("Enemy").Length).ToString();
		arrowCount.text = "x" + 
			(playerItems.arrowCount).ToString();
	}

}
