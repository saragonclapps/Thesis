using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSizeObject : IVacuumObjects {

    

    void Awake () {
        isAbsorvable = true;
        isBeeingAbsorved = false;
        rb = GetComponent<Rigidbody>();
    }

    public override void BlowUp(Transform origin, float atractForce, Vector3 direction)
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

    public override void ReachedVacuum()
    {
        EventManager.DispatchEvent(GameEvent.SMALLABSORVABLE_REACHED, this.gameObject, true);
    }

    public override void SuckIn(Transform origin, float atractForce)
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

    public override void Shoot(float shootForce, Vector3 direction){}
    public override void ViewFX(bool active){}
    public override void Exit(){}
}
