using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPCamera
{
    public class CameraFSM : MonoBehaviour
    {
        FSM<Inputs> _fsm;
        public FSM<Inputs> Fsm { get { return _fsm; } }

        //States
        public NormalState normalState { get { return _normalState; } }
        NormalState _normalState;
        FixedState _fixedState;

        #region NormalState Variables
        [Header("Normal State Variables")]
        public Transform _lookAt;
        [Range(0.1f,1f)]
        public float positionSmoothness;
        [Range(0f, 5f)]
        public float speed = 1.8f;
        public float unadjustedDistance;
        public LayerMask collisionLayer;
        Camera _cam;
        GameInput _I;
        #endregion

        #region FixedState Variables
        [Header("Fixed State Variables")]
        public float xRotationSpeed;
        public float yRotationSpeed;
        #endregion

        void Awake()
        {
            _cam = GetComponent<Camera>();
            _I = GameInput.instance;
            #region FSM
            _normalState = new NormalState(_lookAt, transform, speed, positionSmoothness, unadjustedDistance, _cam, collisionLayer, _I);
            _fixedState = new FixedState(transform, xRotationSpeed, yRotationSpeed,_lookAt);


            var normalTransitions = new Dictionary<Inputs, IState<Inputs>>();
            normalTransitions.Add(Inputs.TO_FIXED, _fixedState);

            var fixedTransitions = new Dictionary<Inputs, IState<Inputs>>();
            fixedTransitions.Add(Inputs.TO_NORMAL, _normalState);

            _normalState.Transitions = normalTransitions;
            _fixedState.Transitions = fixedTransitions;

            _fsm = new FSM<Inputs>(_normalState);
            #endregion
        }
        // Use this for initialization
        void Start ()
        {
            UpdatesManager.instance.AddUpdate(UpdateType.LATE, Execute);
            EventManager.AddEventListener(GameEvent.CAMERA_FIXPOS, ToFixed);
            EventManager.AddEventListener(GameEvent.CAMERA_NORMAL, ToNormal);
	    }

        // Update is called once per frame
        void Execute () {
            _fsm.Execute();
            CheckInputs();
	    }

        void CheckInputs()
        {
            
        }

        void ToFixed(object[] parameterContainer)
        {
            _fixedState.targetX = (float)parameterContainer[0];
            _fixedState.targetY = (float)parameterContainer[1];
            _fixedState.targetDistance = (float)parameterContainer[2];
            _fsm.ProcessInput(Inputs.TO_FIXED);
        }

        void ToNormal(object[] parameterContainer)
        {
            _fsm.ProcessInput(Inputs.TO_NORMAL);
        }

        private void OnDestroy()
        {
            UpdatesManager.instance.RemoveUpdate(UpdateType.LATE, Execute);
            EventManager.RemoveEventListener(GameEvent.CAMERA_FIXPOS, ToFixed);
            EventManager.RemoveEventListener(GameEvent.CAMERA_NORMAL, ToNormal);
        }
    }

}
