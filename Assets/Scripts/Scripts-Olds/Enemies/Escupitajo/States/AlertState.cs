using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Escupitajo
{
    public class AlertState : IState<Inputs> {

        public Dictionary<Inputs, IState<Inputs>> _transitions;

        //Escupitajo Controller References
        EscupitajoController _eC;
        Transform _transform;
        NavMeshAgent _agent;

        //Player Target
        Transform _target;

        //Scope Curve
        AnimationCurve _curve;

        //Alert params
        float _alertMaxTime;
        public float alertTime = 0;
        float _alertMaxAngle;

        //View
        View _view;
        float _viewDistance;
        float _viewAngle;


        bool isRotating = true;
        Quaternion _targetRotation;

        Vector3 initialForward;

        public AlertState(EscupitajoController eC, Transform target, AnimationCurve c, float alertMaxTime, float alertMaxAngle, Transform t, NavMeshAgent a, View view, float viewDistance, float viewAngle)
        {
            _eC = eC;
            _target = target;
            _curve = c;
            _alertMaxTime = alertMaxTime;
            _alertMaxAngle = alertMaxAngle;
            _transform = t;
            _agent = a;
            _view = view;
            _viewAngle = viewAngle;
            _viewDistance = viewDistance;
        }

        public void Enter()
        {
            _view.Configure(_viewAngle, _viewDistance);
            _eC.sightRange += 2;
            alertTime = 0;
            isRotating = true;
            var dir = _target.position -_transform.position ;
            dir = new Vector3(dir.x, 0, dir.z);
            _targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            _agent.SetDestination(_transform.position);
        }

        public void Execute()
        {

            if (Quaternion.Angle(_targetRotation, _transform.rotation) < 1)
            {
                
                isRotating = false;
                initialForward = _transform.forward;
            }
            if (isRotating)
            {
                _transform.rotation = Quaternion.Slerp(_transform.rotation, _targetRotation, 0.5f);
            }
            else
            {
                alertTime += Time.deltaTime;
                var alertAngle = _alertMaxAngle * _curve.Evaluate(alertTime / _alertMaxTime * 2);
                var rotation = Quaternion.AngleAxis(alertAngle, Vector3.up);
                _transform.forward = rotation * initialForward;
            }


        }

        public void Exit()
        {
            alertTime = 0;
            _eC.sightRange -= 2;
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
    }

}
