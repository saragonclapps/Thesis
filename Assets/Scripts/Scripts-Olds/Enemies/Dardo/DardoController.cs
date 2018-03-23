using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dardo
{
    public class DardoController : MonoBehaviour {

        #region Global Variables
        //Target
        [Header("Player Head")]
        public Transform _target; 

        //Idle State
        private DardoMovement _move;
        [Header("Idle State Params")]
        public float yShake;
        public float yShakeSpeed;

        //Atack State
        [Header("Atack State Params")]
        public float atackSpeed;

        //Charging State
        [Header("Charge State Params")]
        public float rotationSpeed;
        public float chargingSpeed;
        public float moveSpeed;
        public float chargeMaxTime;

        //Chase State
        [Header("Chase State Params")]
        public float chaseDistanceToTravel;

        //Return State
        [Header("Return State Params")]
        public float returnDistanceToTravel;

        //Inputs Params
        private DardoAbsorver _dA;
        [HideInInspector] public bool isCharging = false;
        [HideInInspector] public bool atackIsComplete = false;
        private Vector3 originalPosition;

        public float viewDistance;
        public float chaseDistance;
        public float idleDistance;
        public float dardoDamage;

        FSM<Inputs> _fsm;
        ChargeState _charge;
        #endregion

        void Awake()
        {

            //Initializations
            _dA = GetComponent<DardoAbsorver>();
            originalPosition = transform.position; //Se cambia luego en el inicializacion del pool;
            _move = new DardoMovement(transform, yShake, rotationSpeed, moveSpeed, atackSpeed);

            //Fsm states initialization
            var idleState = new IdleState(_move, yShakeSpeed);
            var atackState = new AtackState(_move);
            _charge = new ChargeState(_move, chargingSpeed, this, chargeMaxTime);
            var beeingAbsorvedState = new BeeingAbsorvedState();
            var returningState = new ReturningState(_move, returnDistanceToTravel, yShakeSpeed);
            var chaseState = new ChaseState(_move, chaseDistanceToTravel, yShakeSpeed);


            //Fsm Transitions
            var idleTransitions = new Dictionary<Inputs, IState<Inputs>>();
            idleTransitions.Add(Inputs.Charge, _charge);
            idleTransitions.Add(Inputs.Atracted, beeingAbsorvedState);

            var chargeTransitions = new Dictionary<Inputs, IState<Inputs>>();
            chargeTransitions.Add(Inputs.Atack, atackState);
            chargeTransitions.Add(Inputs.NotInView, chaseState);
            chargeTransitions.Add(Inputs.Atracted, beeingAbsorvedState);

            var atackTransitions = new Dictionary<Inputs, IState<Inputs>>();
            atackTransitions.Add(Inputs.Charge, _charge);
            atackTransitions.Add(Inputs.NotInView, chaseState);
            atackTransitions.Add(Inputs.Atracted, beeingAbsorvedState);
            atackTransitions.Add(Inputs.OutOfRange,returningState);

            var chaseTransitions = new Dictionary<Inputs, IState<Inputs>>();
            chaseTransitions.Add(Inputs.OutOfRange, returningState);
            chaseTransitions.Add(Inputs.Idle, idleState);
            chaseTransitions.Add(Inputs.Atracted, beeingAbsorvedState);

            var returnTransitions = new Dictionary<Inputs, IState<Inputs>>();
            returnTransitions.Add(Inputs.Idle, idleState);
            returnTransitions.Add(Inputs.Charge, _charge);
            returnTransitions.Add(Inputs.Atracted, beeingAbsorvedState);

            var beeingAbsorvedTransitions = new Dictionary<Inputs, IState<Inputs>>();
            beeingAbsorvedTransitions.Add(Inputs.Idle, idleState);
            beeingAbsorvedTransitions.Add(Inputs.Charge, _charge);
            beeingAbsorvedTransitions.Add(Inputs.NotInView, chaseState);
            beeingAbsorvedTransitions.Add(Inputs.OutOfRange, returningState);

            idleState.Transitions = idleTransitions;
            atackState.Transitions = atackTransitions;
            _charge.Transitions = chargeTransitions;
            beeingAbsorvedState.Transitions = beeingAbsorvedTransitions;
            returningState.Transitions = returnTransitions;
            chaseState.Transitions = chaseTransitions;

            _fsm = new FSM<Inputs>(idleState);

            
        }

        void Start()
        {
            EventManager.AddEventListener(GameEvent.SMALLABSORVABLE_REACHED, goBackToPool);
            EventManager.AddEventListener(GameEvent.PLAYER_DIE, PlayerDie);
            EventManager.AddEventListener(GameEvent.DARDO_DIE, goBackToPool);
        }

        void Execute()
        {
            CheckInputs();
            _fsm.Execute();
        }

        private void CheckInputs()
        {
            if(_target != null)
            {
                
                var distance = Mathf.Abs((_target.position - transform.position).magnitude);
                var distanceToOriginalPos = Mathf.Abs((transform.position - originalPosition).magnitude);

                if (_dA.isBeeingAbsorved) _fsm.ProcessInput(Inputs.Atracted);
                else if (_charge.tick > chargeMaxTime && distance < viewDistance && !atackIsComplete) _fsm.ProcessInput(Inputs.Atack);
                else if (distance < viewDistance && !isCharging || atackIsComplete)_fsm.ProcessInput(Inputs.Charge);
                else if (distance < chaseDistance && !isCharging) _fsm.ProcessInput(Inputs.NotInView);
                else if (distance > chaseDistance && !isCharging) _fsm.ProcessInput(Inputs.OutOfRange);

                if (distanceToOriginalPos < idleDistance) _fsm.ProcessInput(Inputs.Idle);

            }
        }

        public void Configure(Transform tr, Vector3 initialpos, Transform parent)
        {
            _target = tr;
            originalPosition = initialpos;
            transform.SetParent(parent);
            transform.position = initialpos;
            _move.Configure(tr, initialpos);
        }


        private void PlayerDie(object[] parameterContainer)
        {
            _fsm.ProcessInput(Inputs.OutOfRange);
        }

        #region Pool
        private void goBackToPool(object[] parameterContainer)
        {
            if ((GameObject)parameterContainer[0] == gameObject)
            {
                if((bool)parameterContainer[1])
                    BulletManager.instance.AddItemToBag(Items.DARDO);
                EnemyManager.instance.ReturnDardoToPool(this);
            }
        }

        public static void InitializeDardo(DardoController dardoObj)
        {
            dardoObj.gameObject.SetActive(true);
            dardoObj.GetComponent<DardoAbsorver>().isAbsorved = false;
            UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, dardoObj.Execute);
        }

        public static void DestroyDardo(DardoController dardoObj)
        {
            dardoObj.gameObject.SetActive(false);
            UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, dardoObj.Execute);
        }
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Shield")
            {
                EventManager.DispatchEvent(GameEvent.DARDO_DIE, gameObject, false);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.layer == 9)
            {
                atackIsComplete = true;
                EventManager.DispatchEvent(GameEvent.DARDO_HIT, _target.position, transform.position);
                EventManager.DispatchEvent(GameEvent.PLAYER_TAKE_DAMAGE, dardoDamage);
            }
        }
    }

    public enum Inputs
    {
        Atack,
        Charge,
        NotInView,
        OutOfRange,
        Idle,
        Atracted
    }
}

