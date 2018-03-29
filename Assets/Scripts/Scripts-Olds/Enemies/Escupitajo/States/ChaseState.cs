using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Escupitajo
{
    public class ChaseState : IState<Inputs>
    {
        public Dictionary<Inputs, IState<Inputs>> _transitions;

        //Escupitajo Controller
        EscupitajoController _eC;

        //Movement Params
        NavMeshAgent _agent;
        float _speed;
        Transform _transform;

        //View Params
        View _view;
        float _viewAngle;
        float _viewDistance;

        //Target
        Transform _target;

        public ChaseState(EscupitajoController eC, NavMeshAgent a, float s, Transform t , View v, float vAngle, float vDistance, Transform target)
        {
            _eC = eC;

            _agent = a;
            _speed = s;
            _transform = t;
            _view = v;
            _viewAngle = vAngle;
            _viewDistance = vDistance;

            _target = target;
        }

        public void Enter()
        {
            _eC.sightRange += 2;
            _agent.speed = _speed;
            _view.Configure(_viewAngle, _viewDistance);
        }

        public void Execute()
        {
            _agent.SetDestination(_target.position);
            _transform.LookAt(_target);
        }

        public void Exit()
        {
            _eC.sightRange -= 2;
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
    }

}
