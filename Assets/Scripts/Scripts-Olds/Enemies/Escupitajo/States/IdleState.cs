using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Escupitajo
{
    public class IdleState : IState<Inputs>
    {
        Dictionary<Inputs, IState<Inputs>> _transitions;
        //Escupitajo Variables
        Transform _transform;

        //Idle Scout Movement
        AnimationCurve _scoutCurve;
        public float idleTime;
        float _idleMaxTime;
        float _maxScoutAngle;
        Vector3 idleForward;

        //View variables
        View _view;
        float _alertViewAngle;
        float _alertVireDistance;

        public IdleState(Transform t, AnimationCurve c,float idleMaxTime , float maxAng, View view, float aAng, float aDist)
        {
            _transform = t;
            _scoutCurve = c;
            _idleMaxTime = idleMaxTime;
            _maxScoutAngle = maxAng;
            _view = view;
            _alertViewAngle = aAng;
            _alertVireDistance = aDist;
        }

        public void Enter()
        {
            idleTime = 0;
            idleForward = _transform.forward;
            _view.Configure(_alertViewAngle, _alertVireDistance);
        }

        public void Execute()
        {
            idleTime += Time.deltaTime;
            var idleAngle = _maxScoutAngle * _scoutCurve.Evaluate(idleTime / _idleMaxTime);
            var rotation = Quaternion.AngleAxis(idleAngle, Vector3.up);
            _transform.forward = rotation * idleForward;
        }

        public void Exit()
        {
            idleTime = 0;
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
    }

    

}
