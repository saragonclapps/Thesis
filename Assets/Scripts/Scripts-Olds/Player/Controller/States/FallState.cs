using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Jumper))]
public class FallState : MonoBehaviour
{

    private Rigidbody _rb;
    private Jumper _jump;
    private LandChecker _lc;


    //Fall multiplier Variables
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private float _horizontal;
    private float _vertical;

    //Camera Reference
    private CameraFMS _cam;

    //Player controller Reference
    private PlayerController _pc;

    private AnimatorEventsBehaviour _aES;

    [HideInInspector]
    public bool land;

    private int landCount = 0;
    private float ypos;

    private bool isJumpingForward;

    public void Enter()
    {
        isJumpingForward = _jump.forwardJump;
        land = false;
        _aES.landEnd = false;
        landCount = 0;
        ypos = transform.position.y;
    }

    public void Execute()
    {
        _rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

        var ydif = Mathf.Abs(ypos - transform.position.y);
        var ydifComp = _rb.velocity.y * Time.deltaTime;
        if (ydif <= ydifComp) landCount++;
        else landCount = 0;

        ypos = transform.position.y;

        /*if (Mathf.Abs(_rb.velocity.y) <= 0.2) landCount++;
        else landCount = 0;*/

        if (landCount >= 2)
        {
            _pc.land = true;
            landCount = 0;
            Debug.Log("aterrizo por contador");
        }
        else
        {
            _pc.land = _lc.land;
        }
        if (!_pc.CheckForwardCollision(GetCorrectedForward(), true))
        {
            if (isJumpingForward)
            {
                transform.position += transform.forward * _jump.jumpSpeed * Time.deltaTime;
            }

            if (Mathf.Abs(GameInput.instance.horizontalMove) > 0.1f || Mathf.Abs(GameInput.instance.verticalMove) > 0.1f)
            {
                var newDirection = GetCorrectedForward();

                //AirMove
                transform.position += newDirection * Time.deltaTime * _jump.jumpSpeed / 2;
            }

        }
        _pc.anim.SetFloat("velocityY", _rb.velocity.y);

    }

    public void Exit()
    {

    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _jump = GetComponent<Jumper>();
        _pc = GetComponent<PlayerController>();
        _cam = GetComponent<CharacterMove>().cam;
        //Initialize LandChecker 
        _lc = GetComponentInChildren<LandChecker>();
        _aES = GetComponentInChildren<AnimatorEventsBehaviour>();
    }

    Vector3 GetCorrectedForward()
    {
        //Get inputs
        _horizontal = GameInput.instance.horizontalMove;
        _vertical = GameInput.instance.verticalMove;

        //Get cameraForward(2D floor plain)
        var camForwardWithOutY = new Vector3(_cam.transform.forward.x, 0, _cam.transform.forward.z);
        var anglesign = Vector3.Cross(new Vector3(0, 0, 1), camForwardWithOutY).y > 0 ? 1 : -1;
        var _angleCorrection = Vector3.Angle(new Vector3(0, 0, 1), camForwardWithOutY) * anglesign;

        //Get forward multiplying the input vector3 with the quaternion containing the camera angle
        return (Quaternion.Euler(0f, _angleCorrection, 0f) * new Vector3(_horizontal, 0, _vertical)).normalized;
    }
}
	
