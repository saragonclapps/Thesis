using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dardo
{
    public class IdleState : IState<Inputs>
    {
        public Dictionary<Inputs, IState<Inputs>> _transitions;

        DardoMovement _dM;
        float _yShakeSpeed;

        public IdleState(DardoMovement dM, float yShakeSpeed)
        {
            _dM = dM;
            _yShakeSpeed = yShakeSpeed;
        }


        public void Execute()
        {
            _dM.FairyMovement(_yShakeSpeed, true);
        }

        public void Enter(){}
        public void Exit(){}

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
    }

}
