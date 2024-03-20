using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // audio source
    [Header("Audio Source")]
    [SerializeField] public AudioSource SFXSource;

    // audio clips
    [Header("Audio Clips")]
    public AudioClip boom;
    public AudioClip shoot;
    public AudioClip wallTouch;
    public AudioClip movement;
    public AudioClip flowerCollect;
    public AudioClip extra;

    // play sound
    public void PlaySFX(AudioClip clip) {
        SFXSource.PlayOneShot(clip);
    }

    // stop sound
    public void StopSFX() {
        SFXSource.Stop();
    }
}
