using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Jumper : MonoBehaviour {

    private Collider _col;
    private Rigidbody _rb;
    private FallState _fall;
    private LandChecker _lc;

    public float jumpSpeed;
    public float jumpForce;

    //Fall multiplier Variables
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private float _horizontal;
    private float _vertical;


    [HideInInspector]
    public bool forwardJump;

    private CameraFMS cam;
    private PlayerController _pc;
    private AnimatorEventsBehaviour _aES;
    private CharacterMove _cm;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        cam = GetComponent<CharacterMove>().cam;
        _pc = GetComponent<PlayerController>();
        _lc = GetComponentInChildren<LandChecker>();
        _fall = GetComponent<FallState>();
        _aES = GetComponentInChildren<AnimatorEventsBehaviour>();
        _cm = GetComponent<CharacterMove>();
    } 

    public void Enter()
    {
        _cm.SetColliderDimensions(0);
        //Apply jump force
        _rb.velocity = Vector3.up * jumpForce;
        forwardJump = false;

        //If jumping in place or forward
        _horizontal = GameInput.instance.horizontalMove;
        _vertical = GameInput.instance.verticalMove;
        forwardJump = (Mathf.Abs(_horizontal) > 0.1f || Mathf.Abs(_vertical) > 0.1f) &&  !_pc.CheckForwardCollision(GetCorrectedForward(), true);
        
        //Set Animator Parameter
        _pc.anim.SetBool("toJump", true);

        _aES.landEnd = false;
    }

    public void Execute()
    {
        if(_rb.velocity.y <= 0) _pc.land = _lc.land;

        if (!_pc.CheckForwardCollision(GetCorrectedForward(), true))
        {
            if(forwardJump)
            {
                transform.position += transform.forward * jumpSpeed * Time.deltaTime;
            }

            if(Mathf.Abs(GameInput.instance.horizontalMove) > 0.1f || Mathf.Abs(GameInput.instance.verticalMove) > 0.1f )
            {
                var newDirection = GetCorrectedForward();
            
                //AirMove
                transform.position += newDirection * Time.deltaTime * jumpSpeed / 2;
            }

        }
        if (!GameInput.instance.jumpButton)
        {
            //Speed up fall
            _rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }   
    }

    public void Exit()
    {
        _pc.anim.SetBool("toJump", false);
    }

    Vector3 GetCorrectedForward()
    {
        //Get inputs
        _horizontal = GameInput.instance.horizontalMove;
        _vertical = GameInput.instance.verticalMove;

        //Get cameraForward(2D floor plain)
        var camForwardWithOutY = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
        var anglesign = Vector3.Cross(new Vector3(0, 0, 1), camForwardWithOutY).y > 0 ? 1 : -1;
        var _angleCorrection = Vector3.Angle(new Vector3(0, 0, 1), camForwardWithOutY) * anglesign;

        //Get forward multiplying the input vector3 with the quaternion containing the camera angle
        return (Quaternion.Euler(0f, _angleCorrection, 0f) * new Vector3(_horizontal, 0, _vertical)).normalized;
    }
}
