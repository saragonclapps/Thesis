using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    public VacuumSwitch elevatorSwitch;
    public bool isActive;
    public Transform objective;

	void Start () {
        elevatorSwitch.AddOnSwitchEvent(SwitchOn);
        isActive = false;

    }

    void SwitchOn()
    {
        isActive = true;
        elevatorSwitch.RemoveOnSwitchEvent(SwitchOn);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isActive)
        {
            other.transform.root.position = objective.position;
        }
    }
}
