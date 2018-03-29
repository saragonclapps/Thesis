using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShoot : IVacuumAction
{
    private ArmRotator _arm;
    private PathCalculate _pc;
    private Transform _vacuumTransform;
    private VacuumController _vC;

    private Items currentItem;

    public BulletShoot(ArmRotator arm, PathCalculate pc, Transform vacuumTransform ,VacuumController vC)
    {
        _arm = arm;
        _pc = pc;
        _vacuumTransform = vacuumTransform;
        _vC = vC;
    }


    public void Execute(params object[] pC)
    {
        if (_vC.playerController.isAiming)
        {
            _pc.SimulateStraightAim(_vacuumTransform);
            if (GameInput.instance.shootButton)
            {
                BulletManager.instance.RemoveItemFromBag(currentItem);
                if (!BulletManager.instance.CheckItemFromBag(currentItem))
                {
                    _vC.OutOfAmmo();
                }
            }
        }
        else
        {
            if (GameInput.instance.aimToShootButton)
            {
                _arm.activateIK = true;
                _pc.SimulateStraightAim(_vacuumTransform);
                if (GameInput.instance.shootButton)
                {
                    BulletManager.instance.RemoveItemFromBag(currentItem);
                    if (!BulletManager.instance.CheckItemFromBag(currentItem))
                    {
                        _vC.OutOfAmmo();
                    }
                }
            }
            else
            {
                _arm.activateIK = false;
                _pc.DeactivatePath();
            }
        }

    }

    public void Exit()
    {
        _pc.DeactivatePath();
        _arm.activateIK = false;
    }

    public void Initialize()
    {
        currentItem = _vC.currentItem;
    }
}
