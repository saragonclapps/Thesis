using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToWeight : MonoBehaviour {

    Rigidbody _rb;
    public float mass;
    public Weight control;

    bool wasAdded;

	void Start ()
    {
        _rb = GetComponent<Rigidbody>();
        mass = _rb.mass;
        wasAdded = false;
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }

    private void Execute()
    {
        if(control != null && _rb.IsSleeping() && !wasAdded)
        {
            control.AddToWeight(this);
            wasAdded = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var o = collision.collider.GetComponent<ObjectToWeight>();
        if (o != null)
        {
            control = o.control;
        }
        var w = collision.collider.GetComponent<Weight>();
        if (w != null)
        {
            control = w;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(control != null)
        {
            control.RemoveFromWeight(this);
            control = null;
            wasAdded = false;
        }
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }

}
