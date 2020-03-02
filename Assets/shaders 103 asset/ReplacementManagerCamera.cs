
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacementManagerCamera : MonoBehaviour
{
    private Camera _camDefault;
    private Camera _camReplacement;

    public bool ActiveFx { get; private set; }
    
    private void Awake()
    {
        _camDefault = GetComponent<Camera>();
        _camReplacement = GetComponentInChildren<Camera>();
    }
    
    private void Start()
    {
        ActiveFx = false;
        _camReplacement.enabled = false;
        _camDefault.enabled = true;
        UpdatesManager.instance.AddUpdate(UpdateType.LATE, Execute);
    }

    private void Execute()
    {
        if (!Input.GetKeyDown(KeyCode.G)) return;
        
        ActiveFx = !ActiveFx;
        if (ActiveFx)
        {
            _camReplacement.enabled = true;
            _camDefault.enabled = false;
            return;
        }
        
        _camReplacement.enabled = false;
        _camDefault.enabled = true;
    }
    
    private void OnDestroy()
    {
        ActiveFx = false;
        _camReplacement.enabled = false;
        _camDefault.enabled = true;
        UpdatesManager.instance.RemoveUpdate(UpdateType.LATE, Execute);
    }
}