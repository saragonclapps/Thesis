using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumConeCollider : MonoBehaviour {

    VacuumController vacuum;
	// Use this for initialization
	void Start () {
        vacuum = GetComponentInParent<VacuumController>();
	}

    #region Trigger Methods
    private void OnTriggerEnter(Collider c)
    {
        VacuumInteractive obj;
        if ((obj = c.GetComponent<VacuumInteractive>()))
            if (!vacuum.objectsToInteract.Contains(obj))
                vacuum.objectsToInteract.Add(obj);
    }

    private void OnTriggerExit(Collider c)
    {
        VacuumInteractive obj;
        if (obj = c.GetComponent<VacuumInteractive>())
        {
            obj.rb.isKinematic = false;
            obj.isAbsorved = false;
            obj.Exit();
            vacuum.objectsToInteract.Remove(obj);
        }
    }

    private void OnTriggerStay(Collider c)
    {
        var aux = GetComponent<VacuumInteractive>();
        if (aux != null)
            if (!vacuum.objectsToInteract.Contains(aux) && (GameInput.instance.absorbButton || GameInput.instance.blowUpButton)) vacuum.objectsToInteract.Add(aux);
    }
    #endregion
}
