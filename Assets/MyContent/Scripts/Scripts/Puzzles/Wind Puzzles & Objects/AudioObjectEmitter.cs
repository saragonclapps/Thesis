using System;
using UnityEngine;

public class AudioObjectEmitter: AudioBaseEmiter {
    public AudioClipData audioClipDataHits;

    public void PlaySoundHit(float volume) {
        if (AudioManager.instance == null) {
            return;
        }
        switch (audioClipDataHits.clips.Length) {
            case 0: {
                throw new Exception("No vacuum sounds");
            }
            case 1: {
                var sound = audioClipDataHits.clips[0];
                AudioManager.instance.PlayAudio(sound, AudioMode.OneShot, AudioGroup.SFX_AMBIENT);
                return;
            }
            default: {
                var toPlay  = RandomSoundFromArray(audioClipDataHits.clips);
                AudioManager.instance.PlayAudio(toPlay, AudioMode.OneShot, AudioGroup.SFX_AMBIENT);
                break;
            }
        }
    }
}