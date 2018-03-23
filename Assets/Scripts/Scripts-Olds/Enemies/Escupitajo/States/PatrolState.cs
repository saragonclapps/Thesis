using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Escupitajo
{

    public class PatrolState : IState<Inputs> {

        Dictionary<Inputs, IState<Inputs>> _transitions;

        //Movement Variables
        Transform _waypointTransform;
        Transform _transform;
        float _patrolSpeed;
        NavMeshAgent _agent;

        //Escupitajo Reference
        EscupitajoController _eFSM;

        //View Config
        View _view;
        float _normalViewAngle;
        float _normalVireDistance;

        public PatrolState(Transform initialWPTransform, Transform t, float patrolSpeed, NavMeshAgent agent ,EscupitajoController eFSM, View view, float nAng, float nDist)
        {
            _waypointTransform = initialWPTransform;
            _transform = t;
            _patrolSpeed = patrolSpeed;
            _agent = agent;
            
            _eFSM = eFSM;

            _view = view;
            _normalViewAngle = nAng;
            _normalVireDistance = nDist;
        }

        public void Enter()
        {
            _view.Configure(_normalViewAngle, _normalVireDistance);
            _agent.speed = _patrolSpeed;
            _agent.SetDestination(_waypointTransform.position);
        }

        public void Execute(){}

        public void Exit(){}

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
    }
}
