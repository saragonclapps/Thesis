using System;
using System.Collections.Generic;
using UnityEngine;

public enum AudioMode {
    OneShot,
    Loop
}
public enum AudioGroup {
    SFX,
    Music
}

public class AudioManager : MonoBehaviour {
    public AudioClip[] audioClips;
    [SerializeField]
    private AudioSource _audioSourceSfx;
    [SerializeField]
    private AudioSource _audioSourceMusic;
    public static AudioManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(instance.gameObject);
            instance = this;
        }
    }


    public AudioClip GetClip(string key) {
        foreach (var clip in audioClips) {
            if (clip.name == key) {
                return clip;
            }
        }
        throw new Exception("Clip not found: (" + key + ")");
    }

    public void PlayAudio(string key, AudioMode mode = AudioMode.OneShot, AudioGroup group = AudioGroup.SFX) {
        var clip = GetClip(key);
        if (clip == null) return;
        if (group == AudioGroup.Music) {
            PlayMusic(clip, mode);
        }
        else {
            PlaySfx(clip, mode);
        }
    }
    
    public void PlayAudio(AudioClip clip, AudioMode mode = AudioMode.OneShot, AudioGroup group = AudioGroup.SFX) {
        if (group == AudioGroup.Music) {
            PlayMusic(clip, mode);
        }
        else {
            PlaySfx(clip, mode);
        }
    }
    
    public void PlayMusic(AudioClip clip, AudioMode mode = AudioMode.OneShot) {
        _audioSourceMusic.clip = clip;
        _audioSourceMusic.loop = mode == AudioMode.Loop;
        _audioSourceMusic.Play();
    }
    
    public void PlaySfx(AudioClip clip, AudioMode mode = AudioMode.OneShot) {
        _audioSourceSfx.clip = clip;
        _audioSourceSfx.loop = mode == AudioMode.Loop;
        _audioSourceSfx.Play();
    }
}