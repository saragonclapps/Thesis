using System.Collections;
using System.Collections.Generic;
using Kino;
using UnityEngine;

[RequireComponent(typeof(DigitalGlitch))]
public class GlitchDistanceManager : MonoBehaviour {
    private SaveDisk _objective;
    private Transform _currentTransform;
    private DigitalGlitch _digitalGlitch;

    [Header("Glitch settings")] [Range(0.1f, 100f)]
    public float distanceOfGlitch;

    private void Start() {
        _objective = FindObjectOfType<SaveDisk>();
        _currentTransform = transform;
        _digitalGlitch = GetComponent<DigitalGlitch>();
        UpdatesManager.instance.AddUpdate(UpdateType.LATE, Execute);
        EventManager.AddEventListener(GameEvent.SAVE_DISK_COLLECTED, OnSaveDiskCollected);
        _digitalGlitch.intensity = 0;
        AudioManager.instance.PlayAudio("Glitch", AudioMode.Loop, AudioGroup.SFX_AMBIENT);
        AudioManager.instance.SetVolume("Glitch", 0);
    }

    private void OnSaveDiskCollected(object[] parameterContainer = null) {
        UpdatesManager.instance.RemoveUpdate(UpdateType.LATE, Execute);
        AudioManager.instance.SetVolume("Glitch", 0);
        AudioManager.instance.StopAudio("Glitch");
        
        if (!_digitalGlitch) return;
        _digitalGlitch.intensity = 0;
        _digitalGlitch.enabled = false;
    }

    private void Execute() {
        if (!_objective || _objective.isDissolving) {
            _digitalGlitch.intensity = 0;
            return;
        }

        var distance = Vector3.Distance(_currentTransform.position, _objective.transform.position);
        if (distance > distanceOfGlitch) return;

        var percentageOfGlitch = Mathf.Clamp(distance, 0.1f, distanceOfGlitch) / distanceOfGlitch;
        
        var powerGlitch = (1f - percentageOfGlitch) * 0.3f;
        _digitalGlitch.intensity = powerGlitch;
        
        var soundVolumeGlitch = (1f - percentageOfGlitch) * 0.5f;
        AudioManager.instance.SetVolume("Glitch", soundVolumeGlitch);
    }

    private void OnDestroy() {
        OnSaveDiskCollected();
    }
}