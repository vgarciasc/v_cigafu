using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogManager : MonoBehaviour {
	public List<string> falasIniciais = new List<string>();
	public List<string> falasVitoria = new List<string>();
	public List<string> falasSemTrap = new List<string>();

	public Text intro_text;
	public GameObject dialogCanvas;

	bool dialog_active = false;
	bool pressed_key = false;
	bool text_running = false;

	Coroutine speaking = null;

	void Start() {
		falasIniciais.Add("voce achou que tinha se livrado de mim nao é mesmo");
		falasIniciais.Add("achou errado !!!!!!!!!!");
		falasIniciais.Add("derrote todos os inimigos e quem sabe eu vou ser boazinha com vc... ");

		falasVitoria.Add("parabens!!");
		falasVitoria.Add("seu premio é...");
		falasVitoria.Add("12d5AdaASDs4a6SS");
		falasVitoria.Add("passe esse codigo para o vinicius e vc tera uma grande surpresa...");

		falasSemTrap.Add("ta sem trap rapaz");
		falasSemTrap.Add("acho q vc esta em apuros");
	}

	public void OnTriggerStay2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (Input.GetKeyDown(KeyCode.E)) {
			if (!dialog_active) {
				dialog_active = true;
				dialogCanvas.SetActive(true);
				speaking = StartCoroutine(Mostrar_Falas());
			}
		}
	}

	public void OnTriggerExit2D(Collider2D coll) {
		if (speaking != null) {
			StopCoroutine(speaking);
		}
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.E) && text_running) {
			pressed_key = true;
		}
	}

	IEnumerator Mostrar_Falas() {
		List<string> aux;
		if (ExplorerManager.Get_Explorer_Manager().cleared_rooms.Count == 3) {
			aux = falasVitoria;
		}
		else if (PlayerItemManager.Get_Player_Item_Manager().trapCount == 0) {
			aux = falasSemTrap;
		}
		else {
			aux = falasIniciais;
		}

		foreach(string s in aux) {
			yield return display_text(s);
			yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
		}

		dialogCanvas.SetActive(false);
		dialog_active = false;
	}

	IEnumerator display_text(string text) {
		int current_character;
		for (current_character = 0; current_character < text.Length; current_character++) {
			text_running = true;
			if (pressed_key) {
				pressed_key = false;
				break;
			}
			intro_text.text = text.Substring(0, current_character) + "<color=#0000>" + text.Substring(current_character) + "</color>";
			yield return HushPuppy.WaitForEndOfFrames(2);
		}

		text_running = false;		
		intro_text.text = text;
	}
}
