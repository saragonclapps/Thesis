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
        IVacuumObject obj;
        obj = c.GetComponent<IVacuumObject>();
        if (obj != null)
            if (!vacuum.objectsToInteract.Contains(obj))
                vacuum.objectsToInteract.Add(obj);
    }

    private void OnTriggerExit(Collider c)
    {
        IVacuumObject obj;
        obj = c.GetComponent<IVacuumObject>();
        if(obj != null)
        {
            obj.rb.isKinematic = false;
            obj.isAbsorved = false;
            obj.Exit();
            vacuum.objectsToInteract.Remove(obj);
        }
    }

    private void OnTriggerStay(Collider c)
    {
        var aux = GetComponent<IVacuumObject>();
        if (aux != null)
            if (!vacuum.objectsToInteract.Contains(aux) && (GameInput.instance.absorbButton || GameInput.instance.blowUpButton)) vacuum.objectsToInteract.Add(aux);
    }
    #endregion
}
