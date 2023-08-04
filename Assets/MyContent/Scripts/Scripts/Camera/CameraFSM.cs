using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPCamera
{
    public class CameraFSM : MonoBehaviour
    {
        public FSM<Inputs> Fsm { get; private set; }

        //States
        public NormalState normalState { get; private set; }
        private FixedState _fixedState;
        private StoryState _storyState;

        #region NormalState Variables

        [Header("Normal State Variables")] 
        public Transform lookAt;
        [Range(0.1f, 1f)] 
        public float positionSmoothness;
        [Range(0f, 5f)] 
        public float speed = 1.8f;
        public float unadjustedDistance;
        public LayerMask collisionLayer;
        
        private Camera _cam;
        private GameInput _gameInput;

        #endregion

        #region FixedState Variables

        #endregion

        private void Awake()
        {
            _cam = GetComponent<Camera>();
            _gameInput = GameInput.instance;

            #region FSM

            normalState = new NormalState(lookAt, transform, speed, positionSmoothness, unadjustedDistance, _cam,
                collisionLayer, _gameInput);
            _fixedState = new FixedState(transform, lookAt, unadjustedDistance);
            _storyState = new StoryState(_cam);


            var normalTransitions = new Dictionary<Inputs, IState<Inputs>>();
            normalTransitions.Add(Inputs.TO_FIXED, _fixedState);
            normalTransitions.Add(Inputs.TO_DEMO, _storyState);

            var fixedTransitions = new Dictionary<Inputs, IState<Inputs>>();
            fixedTransitions.Add(Inputs.TO_NORMAL, normalState);
            fixedTransitions.Add(Inputs.TO_DEMO, _storyState);
            fixedTransitions.Add(Inputs.TO_FIXED, _fixedState);

            var storyTransitions = new Dictionary<Inputs, IState<Inputs>>();
            storyTransitions.Add(Inputs.TO_NORMAL, normalState);
            storyTransitions.Add(Inputs.TO_FIXED, _fixedState);

            normalState.Transitions = normalTransitions;
            _fixedState.Transitions = fixedTransitions;
            _storyState.Transitions = storyTransitions;

            Fsm = new FSM<Inputs>(normalState);

            #endregion

            EventManager.AddEventListener(GameEvent.CAMERA_FIXPOS, ToFixed);
            EventManager.AddEventListener(GameEvent.CAMERA_NORMAL, ToNormal);
            EventManager.AddEventListener(GameEvent.CAMERA_STORY, ToStory);
        }

        // Use this for initialization
        void Start()
        {
            UpdatesManager.instance.AddUpdate(UpdateType.LATE, Execute);
        }


        // Update is called once per frame
        void Execute()
        {
            Fsm.Execute();
            CheckInputs();
        }

        void CheckInputs()
        {
        }

        private void ToStory(object[] parameterContainer)
        {
            //_storyState.update = (Action<Transform>)parameterContainer[0];
            var camTag = (string) parameterContainer[0];
            //CutScenesManager.instance.ActivateCutSceneCamera(camTag);
            _storyState.cutSceneCamera = CutScenesManager.instance.GetCamera(camTag);
            Fsm.ProcessInput(Inputs.TO_DEMO);
        }

        void ToFixed(object[] parameterContainer)
        {
            _fixedState.targetX = (float) parameterContainer[0];
            _fixedState.targetY = (float) parameterContainer[1];
            _fixedState.targetDistance = (float) parameterContainer[2];
            Fsm.ProcessInput(Inputs.TO_FIXED);
        }

        void ToNormal(object[] parameterContainer)
        {
            Fsm.ProcessInput(Inputs.TO_NORMAL);
        }

        private void OnDestroy()
        {
            UpdatesManager.instance.RemoveUpdate(UpdateType.LATE, Execute);
            EventManager.RemoveEventListener(GameEvent.CAMERA_FIXPOS, ToFixed);
            EventManager.RemoveEventListener(GameEvent.CAMERA_NORMAL, ToNormal);
            EventManager.RemoveEventListener(GameEvent.CAMERA_STORY, ToStory);
        }
    }
}