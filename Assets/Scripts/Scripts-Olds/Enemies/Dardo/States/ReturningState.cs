using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dardo
{
    public class ReturningState : IState<Inputs> {

        public Dictionary<Inputs, IState<Inputs>> _transitions;

        DardoMovement _dM;
        float _distanceToTravel;
        float _yShakeSpeed;

        public ReturningState(DardoMovement dM, float distanceToTravel, float yShakeSpeed)
        {
            _dM = dM;
            _distanceToTravel = distanceToTravel;
            _yShakeSpeed = yShakeSpeed;
        }


        public void Execute()
        {
            _dM.ReturningMove(_distanceToTravel);
            _dM.FairyMovement(_yShakeSpeed, false);
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
