
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FencePuzzleAbsorver : VacuumInteractive

{
    public override void BlowUp(Transform origin, float atractForce, Vector3 direction)
    {
        var distance = (transform.position - origin.position).magnitude;
        var forceMagnitude = rb.mass * atractForce * 10 / Mathf.Pow(distance, 2);
        forceMagnitude = Mathf.Clamp(forceMagnitude, 0, 2000);
        var force = direction.normalized * forceMagnitude;
        rb.AddForce(force);
    }

    public override void SuckIn(Transform origin, float atractForce)
    {
        var direction = origin.position - transform.position;
        var distance = direction.magnitude;
        var forceMagnitude = (rb.mass) * atractForce / distance;
        var force = direction.normalized * forceMagnitude;
        rb.AddForce(force);
    }

    public override void Exit(){}
    public override void ReachedVacuum(){}
    public override void Shoot(float shootForce, Vector3 direction){}
    public override void ViewFX(bool active){}

}
