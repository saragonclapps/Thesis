using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Platform))]
public class SwitchPlatform : MonoBehaviour {

    public VacuumSwitch vacuumSwitch;

    Platform platform;
	// Use this for initialization
	void Start ()
    {
        platform = GetComponent<Platform>();
        vacuumSwitch.AddOnSwitchEvent(SwitchOn);
        platform.isActive = false;
	}
	
	// Update is called once per frame
	void Execute ()
    {
		
	}

    void SwitchOn()
    {
        platform.isActive = true;   
    }

    void OnDestroy()
    {
        vacuumSwitch.RemoveOnSwitchEvent(SwitchOn);
    }
}
