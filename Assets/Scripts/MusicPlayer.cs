using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
	static MusicPlayer instance = null;

    public AudioClip startClip;
    public AudioClip gameClip;
    public AudioClip endClip;

    private AudioSource music;

	void Start () {
		if (instance != null && instance != this) {
			Destroy (gameObject);
			print ("Duplicate music player self-destructing!");
		}

        else
        {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
            music = GetComponent<AudioSource>();
            music.clip = startClip; // initial music when game loads
            music.loop = true;
            music.Play();
		}
		
	}

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("MusicPlayer: loaded level" + level);
        music.Stop(); // stop existing music

        if (level == 0) // 0 - Start Menu
        {
            music.clip = startClip;
        }

        if (level == 1) // Game
        {
            music.clip = gameClip;
        }

        if (level == 2) // Win Screen
        {
            music.clip = endClip;
        }

        music.loop = true; // keeps looping music
        music.Play();
    }
}
