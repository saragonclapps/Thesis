using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

namespace Escupitajo
{ 
    [RequireComponent(typeof(View))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EscupitajoController : MonoBehaviour {

        NavMeshAgent _agent;
        public Waypoint waypoint;
        public float distanceTolerance;

        //Target
        [Header("Player Targets")]
        public PlayerController targetPlayeController;
        public Transform target;

        //Patrol State params
        [Header("Patrol State params")]
        public float speed;
        public Transform waypointTransform;

        //Idle State params
        [Header("Idle State params")]
        public float maxIdleAngle;
        public float maxIdleTime;
        public AnimationCurve idleCurve;

        //View States
        [Header("View States")]
        public float alertViewAngle = 25f;
        public float normalViewAngle = 45f;
        public float alertViewDistance = 10f;
        public float normalViewDistance = 5f;

        //Chase State Params
        [Header("Chase State Params")]
        public float chaseSpeed;

        //Alert State Params 
        [Header("Alert State Params")]
        public AnimationCurve alertCurve;
        public float alertMaxTime;
        public float alertMaxAngle = 60f;

        //Atack State Params
        [Header("Atack State Params")]
        public float maxTimeToShoot;
        public Transform mouthTransform;
        public float bulletSpeed;

        //Damaged State Params
        [Header("Damaged State Params")]
        public float maxDamagedTime;
        public AnimationCurve damageAnimationCurve;
        public float maxAngleDamage;
        public Renderer eyeMaterial;
        public Material closedEyeMaterial;

        public Transform eyeTransform;
        public float eyeRadius = 0.5f;


        //Inputs Params
        [Header("Inputs Params")]
        public float atackRange;
        public float hearRange;
        [HideInInspector]public float sightRange;


        View _view;

        float scoutTime;
        Vector3 scoutForward;

        //FSM
        FSM<Inputs> _fsm;
        IdleState idleState;
        AlertState alertState;
        DamagedState damagedState;

        void Awake()
        {
            _view = GetComponent<View>();
            _agent = GetComponent<NavMeshAgent>();

            //Initialitate FSM
            //States
            var patrol = new PatrolState(waypointTransform, transform, speed, _agent, this, _view, normalViewAngle, normalViewDistance);
            idleState = new IdleState(transform, idleCurve, maxIdleTime ,maxIdleAngle, _view, alertViewAngle, alertViewDistance);
            var chase = new ChaseState(this, _agent, chaseSpeed, transform, _view, alertViewAngle, alertViewDistance, targetPlayeController.transform);
            alertState = new AlertState(this, target, alertCurve, alertMaxTime, alertMaxAngle, transform, _agent, _view, alertViewDistance, alertViewAngle);
            var atack = new AtackState(this, _agent, transform, mouthTransform, _view, alertViewAngle, alertViewDistance, maxTimeToShoot, target, bulletSpeed);
            damagedState = new DamagedState(transform, damageAnimationCurve, maxAngleDamage, eyeMaterial, closedEyeMaterial, maxDamagedTime, _agent);

            //Patrol Transitions
            var patrolTransitions = new Dictionary<Inputs, IState<Inputs>>();
            patrolTransitions.Add(Inputs.Stop, idleState);
            patrolTransitions.Add(Inputs.InSight, chase);
            patrolTransitions.Add(Inputs.Heard, alertState);
            patrolTransitions.Add(Inputs.InRange, atack);
            patrolTransitions.Add(Inputs.Damaged, damagedState);
            patrolTransitions.Add(Inputs.Move, patrol);


            //Idle Transitions
            var idleTransitions = new Dictionary<Inputs, IState<Inputs>>();
            idleTransitions.Add(Inputs.Move, patrol);
            idleTransitions.Add(Inputs.InSight, chase);
            idleTransitions.Add(Inputs.Heard, alertState);
            idleTransitions.Add(Inputs.InRange, atack);
            idleTransitions.Add(Inputs.Damaged, damagedState);

            //Chase Transitions
            var chaseTransitions = new Dictionary<Inputs, IState<Inputs>>();
            chaseTransitions.Add(Inputs.Lost, alertState);
            chaseTransitions.Add(Inputs.InRange, atack);
            chaseTransitions.Add(Inputs.Damaged, damagedState); 

            //Alert Transitions
            var alertTransitions = new Dictionary<Inputs, IState<Inputs>>();
            alertTransitions.Add(Inputs.InSight, chase);
            alertTransitions.Add(Inputs.NotFound, patrol);
            alertTransitions.Add(Inputs.InRange, atack);
            alertTransitions.Add(Inputs.Damaged, damagedState);

            //Atack Transitions
            var atackTransitions = new Dictionary<Inputs, IState<Inputs>>();
            atackTransitions.Add(Inputs.InSight, chase);
            atackTransitions.Add(Inputs.Lost, alertState);
            atackTransitions.Add(Inputs.Damaged, damagedState);

            //DamagedTransitions
            var damagedTransitions = new Dictionary<Inputs, IState<Inputs>>();
            damagedTransitions.Add(Inputs.Recovered, alertState);

            //Add transitions to states
            patrol.Transitions = patrolTransitions;
            idleState.Transitions = idleTransitions;
            chase.Transitions = chaseTransitions;
            alertState.Transitions = alertTransitions;
            atack.Transitions = atackTransitions;
            damagedState.Transitions = damagedTransitions;

            //Initial State
            _fsm = new FSM<Inputs>(idleState);

            waypointTransform.position = waypoint.transform.position;
        }

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
            EventManager.AddEventListener(GameEvent.ESCUPITAJO_RECIEVE_DAMAGE, GetHit);
        }

        

        void Execute()
        {
            _fsm.Execute();
            CheckInputs();
            UpdateSightDistance();
        }


        private void CheckInputs()
        {
            var wpDist = (transform.position - waypoint.transform.position).sqrMagnitude;

            if (wpDist < distanceTolerance)
            {
                if (!waypoint.patrolWaypoint)
                {
                    _fsm.ProcessInput(Inputs.Stop);
                    waypoint = waypoint.next;
                    waypointTransform.position = waypoint.transform.position;
                }
                else
                {
                    waypoint = waypoint.next;
                    waypointTransform.position = waypoint.transform.position;
                    _fsm.ProcessInput(Inputs.Move);
                }
            }

            if (idleState.idleTime > maxIdleTime)
                _fsm.ProcessInput(Inputs.Move);

            if(alertState.alertTime > alertMaxTime)
            {
                _fsm.ProcessInput(Inputs.NotFound);
            }

            //Distance dependant Inputs
            var distance = Vector3.Distance(transform.position, targetPlayeController.transform.position);
            if (distance < atackRange && _view.targetInSight)
            {
                //Target is in view cone and in atack range distance
                _fsm.ProcessInput(Inputs.InRange);
            }
            else if (distance < sightRange && _view.targetInSight)
            {
                //Target is in view cone and in sight distance
                _fsm.ProcessInput(Inputs.InSight);
            }
            //Target is out of range
            else _fsm.ProcessInput(Inputs.Lost);

            if (distance < hearRange && !targetPlayeController.isStealth)
            {
                //Target is in range but not in vision
                _fsm.ProcessInput(Inputs.Heard);
            }

            if(damagedState.tick >= maxDamagedTime)
            {
                _fsm.ProcessInput(Inputs.Recovered);
            }


        }

        private void UpdateSightDistance()
        {
            sightRange = _view.distance;
        }

        private void GetHit(object[] pC)
        {
            if((GameObject)pC[0] == gameObject)
            {
                var hitdistance = Mathf.Abs((((Transform)pC[1]).position - eyeTransform.position).magnitude);
                Debug.Log(hitdistance);
                if (hitdistance < eyeRadius)
                {
                    _fsm.ProcessInput(Inputs.Damaged);
                }
                else
                {
                    _fsm.ProcessInput(Inputs.Heard);
                }
            }
        }
    }

    public enum Inputs
    {
        Move,
        InSight,
        Lost,
        NotFound,
        InRange,
        Stop,
        Heard,
        Damaged,
        Recovered
    }
}

