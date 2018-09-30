
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FencePuzzleAbsorver : MonoBehaviour, IVacuumObject

{
    bool _isAbsorved;
    bool _isAbsorvable;
    bool _isBeeingAbsorved;
    Rigidbody _rb;

    public bool isAbsorved { get { return _isAbsorved; } set { _isAbsorved = value; } }
    public bool isAbsorvable { get { return _isAbsorvable; } }
    public bool isBeeingAbsorved { get { return _isBeeingAbsorved; } set { _isBeeingAbsorved = value; } }
    public Rigidbody rb { get { return _rb; } set { _rb = value; } }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();   
    }

    public void BlowUp(Transform origin, float atractForce, Vector3 direction)
    {
        var distance = (transform.position - origin.position).magnitude;
        var forceMagnitude = rb.mass * atractForce * 10 / Mathf.Pow(distance, 2);
        forceMagnitude = Mathf.Clamp(forceMagnitude, 0, 2000);
        var force = direction.normalized * forceMagnitude;
        rb.AddForce(force);
    }

    public  void SuckIn(Transform origin, float atractForce)
    {
        var direction = origin.position - transform.position;
        var distance = direction.magnitude;
        var forceMagnitude = (rb.mass) * atractForce / distance;
        var force = direction.normalized * forceMagnitude;
        rb.AddForce(force);
    }

    public  void Exit(){}
    public  void ReachedVacuum(){}
    public  void Shoot(float shootForce, Vector3 direction){}
    public  void ViewFX(bool active){}

}
