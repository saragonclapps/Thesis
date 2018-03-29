using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour {

    private float _horizontal;
    private float _vertical;
    private float _rotation;

    public float aimingSpeed;
    public float rotationSpeed;

    [SerializeField]
    private bool _isEnterRotation;
    private Quaternion _cameraRotation;

    public bool isAiming;
    public Transform cameraTarget;

    private ArmRotator _aR;
    private CharacterMove _cm;
    private PlayerController _pc;

    private void Start()
    {
        isAiming = false;
        _aR = GetComponentInChildren<ArmRotator>();
        _pc = GetComponent<PlayerController>();
        _cm = GetComponent<CharacterMove>();
    }

    public void Enter()
    {
        isAiming = true;
        _aR.aimToggle = true;
        _isEnterRotation = true;
        var camForwardWithoutY = new Vector3(transform.position.x - _cm.cam.transform.position.x, 0, transform.position.z - _cm.cam.transform.position.z);
        _cameraRotation = Quaternion.LookRotation((camForwardWithoutY).normalized);

        _cm.cam.aimToogle = true;
        _pc.anim.SetTrigger("toAim");
        RaycastHit rch;

        if(Physics.Raycast(transform.position, transform.forward, out rch, _pc.collisionDistance))
        {
            transform.position -= transform.forward * (rch.distance - 0.2f);
        }
    }

    public void Execute()
    {
        if(_isEnterRotation)
        {
            //var targetRotation = Quaternion.Euler(_cameraRotation.x, 0f, _cameraRotation.z);
            var angle = Mathf.Abs(transform.rotation.eulerAngles.y - _cameraRotation.eulerAngles.y);
            //Debug.Log(angle);

            if (angle > 1)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, _cameraRotation, 0.5f);
            }
            else _isEnterRotation = false;
        }
        else
        {
            _vertical = GameInput.instance.verticalMove;
            _horizontal = GameInput.instance.horizontalMove;
            _rotation = GameInput.instance.cameraRotation;

            var movedir = transform.forward * _vertical + transform.right * _horizontal;
            if (!_pc.CheckForwardCollision(movedir, false))
            {
                transform.position += transform.forward * _vertical * aimingSpeed * Time.deltaTime;
                transform.position += transform.right * _horizontal * aimingSpeed * Time.deltaTime;
            }

            transform.Rotate(0f, _rotation * rotationSpeed * Time.deltaTime, 0f);

            //Set Animator Parameters
            _pc.anim.SetFloat("horizontalSpeed", _horizontal);
            _pc.anim.SetFloat("verticalSpeed", _vertical);
        }
    }

    public void Exit()
    {
        isAiming = false;
        _aR.aimToggle = true;
        _cm.cam.aimToogle = true;
        _pc.anim.SetTrigger("toIdle");
    }
}
