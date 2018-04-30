using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class AimState : IState<Inputs>
    {
        public Dictionary<Inputs, IState<Inputs>> _transitions;

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

        PlayerController2 _pC2;
        //ArmRotator _aR;
        Transform transform;
        CameraFMS _cam;
        Animator _anim;

        public AimState(PlayerController2 pC2, /*ArmRotator aR,*/ Transform t, CameraFMS cam, Animator anim)
        {
            _pC2 = pC2;
            //_aR = aR;
            transform = t;
            _cam = cam;
            isAiming = false;
            _anim = anim;
        }

        public void Enter()
        {
            isAiming = true;
            //_aR.aimToggle = true;
            _isEnterRotation = true;
            var camForwardWithoutY = new Vector3(transform.position.x - _cam.transform.position.x, 0, transform.position.z - _cam.transform.position.z);
            _cameraRotation = Quaternion.LookRotation((camForwardWithoutY).normalized);

            _cam.aimToogle = true;
            _anim.SetTrigger("toAim");
            RaycastHit rch;

            if (Physics.Raycast(transform.position, transform.forward, out rch, _pC2.collisionDistance))
            {
                transform.position -= transform.forward * (rch.distance - 0.2f);
            }
        }

        public void Execute()
        {
            if (_isEnterRotation)
            {
                var angle = Mathf.Abs(transform.rotation.eulerAngles.y - _cameraRotation.eulerAngles.y);
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
                if (!_pC2.CheckForwardCollision(movedir, false))
                {
                    transform.position += transform.forward * _vertical * aimingSpeed * Time.deltaTime;
                    transform.position += transform.right * _horizontal * aimingSpeed * Time.deltaTime;
                }

                transform.Rotate(0f, _rotation * rotationSpeed * Time.deltaTime, 0f);

                //Set Animator Parameters
                _anim.SetFloat("horizontalSpeed", _horizontal);
                _anim.SetFloat("verticalSpeed", _vertical);
            }
            Debug.Log("AimState");
        }

        public void Exit()
        {
            isAiming = false;
            //_aR.aimToggle = true;
            _cam.aimToogle = true;
            _anim.SetTrigger("toIdle");
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }

    }

}
