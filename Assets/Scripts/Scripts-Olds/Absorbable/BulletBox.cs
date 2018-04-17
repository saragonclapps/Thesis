using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBox : IVacuumObjects {

    public float _timmer;
    float _tick;

    private void Start()
    {
        _tick = 0;
    }


    public override void Exit()
    {
        _tick = 0;
    }



    public override void SuckIn(Transform origin, float atractForce)
    {
        _tick += Time.deltaTime;
        if(_tick > _timmer)
        {
            BulletManager.instance.AddItemToBag(Items.SCRAP);
            _tick = 0;
        }
    }

    public override void ViewFX(bool active)
    {
        throw new NotImplementedException();
    }

    public override void BlowUp(Transform origin, float atractForce, Vector3 direction){}
    public override void ReachedVacuum(){}
    public override void Shoot(float shootForce, Vector3 direction){}
}
