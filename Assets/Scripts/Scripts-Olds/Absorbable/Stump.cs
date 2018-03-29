using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stump : VacuumInteractive {

    private Vector3 _maxMove = new Vector3(8.4f, 7.5f, -1.5f);
    private Vector3 _minMove = new Vector3(0.4f, 0.0f, -1.5f);

    public override void BlowUp(Transform origin, float atractForce, Vector3 direction){}

    public override void Exit(){}
    public override void ReachedVacuum(){}
    public override void Shoot(float shootForce, Vector3 direction){}
    public override void SuckIn(Transform origin, float atractForce){}
    public override void ViewFX(bool active){}

    void Start ()
    {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, UpdateMove);
    }

    private void UpdateMove()
    {
        var x = Mathf.Clamp(transform.localPosition.x, _minMove.x, _maxMove.x);
        var y = Mathf.Clamp(transform.localPosition.y, _minMove.y, _maxMove.y);
        var z = Mathf.Clamp(transform.localPosition.z, _minMove.z, _maxMove.z);

        transform.localPosition = new Vector3(x, y, z);
    }
}
