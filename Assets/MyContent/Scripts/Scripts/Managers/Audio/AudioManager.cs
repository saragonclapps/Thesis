using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Debug = Logger.Debug;

public enum AudioMode {
    OneShot,
    Loop
}
public enum AudioGroup {
    Music = 0,
    SFX = 1,
}

public class AudioManager : MonoBehaviour {
    public AudioClip[] audioClips;
    [SerializeField]
    private AudioListener _audioListener;
    [SerializeField]
    private AudioMixerGroup _audioMixerMasterGroup;
    [SerializeField]
    private AudioMixerGroup _audioMixerSfxMixerGroup;
    [SerializeField]
    private AudioMixerGroup _audioMixerMusicMixerGroup;
    private Transform _soundsContainer;
    private Dictionary<string, AudioSource> _audioSources = new Dictionary<string, AudioSource>();
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

    private void Start() {
        _soundsContainer = new GameObject("Sounds").transform;
        _soundsContainer.transform.parent = _audioListener.transform;
        _soundsContainer.transform.localPosition = Vector3.zero;
        _soundsContainer.transform.localRotation = Quaternion.identity;
        
        foreach (var clip in audioClips) {
#if UNITY_EDITOR
            Debug.Log(this, "Adding clip: " + clip.name);
#endif
            if (_audioSources.ContainsKey(clip.name)) continue;
            _audioSources.Add(clip.name, gameObject.AddComponent<AudioSource>());
        }

        PlayAudio("background-02", AudioMode.Loop, AudioGroup.Music);
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
        Debug.Log("Playing audio: " + key);
        AudioSource source;
        if (!_audioSources.ContainsKey(key)) {
            var newGameObject = new GameObject();
            newGameObject.name = key;
            newGameObject.transform.parent = _soundsContainer;
            newGameObject.transform.localPosition = Vector3.zero;
            newGameObject.transform.localRotation = Quaternion.identity;
            source = newGameObject.AddComponent<AudioSource>();
            _audioSources.Add(key, source);
        }
        else {
            source = _audioSources[key];
        }
        source.clip = clip;
        source.loop = mode == AudioMode.Loop;
        source.outputAudioMixerGroup = group == AudioGroup.SFX ? _audioMixerSfxMixerGroup : _audioMixerMusicMixerGroup;
        source.Play();
    }
    
    public void PlayAudio(AudioClip clip, AudioMode mode = AudioMode.OneShot, AudioGroup group = AudioGroup.SFX) {
        AudioSource source;
        if (!_audioSources.ContainsKey(clip.name)) {
            var newGameObject = new GameObject();
            newGameObject.name = clip.name;
            newGameObject.transform.parent = _soundsContainer;
            newGameObject.transform.localPosition = Vector3.zero;
            newGameObject.transform.localRotation = Quaternion.identity;
            source = newGameObject.AddComponent<AudioSource>();
            _audioSources.Add(clip.name, source);
#if UNITY_EDITOR
            Debug.Log(this, "Adding clip: " + clip.name);
#endif
        }
        else {
            source = _audioSources[clip.name];
        }
        source.clip = clip;
        source.loop = mode == AudioMode.Loop;
        source.outputAudioMixerGroup = group == AudioGroup.SFX ? _audioMixerSfxMixerGroup : _audioMixerMusicMixerGroup;
        source.Play();
    }
    
    public void SetPitch(string key, float pitch) {
        if (!_audioSources.ContainsKey(key)) {
            throw new Exception("Clip not found: (" + key + ")");
        }
        _audioSources[key].pitch = pitch;
    }
    
    public void SetPitch(AudioClip clip, float pitch) {
        this.SetPitch(clip.name, pitch);
    }
    
    public void SetVolume(string key, float volume) {
        if (!_audioSources.ContainsKey(key)) {
            throw new Exception("Clip not found: (" + key + ")");
        }
#if UNITY_EDITOR
        Debug.Log(this, "Volume clip: " + key + " volume: " + volume);
#endif
        _audioSources[key].volume = volume;
    }
    
    public void SetVolume(AudioClip clip, float volume) {
        this.SetVolume(clip.name, volume);
    }
    
    public void StopAudio(string key) {
        if (!_audioSources.ContainsKey(key)) {
            throw new Exception("Clip not found: (" + key + ")");
        }
#if UNITY_EDITOR
        Debug.Log(this, "Stop clip: " + key);
#endif
        _audioSources[key].volume = 0;
        _audioSources[key].Stop();
    }

    public void StopAudio(AudioClip clip) {
        this.StopAudio(clip.name);
    }
}