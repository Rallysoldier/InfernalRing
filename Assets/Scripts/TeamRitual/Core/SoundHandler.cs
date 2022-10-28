using BlackGardenStudios.HitboxStudioPro;
using UnityEngine;

namespace TeamRitual.Core {
public class SoundHandler {
    public AudioSource audioSource;
    public SoundHandler(AudioSource audioSource) {
        this.audioSource = audioSource;
    }

    public void PlaySound(AudioClip clip, bool stopOthers) {
        if (stopOthers) {
            audioSource.clip = clip;
            audioSource.Play();
        } else {
            audioSource.PlayOneShot(clip);
        }
    }
}
}