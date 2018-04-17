using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumSwitch : MonoBehaviour, IVacuumObject
{
    bool isActive;
    Material mat;

    #region VacuumObject Implementation
    bool _isAbsorved;
    bool _isAbsorvable;
    bool _isBeeingAbsorved;
    Rigidbody _rb;

    public bool isAbsorved {
        get { return _isAbsorved; }
        set { _isAbsorved = value; }
    }

    public bool isAbsorvable { get { return _isAbsorvable; } }

    public bool isBeeingAbsorved {
        get {return _isBeeingAbsorved; }
        set { _isBeeingAbsorved = value; }
    }

    public Rigidbody rb {
        get { return _rb; }
        set { _rb = value; }
    }


    public void BlowUp(Transform origin, float atractForce, Vector3 direction)
    {
        if (isActive)
        {
            if (currentAmountOfAir < maxAmountOfAir)
                currentAmountOfAir += 1;
            else
            {
                if(callbacks != null)
                {
                    callbacks();
                    isActive = false;
                }
            }

            mat.color = new Color(currentAmountOfAir / maxAmountOfAir, 0, 0);
        }
    }
    public void SuckIn(Transform origin, float atractForce)
    {
        if (isActive)
        {
            if(currentAmountOfAir > 0)
                currentAmountOfAir -= 1;

            mat.color = new Color(currentAmountOfAir / maxAmountOfAir, 0, 0);
        }
    }

    //Unused Interface Methods
    public void Exit(){}
    public void ReachedVacuum(){}
    public void Shoot(float shootForce, Vector3 direction){}
    public void ViewFX(bool active){}

    #endregion

    #region Delegate Implementation
    public delegate void OnSwitch();

    OnSwitch callbacks;

    public void AddOnSwitchEvent(OnSwitch callback)
    {
        callbacks += callback;
    }

    public void RemoveOnSwitchEvent(OnSwitch callback)
    {
        callbacks -= callback;
    }
    #endregion

    #region VacuumSwitch Implementation

    public float maxAmountOfAir;
    float currentAmountOfAir;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>().material;
        currentAmountOfAir = 0;
        isActive = true;
        _isAbsorvable = false;
        mat.color = new Color(0, 0, 0, 0);
    }


    #endregion

}
