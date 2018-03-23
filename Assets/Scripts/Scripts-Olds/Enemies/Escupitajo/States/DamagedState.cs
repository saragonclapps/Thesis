using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Escupitajo
{
    public class DamagedState : IState<Inputs>
    {
        Dictionary<Inputs, IState<Inputs>> _transitions;

        Transform _transform;
        AnimationCurve _curve;
        float _maxAngle;

        //Change Eye Material
        Renderer _renderer;
        Material _closedEyeMat;
        Material _auxMat;

        //Timmer
        float _timmer = 5;
        public float tick;

        //Initial Forward
        Vector3 initialForward;

        //Navmesh
        NavMeshAgent _agent;


        public DamagedState(Transform t, AnimationCurve c, float angle, Renderer ren, Material cemat, float timmer, NavMeshAgent agent)
        {
            _transform = t;
            _curve = c;
            _maxAngle = angle;
            _renderer = ren;
            _closedEyeMat = cemat;
            _timmer = timmer;
            _agent = agent;
        }

        public void Enter()
        {
            tick = 0;
            initialForward = _transform.forward;
            _agent.SetDestination(_transform.position);
            _auxMat = _renderer.material;
            _renderer.material = _closedEyeMat;
        }

        public void Execute()
        {
            //TODO: Referencias y la textura del ojo cerrado, tambien agregar el nuevo estado
            tick += Time.deltaTime;
            var idleAngle = _maxAngle * _curve.Evaluate(tick);
            var rotation = Quaternion.AngleAxis(idleAngle, Vector3.up);
            _transform.forward = rotation * initialForward;

        }

        public void Exit()
        {
            _renderer.material = _auxMat;
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
    }

}
