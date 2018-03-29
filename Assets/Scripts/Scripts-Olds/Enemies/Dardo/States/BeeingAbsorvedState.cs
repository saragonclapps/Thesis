using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dardo
{
    public class BeeingAbsorvedState : IState<Inputs> {

        public Dictionary<Inputs, IState<Inputs>> _transitions;

        public BeeingAbsorvedState(){}
        public void Enter(){}
        public void Execute() {}
        public void Exit(){}

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
    }
}
