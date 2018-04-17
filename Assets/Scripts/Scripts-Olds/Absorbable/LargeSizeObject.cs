using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeSizeObject : IVacuumObjects
{
    private Rigidbody _rb;
    public float movement;

    [Header("Constrains")]
    public bool x;
    public bool z;

    void Start ()
    {
        _rb = GetComponent<Rigidbody>();
        isAbsorvable = false;
	}

    public override void BlowUp(Transform origin, float atractForce, Vector3 direction)
    {
        //Distance from vacuum to object
        RaycastHit ray;
        Physics.Raycast(origin.position, origin.forward, out ray, 2f);
        var distance = ray.distance;

        //Movement
        var dirx = x ? 0 : ray.normal.x;
        var dirz = z ? 0 : ray.normal.z;
        var dir = new Vector3(dirx, 0, dirz);
        movement = atractForce / _rb.mass;
        transform.position -= dir * movement * Time.deltaTime;

    }


    public override void SuckIn(Transform origin, float atractForce)
    {
        //Distance from vacuum to object
        RaycastHit ray;
        Physics.Raycast(origin.position, origin.forward, out ray, 2f);
        var distance = ray.distance;

        //Movement
        if(distance > 0.2f)
        {
            var dirx = x ? 0 : ray.normal.x;
            var dirz = z ? 0 : ray.normal.z;
            var dir = new Vector3(dirx, 0, dirz);
            movement = atractForce / _rb.mass;
            transform.position += dir * movement * Time.deltaTime;
        }
        else
        {
            movement = 0;
        }
    }

    public override void Exit()
    {
        isBeeingAbsorved = false;
    }

    public override void ReachedVacuum(){}
    public override void Shoot(float shootForce, Vector3 direction){}
    public override void ViewFX(bool active){}

}
