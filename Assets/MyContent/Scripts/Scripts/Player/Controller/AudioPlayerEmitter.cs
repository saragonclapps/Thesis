using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = Logger.Debug;

public class AudioPlayerEmitter : AudioBaseEmiter {
    // an array of sounds that will be randomly selected from.
    [SerializeField] private AudioClip[] _footsFepSounds;
    [SerializeField] private AudioClip[] _jumpAndOnLandSounds;
    [SerializeField] private AudioClip[] _vacuumSounds;
    [SerializeField] private AudioClip[] _fireSounds;
    [SerializeField] private AudioClip[] _waterSounds;
    
    [SerializeField] private LandChecker _lC;

    public string audioOnPower { get; set; }

    AudioSource _stepsSources;
    public static AudioPlayerEmitter instance;

    private void Awake() {
        //singleton
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    public void PlayFootStepAudio(float volume) {
        if (AudioManager.instance == null || _mute) {
            return;
        }

        if (!_lC.land) {
            return;
        }

        var toPlay  = RandomSoundFromArray(_footsFepSounds);
        AudioManager.instance.PlayAudio(toPlay, AudioMode.OneShot, AudioGroup.SFX_STEPS, volume);
    }
    
    public void PlayJumpAndOnLandAudio(float volume) {
        if (AudioManager.instance == null || _mute) {
            return;
        }

        if (!_lC.land) {
            return;
        }
        
        var toPlay  = RandomSoundFromArray(_jumpAndOnLandSounds);
        AudioManager.instance.PlayAudio(toPlay, AudioMode.OneShot, AudioGroup.SFX_STEPS, volume);
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
        if (AudioManager.instance == null || _mute) {
            return;
        }

        switch (_vacuumSounds.Length) {
            case 0: {
                throw new Exception("No vacuum sounds");
            }
            case 1: {
                var sound = _vacuumSounds[0];
                audioOnPower = sound.name;
                AudioManager.instance.PlayAudio(sound, AudioMode.Loop, AudioGroup.SFX_POWERS);
                return;
            }
            default: {
                var toPlay  = RandomSoundFromArray(_vacuumSounds);
                AudioManager.instance.PlayAudio(toPlay, AudioMode.Loop, AudioGroup.SFX_POWERS);
                break;
            }
        }
    }

    public void PlayFireAudio() {
#if UNITY_EDITOR
        Debug.Log(this, "Playing POWER fire audio");
#endif
        if (AudioManager.instance == null || _mute) {
            return;
        }

        switch (_fireSounds.Length) {
            case 0: {
                throw new Exception("No fire sounds");
            }
            case 1: {
                var sound = _fireSounds[0];
                audioOnPower = sound.name;
                AudioManager.instance.PlayAudio(sound, AudioMode.Loop, AudioGroup.SFX_POWERS);
                return;
            }
            default: {
                var toPlay  = RandomSoundFromArray(_fireSounds);
                AudioManager.instance.PlayAudio(toPlay, AudioMode.Loop, AudioGroup.SFX_POWERS);
                break;
            }
        }
    }

    public void PlayWaterAudio() {
#if UNITY_EDITOR
        Debug.Log(this, "Playing POWER water audio");
#endif
        if (AudioManager.instance == null || _mute) {
            return;
        }
        
        switch (_waterSounds.Length) {
            case 0: {
                throw new Exception("No water sounds");
            }
            case 1: {
                var sound = _waterSounds[0];
                audioOnPower = sound.name;
                AudioManager.instance.PlayAudio(sound, AudioMode.Loop, AudioGroup.SFX_POWERS);
                return;
            }
            default: {
                var toPlay  = RandomSoundFromArray(_waterSounds);
                AudioManager.instance.PlayAudio(toPlay, AudioMode.OneShot, AudioGroup.SFX_POWERS);
                break;
            }
        }
    }

    public override void SetMute(bool mute) {
#if UNITY_EDITOR
        Debug.LogColor(this, "Mute: " + mute, "red");
#endif
        this._mute = mute;
    }
}