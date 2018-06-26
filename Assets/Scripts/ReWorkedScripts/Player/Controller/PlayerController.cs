using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPCamera;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour {

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
        public float jumpTolerance;

        [Header("Collision Parameters")]
        public float collisionDistance;
        public LayerMask lm;

        [Header("Fall Parameters")]
        public float fallDistance;
        public LayerMask fallLayer;

        //States
        IdleState idleState;
        MoveState moveState;
        JumpState jumpState;
        FallState fallState;
        LandState landState;

        Animator _anim;

        [Header("Camera Reference")]
        //public CameraFMS cam;
        public CameraFSM cam2;

        CameraController _camController;
        AnimatorEventsBehaviour _aEB;
        Rigidbody _rB;

        //Sensors
        LandChecker _lC;
        [HideInInspector]
        public ForwardChecker forwardCheck;
        int fallCount;

        FSM<Inputs> _fsm;
        public FSM<Inputs> Fsm { get { return _fsm; } }

        [HideInInspector]
        public bool isSkillLocked;
        [HideInInspector]
        public bool fixedCamera;

        void Awake()
        {
            _anim = GetComponentInChildren<Animator>();
            _lC = GetComponentInChildren<LandChecker>();
            //_camController = cam.GetComponent<CameraController>();
            _aEB = GetComponentInChildren<AnimatorEventsBehaviour>();
            _rB = GetComponent<Rigidbody>();
            forwardCheck = GetComponentInChildren<ForwardChecker>();
            isSkillLocked = false;

            #region FSM
            idleState = new IdleState(this, _anim, cam2.transform, transform, cam2);
            moveState = new MoveState(cam2, transform, angleTurnTolerance, idleTurnTolerance, runningTurnSpeed, speed, this, _aEB, _anim);
            jumpState = new JumpState(_rB, cam2, this, _lC, _aEB, transform, _anim, jumpForce, jumpSpeed);
            fallState = new FallState(_rB, this, cam2, _lC, _aEB, transform, _anim, jumpSpeed);
            landState = new LandState(_anim, this, _aEB, cam2);

            //Fsm Transitions
            var idleTransitions = new Dictionary<Inputs, IState<Inputs>>();
            idleTransitions.Add(Inputs.Move, moveState);
            idleTransitions.Add(Inputs.Fall, fallState);
            idleTransitions.Add(Inputs.Jump, jumpState);

            var moveTransitions = new Dictionary<Inputs, IState<Inputs>>();
            moveTransitions.Add(Inputs.Idle, idleState);
            moveTransitions.Add(Inputs.Jump, jumpState);
            moveTransitions.Add(Inputs.Fall, fallState);

            var jumpTransitions = new Dictionary<Inputs, IState<Inputs>>();
            jumpTransitions.Add(Inputs.Land, landState);
            jumpTransitions.Add(Inputs.Fall, fallState);

            var fallTransitions = new Dictionary<Inputs, IState<Inputs>>();
            fallTransitions.Add(Inputs.Land, landState);

            var landTransitions = new Dictionary<Inputs, IState<Inputs>>();
            landTransitions.Add(Inputs.EndLand, idleState);
            landTransitions.Add(Inputs.Jump, jumpState);

            idleState.Transitions = idleTransitions;
            moveState.Transitions = moveTransitions;
            jumpState.Transitions = jumpTransitions;
            fallState.Transitions = fallTransitions;
            landState.Transitions = landTransitions;

            
            #endregion

            fallCount = 0;
        }

        void Start ()
        {
            _fsm = new FSM<Inputs>(idleState);
            UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
            EventManager.AddEventListener(GameEvent.CAMERA_FIXPOS, ToFixedCamera);
            EventManager.AddEventListener(GameEvent.CAMERA_NORMAL, ToNormalCamera);
        }

        void ToFixedCamera(object[] parameterContainer)
        {
            fixedCamera = true;
        }

        void ToNormalCamera(object[] parameterContainer)
        {
            fixedCamera = false;
        }

        void Execute () {
            CheckInputs();
            _fsm.Execute();
        }

        void CheckInputs()
        {
            
            if (CheckMove())
            {
                _fsm.ProcessInput(Inputs.Move);
            }
            else _fsm.ProcessInput(Inputs.Idle);

            if (GameInput.instance.initialJumpButton && !land && CheckJump()) _fsm.ProcessInput(Inputs.Jump);

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
                _fsm.ProcessInput(Inputs.Fall);
            }

        }

        public bool CheckMove()
        {
            return Mathf.Abs(GameInput.instance.horizontalMove) > 0.1f || Mathf.Abs(GameInput.instance.verticalMove) > 0.1f;
        }

        public bool CheckFall()
        {
            return fallCount >= 2 && !Physics.Raycast(transform.position, -transform.up, fallDistance, fallLayer);
        }

        bool CheckJump()
        {
            Debug.DrawLine(transform.position + transform.up * 1.6f, transform.position + transform.up * 1.6f + transform.up * jumpTolerance);
            return !Physics.Raycast(transform.position + transform.up * 1.6f, transform.up, jumpTolerance);
        }

        private void OnDestroy()
        {
            UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
        }

    }



}
