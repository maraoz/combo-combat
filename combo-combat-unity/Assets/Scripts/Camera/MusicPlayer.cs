using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
    public AudioClip[] songs;
    public float musicVolume =  1.0f;

    private int index;

    void Awake() {
        if (Network.isServer) {
            enabled = false;
            Destroy(this);
            return;
        }

        if (audio == null) {
            gameObject.AddComponent<AudioSource>();
            audio.volume = musicVolume;
        }
        index = 0;
    }

    void Update() {
        if (!audio.isPlaying) {
            float r = Random.value;
            index = (int) (r * songs.Length);
            audio.clip = songs[index];
            audio.Play();
        }
    }

    public void PauseMusic() {
        audio.Pause();
        enabled = false;
    }
}
