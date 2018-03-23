using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour {

    //Movement Variables
    private Vector3 _initialRotation;
    private float _rangeYangle;
    private float _initialYangle;

    [Header("True = ClockWise, False = CounterClockWise") ]
    public bool direction;

    private float _multiplier;
    public float openRotationSpeed;
    public float closeRotationSpeed;
    public float openAngle;
    //Weight Reference
    public Weight weight;

    private bool open;


	void Start () {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);

        _initialRotation = transform.rotation.eulerAngles;

        _multiplier = direction ? -1 : 1;

        _initialYangle = _initialRotation.y == 0 ? 1 : _initialRotation.y;

        _rangeYangle = _initialYangle + (_multiplier * openAngle);

        weight.AddOnWeightEnterEvent(OnWeightEnter);
        weight.AddOnWeightExitEvent(OnWeightExit);

        open = false;
	}
	
	void Execute ()
    {
        if (open)
        {
            if (direction)
            {
                if (transform.rotation.eulerAngles.y >= _rangeYangle)
                {
                    transform.Rotate(new Vector3(0, -openRotationSpeed, 0) * Time.deltaTime);
                }
            }
            else
            {
                if(transform.rotation.eulerAngles.y <= _rangeYangle)
                {
                    transform.Rotate(new Vector3(0, openRotationSpeed, 0) * Time.deltaTime);
                }
            }
        }
        else
        {
            if (direction)
            {
                if (transform.rotation.eulerAngles.y <= _initialYangle - 1)
                {
                    transform.Rotate(new Vector3(0, closeRotationSpeed, 0) * Time.deltaTime);
                }
            }
            else
            {
                if (transform.rotation.eulerAngles.y >= _initialYangle + 1)
                {
                    transform.Rotate(new Vector3(0, -closeRotationSpeed, 0) * Time.deltaTime);
                }
            }
        }   
	}

    void OnWeightEnter()
    {
        open = true;
    }

    void OnWeightExit()
    {
        open = false;
    }
}
