using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSizeObject : MonoBehaviour, IVacuumObject
{
    bool _isAbsorved;
    bool _isAbsorvable;
    bool _isBeeingAbsorved;
    Rigidbody _rb;

    public bool isAbsorved
    {
        get{ return _isAbsorved; }
        set{ _isAbsorved = value; }
    }

    public bool isAbsorvable { get { return _isAbsorvable; } }

    public bool isBeeingAbsorved { get {return _isBeeingAbsorved; } set { _isBeeingAbsorved = value; } }

    public Rigidbody rb { get { return _rb; }  set {_rb = value; }}

    void Awake () {
        _isAbsorvable = true;
        _isBeeingAbsorved = false;
        _rb = GetComponent<Rigidbody>();
    }

    public void BlowUp(Transform origin, float atractForce, Vector3 direction)
    {
        
            rb.isKinematic = false;
            isAbsorved = false;
            var distanceRaw = transform.position - origin.position;
            var distance = distanceRaw.magnitude;
            var forceMagnitude = rb.mass * atractForce * 10 / Mathf.Pow(distance, 2);
            forceMagnitude = Mathf.Clamp(forceMagnitude, 0, 50);
            var force = direction.normalized * forceMagnitude;
            rb.AddForce(force);
            transform.rotation = Quaternion.LookRotation(direction);
        
    }

    public void ReachedVacuum()
    {
        EventManager.DispatchEvent(GameEvent.SMALLABSORVABLE_REACHED, this.gameObject, true);
    }

    public void SuckIn(Transform origin, float atractForce)
    {
        var direction = origin.position - transform.position;
        var distance = direction.magnitude;
        if (distance <= 0.41f)
        {
            rb.isKinematic = true;
            transform.position = origin.position;
            isAbsorved = true;
            transform.SetParent(origin);
        }
        else if (distance < 0.6f)
        {
            rb.isKinematic = true;
            transform.position = Vector3.Lerp(transform.position, origin.position, 0.5f);
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.isKinematic = false;
            var forceMagnitude = (rb.mass) * atractForce / Mathf.Pow(distance, 2) / 10;
            var force = direction.normalized * forceMagnitude;
            transform.position += force * Time.deltaTime;
            rb.AddForce(force);
        }

    }

    public void Shoot(float shootForce, Vector3 direction){}
    public void ViewFX(bool active){}
    public void Exit(){}
}
