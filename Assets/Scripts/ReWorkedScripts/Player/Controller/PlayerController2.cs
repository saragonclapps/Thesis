using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController2 : MonoBehaviour {


        //public enum States { Idle, Move, StealthIdle, StealthMove, Locked, Jumping, Falling, Landing }
        //public enum Inputs { Move, Stealth, Aiming, Idle, NotAiming, Unstealth, Jump, Land, Fall }

        [HideInInspector]
        public bool jumpForward;
        [HideInInspector]
        public bool land;

        [Header("Move Parameters")]

        public float speed;
        public float angleTurnTolerance;
        [Range(0, 0.9f)]
        public float idleTurnTolerance;
        [Range(0, 0.9f)]
        public float runningTurnSpeed;

        [Header("Jump Parameters")]
        public float jumpForce;
        public float jumpSpeed;

        [Header("Collision Parameters")]
        public float collisionDistance;
        public LayerMask lm;

        [Header("Fall Parameters")]
        public float fallDistance;
        public LayerMask fallLayer;

        //States
        IdleState idleState;
        MoveState moveState;
        //AimState aimState;
        JumpState jumpState;
        FallState fallState;
        LandState landState;

        Animator _anim;

        [Header("Camera Reference")]
        public CameraFMS cam;

        CameraController _camController;
        AnimatorEventsBehaviour _aEB;
        ArmRotator _aR;
        Rigidbody _rB;
        LeftHandIKControl _lHIK;

        //Land sensors
        LandChecker _lC;
        int fallCount;

        FSM<Inputs> _fsm;

        public FSM<Inputs> Fsm { get { return _fsm; } }

        void Awake()
        {
            _anim = GetComponentInChildren<Animator>();
            _lC = GetComponentInChildren<LandChecker>();
            _camController = cam.GetComponent<CameraController>();
            _aEB = GetComponentInChildren<AnimatorEventsBehaviour>();
            _aR = GetComponentInChildren<ArmRotator>();
            _rB = GetComponent<Rigidbody>();
            _lHIK = GetComponentInChildren<LeftHandIKControl>();

            #region FSM
            idleState = new IdleState(this, _anim, cam.transform, transform);
            moveState = new MoveState(_camController, transform, angleTurnTolerance, idleTurnTolerance, runningTurnSpeed, speed, this, _aEB, _anim, _lHIK);
            //aimState = new AimState(this, _aR, transform, cam, _anim);
            jumpState = new JumpState(_rB, cam, this, _lC, _aEB, transform, _anim, jumpForce, jumpSpeed);
            fallState = new FallState(_rB, this, cam, _lC, _aEB, transform, _anim, jumpSpeed);
            landState = new LandState(_anim, this);

            //Fsm Transitions
            var idleTransitions = new Dictionary<Inputs, IState<Inputs>>();
            idleTransitions.Add(Inputs.Move, moveState);
            //idleTransitions.Add(Inputs.Aiming, aimState);
            idleTransitions.Add(Inputs.Fall, fallState);
            idleTransitions.Add(Inputs.Jump, jumpState);

            var moveTransitions = new Dictionary<Inputs, IState<Inputs>>();
            moveTransitions.Add(Inputs.Idle, idleState);
            //moveTransitions.Add(Inputs.Aiming, aimState);
            moveTransitions.Add(Inputs.Jump, jumpState);
            moveTransitions.Add(Inputs.Fall, fallState);

            /*var aimingTransitions = new Dictionary<Inputs, IState<Inputs>>();
            aimingTransitions.Add(Inputs.NotAiming, idleState);*/

            var jumpTransitions = new Dictionary<Inputs, IState<Inputs>>();
            jumpTransitions.Add(Inputs.Land, landState);
            jumpTransitions.Add(Inputs.Fall, fallState);

            var fallTransitions = new Dictionary<Inputs, IState<Inputs>>();
            fallTransitions.Add(Inputs.Land, landState);

            var landTransitions = new Dictionary<Inputs, IState<Inputs>>();
            landTransitions.Add(Inputs.EndLand, idleState);

            idleState.Transitions = idleTransitions;
            moveState.Transitions = moveTransitions;
            jumpState.Transitions = jumpTransitions;
            fallState.Transitions = fallTransitions;
            landState.Transitions = landTransitions;

            //aimState.Transitions = aimingTransitions;
            _fsm = new FSM<Inputs>(idleState);
            #endregion

            fallCount = 0;
        }

        void Start ()
        {
            UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        }
	

	    void Execute () {
            CheckInputs();
            _fsm.Execute();
        }

        void CheckInputs()
        {
            /*if (GameInput.instance.aimButton)
            {
                if (aimState.isAiming)
                {
                    _fsm.ProcessInput(Inputs.NotAiming);
                    //isAiming = false; //TODO: Ponerlo en el exit de aiming
                }
                else
                {
                    _fsm.ProcessInput(Inputs.Aiming);
                    //isAiming = true; //TODO: Ponerlo en el enter de aiming
                }
            }*/
            if (CheckMove())
            {
                _fsm.ProcessInput(Inputs.Move);
            }
            else _fsm.ProcessInput(Inputs.Idle);

            if (GameInput.instance.initialJumpButton && !land && _aEB.landEnd) _fsm.ProcessInput(Inputs.Jump);

            if (land)
            {
                _fsm.ProcessInput(Inputs.Land);
                land = false;
            }

            if (_aEB.landEnd)
            {
                _fsm.ProcessInput(Inputs.EndLand);
            }

            //Triple check for fall state
            if (_rB.velocity.y < -0.2f && !_lC.land) fallCount++;
            else fallCount = 0;
            if (CheckFall())
            {
                Debug.Log("Proces Fall");
                _fsm.ProcessInput(Inputs.Fall);
            }

        }

        public bool CheckMove()
        {
            return Mathf.Abs(GameInput.instance.horizontalMove) > 0.1f || Mathf.Abs(GameInput.instance.verticalMove) > 0.1f;
        }

        /// <summary>
        /// Checks forward in order to see if the player can move or not
        /// </summary>
        /// <returns></returns>
        public bool CheckForwardCollision(Vector3 moveDirection, bool forward)
        {
            Debug.DrawLine(transform.position, transform.position + transform.forward);
            RaycastHit ray;
            bool didcollide = false;
            for (int i = 0; i < 3; i++)
            {
                var hit = Physics.Raycast(transform.position + new Vector3(0, i * 0.6f, 0), moveDirection, out ray, collisionDistance, lm);
                if (hit)
                {
                    var value = Vector3.Cross(transform.forward, ray.normal);
                    var valueWithoutY = new Vector3(value.x, 0f, value.z);
                    didcollide = didcollide || Vector3.SqrMagnitude(valueWithoutY) <= 0.01;
                }
            }


            return didcollide;
            /*return (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, collisionDistance, lm) ||
                   Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.forward, collisionDistance, lm) ||//Edit
                   Physics.Raycast(transform.position + new Vector3(0, 1.5f, 0), transform.forward, collisionDistance, lm)) && forward ||
                   Physics.Raycast(transform.position + new Vector3(0, 1, 0), moveDirection, collisionDistance, lm) ||
                   Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), moveDirection, collisionDistance, lm) ||
                   Physics.Raycast(transform.position + new Vector3(0, 1.5f, 0), moveDirection, collisionDistance, lm);*/
        }

        public bool CheckFall()
        {
            return fallCount >= 2 && !Physics.Raycast(transform.position, -transform.up, fallDistance, fallLayer);
        }
    }



}
