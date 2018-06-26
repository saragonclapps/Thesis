using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPCamera;

public class ForwardChecker : MonoBehaviour {

    float _horizontal;
    float _vertical;
    float _angleCorrection;

    Vector3 _newDirection;
    //Quaternion _newDirection;

    public CameraFSM _cam;
    public float offset;

    public bool isForwardObstructed;


	// Use this for initialization
	void Start () {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	// Update is called once per frame
	void Execute () {
        if (CheckMove())
        {
            GetCorrectedForward();
            transform.forward = _newDirection;
            //transform.position = _newDirection * offset;
        }
	}

    void GetCorrectedForward()
    {
        //Get inputs
        _horizontal = GameInput.instance.horizontalMove;
        _vertical = GameInput.instance.verticalMove;

        //Get cameraForward(2D floor plain)
        var camForwardWithOutY = new Vector3(_cam.transform.forward.x, 0, _cam.transform.forward.z);
        var anglesign = Vector3.Cross(new Vector3(0, 0, 1), camForwardWithOutY).y > 0 ? 1 : -1;
        _angleCorrection = Vector3.Angle(new Vector3(0, 0, 1), camForwardWithOutY) * anglesign;

        //Get forward multiplying the input vector3 with the quaternion containing the camera angle
        _newDirection = (Quaternion.Euler(0f, _angleCorrection, 0f) * new Vector3(_horizontal, 0, _vertical)).normalized;
        //_newDirection = Quaternion.Euler(0f, _angleCorrection, 0f);
    }

    bool CheckMove()
    {
        return Mathf.Abs(GameInput.instance.horizontalMove) > 0.1f || Mathf.Abs(GameInput.instance.verticalMove) > 0.1f;
    }

    void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.layer != 9)
        {
            isForwardObstructed = true;
        }
    }

    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.layer != 9)
        {
            isForwardObstructed = true;
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.layer != 9)
        {
            isForwardObstructed = false;
        }
    }
}
