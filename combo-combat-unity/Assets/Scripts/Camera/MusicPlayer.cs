using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
    public AudioClip[] songs;

    private int index;

    void Awake() {
        if (Network.isServer) {
            enabled = false;
            Destroy(this);
            return;
        }

        if (audio == null) {
            gameObject.AddComponent<AudioSource>();
            audio.volume = 0.5f;
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
