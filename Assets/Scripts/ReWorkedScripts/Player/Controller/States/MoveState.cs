using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class MoveState : IState<Inputs>
    {
        public Dictionary<Inputs, IState<Inputs>> _transitions;

        #region Global Variables
        float _speed;
        CameraController _cam;
        Transform transform;

        //Turn Variables
        Vector3 _oldDirection;
        Vector3 _newDirection;
        Vector3 _desiredForward;
        bool _turn;

        float _angleTurnTolerance;

        [Range(0.1f, 0.9f)]
        float _idleTurnSpeed;

        [Range(0.1f, 0.9f)]
        float _runingTurnSpeed;

        //Move Variables
        float _horizontal;
        float _vertical;
        float _movementSpeed;
        float _angleCorrection;

        bool forwardCollision;


        //Player Controller
        PlayerController2 _pC;
        AnimatorEventsBehaviour _aEB;
        Animator[] _anim;
        LeftHandIKControl _lHIK;

        //private LeftHandIKControl _lHIK;

        //private CapsuleCollider _cC;
        #endregion

        public MoveState(CameraController cam, Transform t, float angleTurnTolerance, float idleTurnSpeed, float runingTurnSpeed,float speed, PlayerController2 pC,
                        AnimatorEventsBehaviour aEB, Animator[] anim, LeftHandIKControl lHIK)
        {
            _cam = cam;
            transform = t;
            _angleTurnTolerance = angleTurnTolerance;
            _idleTurnSpeed = idleTurnSpeed;
            _runingTurnSpeed = runingTurnSpeed;
            _speed = speed;
            _pC = pC;
            _aEB = aEB;
            _anim = anim;
            _lHIK = lHIK;

            _oldDirection.x = transform.forward.x;
            _oldDirection.z = transform.forward.z;
            _oldDirection = _oldDirection.normalized;
        }

        public void Enter()
        {
            GetCorrectedForward();
            //In case you have to pivot from idle
            if (_oldDirection != _newDirection)
            {
                var angle = Vector3.Angle(_oldDirection, _newDirection);
                _turn = Mathf.Abs(angle) > _angleTurnTolerance;
            }
            //Set Animator Transition
            for (int i = 0; i < _anim.Length; i++)
            {
                _anim[i].SetFloat("speed", 1);
            }
        }

        public void Execute()
        {
            if (_turn)
            {
                //_cam.positionSmoothness = 0.1f;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_newDirection), _idleTurnSpeed);
                var angle = Vector3.Angle(transform.forward, _newDirection);
                if (Mathf.Abs(angle) < 5)
                {
                    _turn = false;
                    transform.forward = _newDirection;
                }
            }
            else
            {
                //_cam.positionSmoothness = 0.5f;
                GetCorrectedForward();
                //Rotate to the new forward
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_newDirection), _runingTurnSpeed);

                //MoveForward
                if (_aEB.landEnd /*&& !_pC.CheckForwardCollision(_newDirection, true)*/)
                {
                    if (GameInput.instance.sprintButton)
                    {
                        for (int i = 0; i < _anim.Length; i++)
                        {
                            _anim[i].SetBool("sprint", true);
                        }
                        _movementSpeed = 1.2f * _speed;
                        _pC.fallDistance = 0.7f;
                        //SetColliderDimensions(ColliderSettings.SPRINT);
                        //_pc.collisionDistance = 1;
                    }
                    else
                    {
                        for (int i = 0; i < _anim.Length; i++)
                        {
                            _anim[i].SetBool("sprint", false);
                        }
                        _movementSpeed = _speed / 1.2f;
                        _pC.fallDistance = 0.5f;
                        //SetColliderDimensions(ColliderSettings.NORMAL);
                        //_pc.collisionDistance = 0.7f;
                    }

                    transform.position += transform.forward * Time.deltaTime * _movementSpeed;
                }
                for (int i = 0; i < _anim.Length; i++)
                {
                    _anim[i].SetBool("isAbsorbing", _lHIK.ikActive);
                }
            }
        }

        public void Exit()
        {
            _oldDirection.x = transform.forward.x;
            _oldDirection.z = transform.forward.z;
            _oldDirection = _oldDirection.normalized;
            for (int i = 0; i < _anim.Length; i++)
            {
                _anim[i].SetFloat("speed", 0);
                _anim[i].SetBool("isAbsorbing", false);
                _anim[i].SetBool("sprint", false);
            }
            //_cam.positionSmoothness = 0.1f;
            //_pC.isMoving = false;
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
            var camForwardWithOutY = new Vector3(_cam.transform.forward.x, 0, _cam.transform.forward.z);
            var anglesign = Vector3.Cross(new Vector3(0, 0, 1), camForwardWithOutY).y > 0 ? 1 : -1;
            _angleCorrection = Vector3.Angle(new Vector3(0, 0, 1), camForwardWithOutY) * anglesign;

            //Get forward multiplying the input vector3 with the quaternion containing the camera angle
            _newDirection = (Quaternion.Euler(0f, _angleCorrection, 0f) * new Vector3(_horizontal, 0, _vertical)).normalized;
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
        
    }

}
        
