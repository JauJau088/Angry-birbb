using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private AudioManager audio;
    private string[] sound = {"BGM1", "BGM2", "BGM3"};
    private int i, tmp = 99;
    private bool trigger = false;

    private void Start() {
        audio = FindObjectOfType<AudioManager>();

        StartCoroutine("Bgm");
    }

    private void Update() {
        if (trigger = true) {
            StartCoroutine("Bgm");

            trigger = false;
        }
    }
    
    private IEnumerator Bgm() {
        if (audio.IsPlaying(sound[0]) || audio.IsPlaying(sound[1]) || audio.IsPlaying(sound[2])) {
            yield return null;
        } else {
            i = Random.Range(0, 3);

            if (tmp == i) {
                yield return null;
            } else {
                Debug.Log("Play " + sound[i]);
                audio.Play(sound[i]);

                yield return tmp = i;
                // wait till music is about to end
                yield return new WaitForSecondsRealtime(150f);

                yield return trigger = true;
            }
        }
    }
}
