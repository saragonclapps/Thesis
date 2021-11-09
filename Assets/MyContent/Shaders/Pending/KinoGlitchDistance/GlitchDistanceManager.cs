using System.Collections;
using System.Collections.Generic;
using Kino;
using UnityEngine;

[RequireComponent(typeof(DigitalGlitch))]
public class GlitchDistanceManager : MonoBehaviour
{
    private SaveDisk _objective;
    private Transform _currentTransform;
    private DigitalGlitch _digitalGlitch;

    [Header("Glitch settings")] 
    [Range(0.1f, 100f)]
    public float distanceOfGlitch;
    
    private void Start()
    {
        _objective = FindObjectOfType<SaveDisk>();
        _currentTransform = transform;
        _digitalGlitch = GetComponent<DigitalGlitch>();
        UpdatesManager.instance.AddUpdate(UpdateType.LATE, Execute);
        _digitalGlitch.intensity = 0;
    }

    private void Execute()
    {
        if (!_objective || _objective.isDisolving)
        {
            _digitalGlitch.intensity = 0;
            return;
        }
        var distance = Vector3.Distance(_currentTransform.position, _objective.transform.position);
        if (distance > distanceOfGlitch) return;
        
        var powerGlitch =  (1f - (Mathf.Clamp(distance, 0.1f, distanceOfGlitch) / distanceOfGlitch)) * 0.3f;
        _digitalGlitch.intensity = powerGlitch;
    }
    
    private void OnDestroy()
    {
        _digitalGlitch.enabled = false;
        UpdatesManager.instance.RemoveUpdate(UpdateType.LATE, Execute);
    }
}
