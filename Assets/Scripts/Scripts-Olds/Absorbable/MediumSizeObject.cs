using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Material))]
public class MediumSizeObject : VacuumInteractive {

    public bool wasShooted;
    public Material material;//Edit for shoot vfx.
    private BoxCollider _bC;

    private void Awake()
    {
        isAbsorvable = false;
        rb = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
        _bC = GetComponent<BoxCollider>();
    }

    
    public override void SuckIn(Transform origin, float atractForce)
    {
        if (!wasShooted)
        {
            var direction = origin.position - transform.position;
            var distance = direction.magnitude;

            if (distance <= 0.5f)
            {
                _bC.enabled = false;
                rb.isKinematic = true;
                transform.position = origin.position;
                isAbsorved = true;
                transform.SetParent(origin);
                
            }
            else if (distance < 1f)
            {
                rb.isKinematic = true;
                /*transform.position = Vector3.Lerp(transform.position, origin.position, 0.5f);
                rb.velocity = Vector3.zero;*/
                var dir = (origin.position - transform.position).normalized;
                transform.position += dir * atractForce/rb.mass * Time.deltaTime;
            }
            else
            {
                rb.isKinematic = false;
                var forceMagnitude = (rb.mass) * atractForce / Mathf.Pow(distance, 2);
                var force = direction.normalized * forceMagnitude;
                rb.AddForce(force);
                /*rb.isKinematic = true;
                float force = atractForce / distance;
                var dir = (origin.position - transform.position).normalized;
                transform.position += dir * force * Time.deltaTime;*/
            }
        }
    }

    public override void BlowUp(Transform origin, float atractForce, Vector3 direction)
    {
        if(!wasShooted)
        {
            rb.isKinematic = false;
            isAbsorved = false;
            transform.SetParent(null);
            var distanceRaw = transform.position - origin.position;
            var distance = distanceRaw.magnitude;
            var forceMagnitude =  rb.mass * atractForce * 10 / Mathf.Pow(distance, 2);
            forceMagnitude = Mathf.Clamp(forceMagnitude, 0, 2000);
            var force = direction.normalized * forceMagnitude;
            rb.AddForce(force);
        }
    }

    public override void ReachedVacuum()
    {
        
    }

    public override void Shoot(float shootForce, Vector3 direction)
    {
        _bC.enabled = true;
        wasShooted = true;
        isAbsorved = false;
        rb.isKinematic = false;
        transform.SetParent(null);
        rb.velocity = direction * shootForce/rb.mass;
    }

    private void OnCollisionEnter(Collision collision)
    {
        wasShooted = false;
    }

    public override void ViewFX(bool activate)
    {
        if (activate)
        {
            //View VFX
            
            material.SetFloat("_Alpha", 0.3f);
        }
        else
        {
            //Reset view VFX
            material.SetFloat("_Alpha", 1f);
        }
    }

    public override void Exit()
    {
        _bC.enabled = true;
        ViewFX(false);
        transform.SetParent(null);
        rb.isKinematic = false;
        isAbsorved = false;
    }
}
