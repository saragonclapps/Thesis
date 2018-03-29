using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DardoAbsorver : VacuumInteractive
{
    [SerializeField]
    Material mat;
    public float FXforce;
    Transform vacuum;

    private void Start()
    {
        var renderer = GetComponentInChildren<Renderer>();
        foreach (var item in renderer.materials)
        {
            if (item.name.Contains("Piel")) mat = item;
        }
    }

    public override void ReachedVacuum()
    {
        EventManager.DispatchEvent(GameEvent.SMALLABSORVABLE_REACHED, gameObject, true);
    }

    public override void SuckIn(Transform origin, float atractForce)
    {
        vacuum = origin;
        isBeeingAbsorved = true;
        //ViewFX(true);
        var distance = Math.Abs((origin.position - transform.position).magnitude);
        var dir = (origin.position - transform.position).normalized;
        if(distance < 0.2f)
        {
            transform.position = origin.position;
            isAbsorved = true;
        }else if(distance <= 1f)
        {

            transform.position += dir * atractForce/10 * Time.deltaTime;
        }
        else
        {
            transform.position += dir * atractForce/20 * Time.deltaTime;
        }
        transform.forward = -dir;
    }

    public override void ViewFX(bool active)
    {
        var force = active ? FXforce : 0;
        var dir = (vacuum.position - transform.position).normalized;
        mat.SetVector("_HitPosition", vacuum.position);
        mat.SetVector("_ImpactVector", dir * force);
    }

    public override void Exit()
    {
        isBeeingAbsorved = false;
        //ViewFX(false);
    }
    public override void Shoot(float shootForce, Vector3 direction){}
    public override void BlowUp(Transform origin, float atractForce, Vector3 direction){}
}
