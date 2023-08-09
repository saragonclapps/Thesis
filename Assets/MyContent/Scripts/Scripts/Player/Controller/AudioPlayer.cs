using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = Logger.Debug;

public class AudioPlayer : MonoBehaviour {
    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip[] _footsFepSounds;
    [SerializeField] private AudioClip[] _vacuumSounds;
    [SerializeField] private AudioClip[] _fireSounds;
    [SerializeField] private AudioClip[] _waterSounds;
    [SerializeField] private LandChecker _lC;

    public string audioOnPower { get; set; }

    AudioSource _stepsSources;

    private void PlayFootStepAudio() {
        if (AudioManager.instance == null) {
            return;
        }

        if (!_lC.land) {
            return;
        }

        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, _footsFepSounds.Length);
        var toPlay = _footsFepSounds[n];
        AudioManager.instance.PlayAudio(toPlay, AudioMode.OneShot, AudioGroup.SFX_STEPS);
        // move picked sound to index 0 so it's not picked next time
        _footsFepSounds[n] = _footsFepSounds[0];
        _footsFepSounds[0] = toPlay;
    }

    public void StopPowerAudio() {
#if UNITY_EDITOR
        Debug.Log(this, "Stopping POWER audio");
#endif
        if (AudioManager.instance == null) {
            Debug.LogWarning("AudioManager.instance is null");
            return;
        }

        if (audioOnPower == null) {
            Debug.LogWarning("No audio on power");
            return;
        }

        AudioManager.instance.StopAudio(audioOnPower);
        audioOnPower = null;
    }

    public void PlayVacuumAudio() {
#if UNITY_EDITOR
        Debug.Log(this, "Playing POWER vacuum audio");
#endif
        if (AudioManager.instance == null) {
            return;
        }

        if (_vacuumSounds.Length == 0) {
            throw new Exception("No vacuum sounds");
        }

        if (_vacuumSounds.Length == 1) {
            var sound = _vacuumSounds[0];
            audioOnPower = sound.name;
            AudioManager.instance.PlayAudio(sound, AudioMode.Loop, AudioGroup.SFX_POWERS);
            return;
        }

        int n = Random.Range(1, _vacuumSounds.Length);
        var toPlay = _vacuumSounds[n];
        audioOnPower = toPlay.name;
        AudioManager.instance.PlayAudio(toPlay, AudioMode.Loop, AudioGroup.SFX_POWERS);
        _vacuumSounds[n] = _vacuumSounds[0];
        _vacuumSounds[0] = toPlay;
    }

    public void PlayFireAudio() {
#if UNITY_EDITOR
        Debug.Log(this, "Playing POWER fire audio");
#endif
        if (AudioManager.instance == null) {
            return;
        }

        if (_fireSounds.Length == 0) {
            throw new Exception("No fire sounds");
        }

        if (_fireSounds.Length == 1) {
            var sound = _fireSounds[0];
            audioOnPower = sound.name;
            AudioManager.instance.PlayAudio(sound, AudioMode.Loop, AudioGroup.SFX_POWERS);
            return;
        }

        int n = Random.Range(1, _fireSounds.Length);
        var toPlay = _fireSounds[n];
        audioOnPower = toPlay.name;
        AudioManager.instance.PlayAudio(toPlay, AudioMode.Loop, AudioGroup.SFX_POWERS);
        _fireSounds[n] = _fireSounds[0];
        _fireSounds[0] = toPlay;
    }

    public void PlayWaterAudio() {
#if UNITY_EDITOR
        Debug.Log(this, "Playing POWER water audio");
#endif
        if (AudioManager.instance == null) {
            return;
        }
        
        if (_waterSounds.Length == 0) {
            throw new Exception("No water sounds");
        }

        if (_waterSounds.Length == 1) {
            var sound = _waterSounds[0];
            audioOnPower = sound.name;
            AudioManager.instance.PlayAudio(sound, AudioMode.Loop, AudioGroup.SFX_POWERS);
            return;
        }

        int n = Random.Range(1, _waterSounds.Length);
        var toPlay = _waterSounds[n];
        audioOnPower = toPlay.name;
        AudioManager.instance.PlayAudio(toPlay, AudioMode.OneShot, AudioGroup.SFX_POWERS);
        _waterSounds[n] = _waterSounds[0];
        _waterSounds[0] = toPlay;
    }
}