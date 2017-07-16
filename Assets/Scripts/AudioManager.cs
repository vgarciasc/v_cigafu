using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	AudioSource audioPlayer;
	static bool is_playing = false;
	public AudioClip song;

	public static AudioManager getAudioManager() {
		return (AudioManager) HushPuppy.safeFindComponent("AudioManager", "AudioManager");
	}

	void Awake() {
		if (is_playing) {
			Destroy(this.gameObject);
		}
		
		audioPlayer = this.GetComponent<AudioSource>();
		DontDestroyOnLoad(this.gameObject);

		is_playing = true;
		audioPlayer.clip = song;
		audioPlayer.loop = true;
		audioPlayer.Play();
	}
}
