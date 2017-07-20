using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemManager : MonoBehaviour {

	public float trapMaxCount = 3;
	public float trapCount;
	public float arrowMaxCount = 1;
	public float arrowCount;

	public static PlayerItemManager Get_Player_Item_Manager() {
		return (PlayerItemManager) HushPuppy.safeFindComponent("Player", "PlayerItemManager");
	}

	void Start() {
		Reset_Arrows();
		Reset_Traps();
	}

	public void Reset_Arrows() {
		arrowCount = arrowMaxCount;
	}

	public void Reset_Traps() {
		trapCount = trapMaxCount;
	}
}
