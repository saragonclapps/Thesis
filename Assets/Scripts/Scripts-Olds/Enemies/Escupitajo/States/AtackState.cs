using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Escupitajo
{
    public class AtackState : IState<Inputs> {

        public Dictionary<Inputs, IState<Inputs>> _transitions;

        //Controller
        EscupitajoController _eC;
        NavMeshAgent _agent;
        Transform _transform;
        Transform _mouthTr;

        //View Config
        View _view;
        float _angleView;
        float _distanceView;

        //Atack timmer
        float _timeToShoot;
        float _maxTimeToShoot;

        //Target
        Transform _target;

        //Bullet Speed
        float _bullSpeed;


        public AtackState(EscupitajoController eC, NavMeshAgent agent, Transform t, Transform mtr, View v, float aView, float dView, float mtts, Transform target, float bullSpeed)
        {
            _eC = eC;
            _agent = agent;
            _transform = t;
            _mouthTr = mtr;
            _view = v;
            _angleView = aView;
            _distanceView = dView;
            _maxTimeToShoot = mtts;
            _target = target;
            _bullSpeed = bullSpeed;
        }

        public void Enter()
        {
            _agent.SetDestination(_transform.position);
            _view.Configure(_angleView, _distanceView);
            _eC.atackRange += 2;
            _timeToShoot = 0;
        }

        public void Execute()
        {
            var dir = (_target.position - _transform.position);
            var normalizedDir = (dir - new Vector3(0,dir.y,0)).normalized;
            _transform.forward = normalizedDir;

            _timeToShoot += Time.deltaTime;

            if(_timeToShoot > _maxTimeToShoot)
            {
                var dist = Vector3.Distance(_target.position, _transform.position);
                var time = dist / _bullSpeed;
                var sqrtime = time * time;

                var vel0y = (_target.position.y - _mouthTr.position.y - ((Physics.gravity.y + 2) * sqrtime / 2)) * (1 / time);

                EventManager.DispatchEvent(GameEvent.ESCUPITAJO_SHOOT, _mouthTr, _bullSpeed, vel0y);
                _timeToShoot = 0;
            }
        }

        public void Exit()
        {
            _eC.atackRange -= 2;
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
    }

}
