using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TPCamera
{
    public class StoryState : IState<Inputs>
    {
        Dictionary<Inputs, IState<Inputs>> _transitions;

        Action<Transform> _update;
        public Action<Transform> update { set { _update = value; } }

        Transform transform;

        public StoryState(Transform t)
        {
            transform = t;
        }


        public void Enter()
        {

        }

        public void Execute()
        {
            _update(transform);
        }

        public void Exit()
        {
            
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
    }

}
