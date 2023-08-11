using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumSwitch : MonoBehaviour, IVacuumObject
{
    private bool _isActive;

    #region VacuumObject Implementation

    private bool _isAbsorved;
    private bool _isAbsorvable;
    private bool _isBeeingAbsorved;

    public bool isAbsorved {
        get { return _isAbsorved; }
        set { _isAbsorved = value; }
    }

    public bool isAbsorvable { get { return _isAbsorvable; } }

    public bool isBeeingAbsorved {
        get {return _isBeeingAbsorved; }
        set { _isBeeingAbsorved = value; }
    }

    public Rigidbody rb { get; set; }

    public Collider collider { get; set; }


    public void BlowUp(Transform origin, float atractForce, Vector3 direction)
    {
        if (_isActive)
        {
            if (currentAmountOfAir < maxAmountOfAir)
                currentAmountOfAir += 60*Time.deltaTime;
            else
            {
                currentAmountOfAir = maxAmountOfAir;
                var soundVolume = maxAmountOfAir / currentAmountOfAir;
                _audioSource.volume = soundVolume;
                if(callbacks != null)
                {
                    callbacks();
                    _isActive = false;
                    UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
                }
            }
            if(increaseCallbacks != null)
                increaseCallbacks();
        }
    }

    public void SuckIn(Transform origin, float atractForce) {
        if (!_isActive) return;
        if(currentAmountOfAir > 0) {
            currentAmountOfAir -= 1;
        }
        if(decreaseCallbacks != null) {
            decreaseCallbacks();
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
    public delegate void OnSwitchIncrease();
    public delegate void OnSwitchDecrease();

    OnSwitch callbacks;
    OnSwitchIncrease increaseCallbacks;
    OnSwitchDecrease decreaseCallbacks;


    public void AddOnSwitchEvent(OnSwitch callback)
    {
        callbacks += callback;
    }

    public void RemoveOnSwitchEvent(OnSwitch callback)
    {
        callbacks -= callback;
    }

    public void AddOnSwitchIncreaseEvent(OnSwitchIncrease callback)
    {
        increaseCallbacks += callback;
    }

    public void RemoveOnSwitchIncreaseEvent(OnSwitchIncrease callback)
    {
        increaseCallbacks -= callback;
    }

    public void AddOnSwitchDecreaseEvent(OnSwitchDecrease callback)
    {
        decreaseCallbacks += callback;
    }

    public void RemoveOnSwitchDecreaseEvent(OnSwitchDecrease callback)
    {
        decreaseCallbacks -= callback;
    }


    #endregion

    #region VacuumSwitch Implementation

    public float maxAmountOfAir;
    public float currentAmountOfAir;
    AudioSource _audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        currentAmountOfAir = 0;
        _isActive = true;
        _isAbsorvable = false;
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, RemainKinematic);
    }

    void Execute()
    {
        if(decreaseCallbacks != null)
        {
            decreaseCallbacks();
        }
        if(currentAmountOfAir > 0)
            currentAmountOfAir -= 30* Time.deltaTime;
        
    }

    //TODO: Select if you want kinematic to return to false when you stop blowing up IVacuumObjects
    void RemainKinematic()
    {
        rb.isKinematic = true;
    }

    #endregion

    public float GetCurrentProgressPercent()
    {
        return _isActive ? currentAmountOfAir / maxAmountOfAir: 1;
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, RemainKinematic);
    }

}
