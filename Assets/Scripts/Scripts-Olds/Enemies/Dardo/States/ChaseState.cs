using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dardo
{
    public class ChaseState : IState<Inputs> {

        public Dictionary<Inputs, IState<Inputs>> _transitions;

        DardoMovement _dM;
        float _distanceMove;
        float _yShakeSpeed;

        public ChaseState(DardoMovement dM, float distanceMove, float yShakeSpeed)
        {
            _dM = dM;
            _distanceMove = distanceMove;
            _yShakeSpeed = yShakeSpeed;
        }


        public void Execute()
        {
            _dM.ChaseMovement(_distanceMove);
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
