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
    MUSIC,
    SFX,
    SFX_AMBIENT,
    SFX_POWERS,
    SFX_PUZZLES,
    SFX_COLLECTABLES,
    SFX_STEPS,
}


public class AudioManager : MonoBehaviour {
    public AudioClip[] audioClips;
    [SerializeField]
    private AudioListener _audioListener;
    [SerializeField]
    private AudioMixer _audioMixer;
    private Transform _soundsContainer;
    private Dictionary<string, AudioSource> _audioSources = new Dictionary<string, AudioSource>();
    private Dictionary<AudioGroup, Tuple<bool, Func<AudioMixerGroup>>> _audioGroups =
        new Dictionary<AudioGroup, Tuple<bool, Func<AudioMixerGroup>>>();
    public static AudioManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(instance.gameObject);
            instance = this;
        }
        _audioGroups[AudioGroup.MUSIC] = Tuple.Create<bool, Func<AudioMixerGroup>>
            (true, () => _audioMixer.FindMatchingGroups("Master/Music")[0]);
        _audioGroups[AudioGroup.SFX] = Tuple.Create<bool, Func<AudioMixerGroup>>
            (true, () => _audioMixer.FindMatchingGroups("Master/SFX")[0]);
        _audioGroups[AudioGroup.SFX_AMBIENT] = Tuple.Create<bool, Func<AudioMixerGroup>>
            (true, () => _audioMixer.FindMatchingGroups("Master/SFX/Ambient")[0]);
        _audioGroups[AudioGroup.SFX_POWERS] = Tuple.Create<bool, Func<AudioMixerGroup>> 
            (true, () => _audioMixer.FindMatchingGroups("Master/SFX/Powers")[0]);
        _audioGroups[AudioGroup.SFX_PUZZLES] = Tuple.Create<bool, Func<AudioMixerGroup>> 
            (true, () => _audioMixer.FindMatchingGroups("Master/SFX/Puzzles")[0]);
        _audioGroups[AudioGroup.SFX_COLLECTABLES] = Tuple.Create<bool, Func<AudioMixerGroup>>
            (true, () => _audioMixer.FindMatchingGroups("Master/SFX/Collectables")[0]);
        _audioGroups[AudioGroup.SFX_STEPS] = Tuple.Create<bool, Func<AudioMixerGroup>>
            (true, () => _audioMixer.FindMatchingGroups("Master/SFX/Steps")[0]);
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
            var go = new GameObject();
            go.name = clip.name;
            go.transform.parent = _soundsContainer;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            var audioSource = go.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 0f;
            _audioSources.Add(clip.name, audioSource);
        }

        PlayAudio("background-02", AudioMode.Loop, AudioGroup.MUSIC);
    }
    
    public void SetMuteGroup(AudioGroup group, bool status) {
        var groupMixer = _audioGroups[group].Item2();
        if (groupMixer != null) {
            _audioMixer.SetFloat(groupMixer.name, !status ? -80f : 0f);
        }
        _audioGroups[group] = Tuple.Create(!status, _audioGroups[group].Item2);
    }
    
    public AudioClip GetClip(string key) {
        foreach (var clip in audioClips) {
            if (clip.name == key) {
                return clip;
            }
        }
        throw new Exception("Clip not found: (" + key + ")");
    }

    public void PlayAudio(string key, AudioMode mode, AudioGroup group, float volume = 1f) {
        var clip = GetClip(key);
        if (clip == null) return;
        var isEnable = _audioGroups[group].Item1;
        if (!isEnable) return;
#if UNITY_EDITOR
        Debug.Log(this, "Playing audio: " + key);
#endif

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
        source.volume = volume;
        var groupMixer = _audioGroups[group].Item2();
        if (groupMixer != null) {
#if UNITY_EDITOR
            Debug.Log("MixerGroup", "clip: " + clip.name + " group: " + groupMixer.name + "key: " + group);
#endif
            source.outputAudioMixerGroup = groupMixer;
        }else {
            throw new Exception("Group not found: (" + group + ")");
        }

        source.Play();
    }
    
    public void PlayAudio(AudioClip clip, AudioMode mode, AudioGroup group, float volume = 1f) {
        AudioSource source;
        var isEnable = _audioGroups[group].Item1;
        if (!isEnable) return;
        
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
        var groupMixer = _audioGroups[group].Item2();
        if (groupMixer != null) {
#if UNITY_EDITOR
            Debug.Log("MixerGroup", "clip: " + clip.name + " group: " + groupMixer.name + "key: " + group);
#endif
            source.outputAudioMixerGroup = groupMixer;
        }else {
            throw new Exception("Group not found: (" + group + ")");
        }
        source.volume = volume;
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
        if (_audioSources[key] == null) {
            Debug.LogWarning("AudioSource is null: (" + key + ")");
            return;
        }
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
        if (_audioSources[key] == null) {
            Debug.LogWarning("AudioSource is null: (" + key + ")");
            return;
        }
        _audioSources[key].volume = 0;
        _audioSources[key].Stop();
    }

    public void StopAudio(AudioClip clip) {
        this.StopAudio(clip.name);
    }
}