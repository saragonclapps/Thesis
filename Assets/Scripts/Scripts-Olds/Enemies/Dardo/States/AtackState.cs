using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dardo
{
    public class AtackState : IState<Inputs> {

        public Dictionary<Inputs, IState<Inputs>> _transitions;

        DardoMovement _dM;

        public AtackState(DardoMovement dM)
        {
            _dM = dM;
        }

        public void Enter(){}

        public void Execute()
        {
            _dM.AtackMove();
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
