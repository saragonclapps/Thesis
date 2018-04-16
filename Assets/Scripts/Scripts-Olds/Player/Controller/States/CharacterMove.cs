using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterMove : MonoBehaviour {

    #region Global Variables
    public float speed;
    public float stealthSpeed;
    private float _speed;
    public CameraFMS cam;

    //Turn Variables
    private Vector3 _oldDirection;
    private Vector3 _newDirection;
    private Vector3 _desiredForward;
    private bool _turn;
    public float angleTurnTolerance;

    [Range(0.1f,0.9f)]
    public float idleTurnSpeed;

    [Range(0.1f, 0.9f)]
    public float runingTurnSpeed;

    //Move Variables
    private float _horizontal;
    private float _vertical;
    private float _movementSpeed;
    private float _angleCorrection;

    private bool forwardCollision;


    //Player Controller
    private PlayerController _pc;

    private AnimatorEventsBehaviour _aES;

    private LeftHandIKControl _lHIK;

    private CapsuleCollider _cC;
    #endregion

    void Start()
    {
        _pc = GetComponent<PlayerController>();
        _oldDirection.x = transform.forward.x;
        _oldDirection.z = transform.forward.z;
        _oldDirection = _oldDirection.normalized;
        _aES = GetComponentInChildren<AnimatorEventsBehaviour>();
        _lHIK = GetComponentInChildren<LeftHandIKControl>();
        _cC = GetComponent<CapsuleCollider>();
    }

    /// <summary>
    /// Enter Move Function
    /// </summary>
    /// <param name="isStealth">if is stealthing or not</param>
    public void Enter(bool isStealth)
    {
        _speed = isStealth ? stealthSpeed : speed;
        GetCorrectedForward();
        //In case you have to pivot from idle
        if (_oldDirection != _newDirection)
        {
            var angle = Vector3.Angle(_oldDirection, _newDirection);
            _turn = Math.Abs(angle) > angleTurnTolerance;
        }
        //Set Animator Transition
        _pc.anim.SetFloat("speed", 1);
    }

    /// <summary>
    /// Execute Move Function
    /// </summary>
    public void Execute()
    {
        if (_turn)
        {
            //cam.GetComponent<CameraController>().positionSmoothness = 0.1f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_newDirection), idleTurnSpeed);
            var angle = Vector3.Angle(transform.forward, _newDirection);
            if (Math.Abs(angle) < 5)
            {
                _turn = false;
                transform.forward = _newDirection;
            }
        }
        else
        {
            //cam.GetComponent<CameraController>().positionSmoothness = 0.5f;
            GetCorrectedForward();
            //Rotate to the new forward
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_newDirection), runingTurnSpeed);

            //MoveForward
            if(_aES.landEnd && !_pc.CheckForwardCollision(_newDirection, true))
            {
                if (GameInput.instance.sprintButton)
                {
                    _pc.anim.SetBool("sprint", true);
                    _movementSpeed = 1.2f * _speed;
                    SetColliderDimensions(ColliderSettings.SPRINT);
                    //_pc.collisionDistance = 1;
                }
                else
                {
                    _pc.anim.SetBool("sprint", false);
                    _movementSpeed =  _speed / 1.2f;
                    SetColliderDimensions(ColliderSettings.NORMAL);
                    //_pc.collisionDistance = 0.7f;
                }
                transform.position += transform.forward * Time.deltaTime * _movementSpeed;
            }
            _pc.anim.SetBool("isAbsorbing", _lHIK.ikActive);
        }
        
    
    }

    /// <summary>
    /// Exit Move Function
    /// </summary>
    public void Exit()
    {
        _oldDirection.x = transform.forward.x;
        _oldDirection.z = transform.forward.z;
        _oldDirection = _oldDirection.normalized;
        _pc.anim.SetFloat("speed", 0);
        _pc.anim.SetBool("isAbsorbing", false);
        _pc.anim.SetBool("sprint", false);
        SetColliderDimensions(ColliderSettings.NORMAL);
        //cam.GetComponent<CameraController>().positionSmoothness = 0.1f;
        //_pc.collisionDistance = 0.7f;
    }

    /// <summary>
    /// Get the new Forward refered to the camera
    /// </summary>
    void GetCorrectedForward()
    {
        //Get inputs
        _horizontal = GameInput.instance.horizontalMove;
        _vertical = GameInput.instance.verticalMove;
        _movementSpeed = new Vector2(_horizontal, _vertical).normalized.magnitude * _speed;

        //Get cameraForward(2D floor plain)
        var camForwardWithOutY = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
        var anglesign = Vector3.Cross(new Vector3(0, 0, 1), camForwardWithOutY).y > 0 ? 1 : -1;
        _angleCorrection = Vector3.Angle(new Vector3(0, 0, 1), camForwardWithOutY) * anglesign;

        //Get forward multiplying the input vector3 with the quaternion containing the camera angle
        _newDirection = (Quaternion.Euler(0f, _angleCorrection, 0f) * new Vector3(_horizontal, 0, _vertical)).normalized;
    }

    public void SetColliderDimensions(ColliderSettings cS)
    {
        switch (cS)
        {
            case ColliderSettings.NORMAL:
                _cC.direction = 1;
                _cC.center = new Vector3(0, 0.75f, 0);
                _cC.height = 1.5f;
                _cC.radius = 0.33f;
                break;
            case ColliderSettings.STEALTH:
                _cC.direction = 1;
                _cC.center = new Vector3(0, 0.55f, 0.1f);
                _cC.height = 1.1f;
                _cC.radius = 0.33f;
                break;
            case ColliderSettings.SPRINT:
                _cC.direction = 2;
                _cC.center = new Vector3(0, 0.4f, 0.15f);
                _cC.height = 1.3f;
                _cC.radius = 0.4f;
                break;

        }
    }
    
    public enum ColliderSettings
    {
        NORMAL,
        STEALTH,
        SPRINT
    }
}
