using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dardo
{
    public class ChargeState : IState<Inputs> {

        public Dictionary<Inputs, IState<Inputs>> _transitions;

        public float tick;
        float _chargeMaxTime;
        DardoMovement _dM;
        float _speed;
        DardoController _dardoController;

        public ChargeState(DardoMovement dM, float speed, DardoController dC, float chargeMaxTime)
        {
            _dM = dM;
            _speed = speed;
            _dardoController = dC;
            _chargeMaxTime = chargeMaxTime;
        }

        public void Enter()
        {
            tick = 0;
            _dardoController.isCharging = true;
            _dardoController.atackIsComplete = false;
            _dM.ChargeMoveInit();
        }

        public void Execute()
        {
            if(tick > _chargeMaxTime + Time.deltaTime)
            {
                _dardoController.isCharging = false;
            }
            tick += Time.deltaTime;
            _dM.ChargeMove(1, _speed);
        }

        public void Exit()
        {
            _dardoController.isCharging = false;
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
    }

}
