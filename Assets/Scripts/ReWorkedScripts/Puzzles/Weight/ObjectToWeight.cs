using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToWeight : MonoBehaviour {

    Rigidbody _rb;
    public float mass;
    public Weight control;

    bool wasAdded;

    float _timmer = 2;
    [SerializeField]
    float _tick;

	void Start ()
    {
        _rb = GetComponent<Rigidbody>();
        mass = _rb.mass;
        wasAdded = false;
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }

    private void Execute()
    {
        if(control != null && !wasAdded)
        {
            _tick += Time.deltaTime;
            if(_tick>_timmer)
            {
                control.AddToWeight(this);
                wasAdded = true;
            }
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
        RemoveWeightFromControl();
    }

    public void RemoveWeightFromControl()
    {
        if (control != null)
        {
            control.RemoveFromWeight(this);
            control = null;
            wasAdded = false;
            _tick = 0;
        }
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
        RemoveWeightFromControl();
    }

}
