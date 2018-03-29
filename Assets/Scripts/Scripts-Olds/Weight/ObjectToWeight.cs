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
        if (collision.collider.gameObject.layer == 8)
        {
            control = collision.collider.GetComponent<ObjectToWeight>().control;
        }
        else if (collision.collider.gameObject.layer == 14)
        {
            control = collision.collider.GetComponent<Weight>();
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
