using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent (typeof(FairyMovement))]
[RequireComponent(typeof(DardoAtack))]
public class DardoFSM : MonoBehaviour {

    #region FSM
    public enum States{Idle, Atack, Chase, Returning, BeeingAbsorved }
    public enum Inputs{Atack, NotInView, OutOfRange, Idle, Atracted }

    private Dictionary<States, Action> updates;
    private Dictionary<States, Action> enters;
    private Dictionary<States, Dictionary<Inputs, States>> _transitions;

    public States currentState;
    #endregion

    #region GlobalVariables
    //Target Variables
    private Transform _target;

    const int terrainLayer = 10;
    int layerMask;

    //Config
    public float viewDistance;
    public float chaseDistance;
    public float chaseDistanceToTravel;
    public float chaseWait;
    public float returnDistanceToTravel;
    public float returnWait;

    public float runDistanceToTravel;
    public float runWait;

    private FairyMovement _fm;
    private DardoAtack _atack;
    private SmallSizeObject _sso;
    private Rigidbody _rb;


    #endregion

    public DardoFSM(Transform target)
    {
        SetTarget(target);
    }

    void Awake()
    {
        //Initialize & Config Updates
        updates = new Dictionary<States, Action>();
        updates.Add(States.Idle, Idle);
        updates.Add(States.Atack, Atack);
        updates.Add(States.Chase, Chase);
        updates.Add(States.Returning, Return);
        updates.Add(States.BeeingAbsorved, TryToScape);

        //Initialize & Config Enters
        enters = new Dictionary<States, Action>();
        enters.Add(States.Idle, IdleEnter);
        enters.Add(States.Atack, AtackEnter);
        enters.Add(States.Chase, ChaseEnter);
        enters.Add(States.Returning, ReturnEnter);
        enters.Add(States.BeeingAbsorved, TryToScapeEnter);

        //Transitions
        var idleTransitions = new Dictionary<Inputs, States>();
        idleTransitions.Add(Inputs.Atack, States.Atack);
        idleTransitions.Add(Inputs.Atracted, States.BeeingAbsorved);

        var atackTransitions = new Dictionary<Inputs, States>();
        atackTransitions.Add(Inputs.NotInView, States.Chase);
        atackTransitions.Add(Inputs.Atracted, States.BeeingAbsorved);
        atackTransitions.Add(Inputs.OutOfRange, States.Returning);

        var chaseTransitions = new Dictionary<Inputs, States>();
        chaseTransitions.Add(Inputs.OutOfRange, States.Returning);
        chaseTransitions.Add(Inputs.Atack, States.Atack);
        chaseTransitions.Add(Inputs.Idle, States.Idle);
        chaseTransitions.Add(Inputs.Atracted, States.BeeingAbsorved);

        var returnTransitions = new Dictionary<Inputs, States>();
        returnTransitions.Add(Inputs.Idle, States.Idle);
        returnTransitions.Add(Inputs.Atack, States.Atack);
        returnTransitions.Add(Inputs.Atracted, States.BeeingAbsorved);

        var beeingAbsorvedTranstions = new Dictionary<Inputs, States>();
        beeingAbsorvedTranstions.Add(Inputs.Idle, States.Idle);
        beeingAbsorvedTranstions.Add(Inputs.NotInView, States.Chase);
        beeingAbsorvedTranstions.Add(Inputs.OutOfRange, States.Returning);

        _transitions = new Dictionary<States, Dictionary<Inputs, States>>();
        _transitions.Add(States.Idle, idleTransitions);
        _transitions.Add(States.Atack, atackTransitions);
        _transitions.Add(States.Chase, chaseTransitions);
        _transitions.Add(States.Returning, returnTransitions);
        _transitions.Add(States.BeeingAbsorved, beeingAbsorvedTranstions);

        //Initial State
        currentState = States.Idle;
        Idle();

        //Fairy Movement Initialization
        _fm = GetComponent<FairyMovement>();

        //Dardo Atack Initialization
        _atack = GetComponent<DardoAtack>();

        //Small Size Object Initialization
        _sso = GetComponent<SmallSizeObject>();


        //Rigid Body Initialization
        _rb = GetComponent<Rigidbody>();
    }

    void Start () {
        layerMask = (1 << terrainLayer);

        EventManager.AddEventListener(GameEvent.SMALLABSORVABLE_REACHED, goBackToPool);
        EventManager.AddEventListener(GameEvent.PLAYER_DIE, PlayerDie);
        EventManager.AddEventListener(GameEvent.DARDO_DIE, goBackToPool);
    }


    private void goBackToPool(object[] parameterContainer)
    {
        
        if((GameObject)parameterContainer[0] == gameObject)
        {
            Debug.Log("Dardo Destroy");
            if ((bool)parameterContainer[1])
            {
                BulletManager.instance.AddItemToBag(Items.DARDO);
            }
            //EnemyManager.instance.ReturnDardoToPool(this);
        }
    }

    void Execute() {
        CheckInputs();
        updates[currentState]();
        
	}

    #region Input Logic
    private void CheckInputs()
    {
        if(_target != null)
        {
            var rayDir = (_target.position - transform.position).normalized;
            var isInView = !Physics.Raycast(transform.position, rayDir, chaseDistance, layerMask);
            var distance = Mathf.Abs((_target.position - transform.position).magnitude);

            if (_sso.isBeeingAbsorved)
            {
                ProcessInput(Inputs.Atracted);
                _sso.isBeeingAbsorved = false;
            }
            else if (distance < viewDistance + _atack.chargingTolerance && isInView) ProcessInput(Inputs.Atack);
            else if (distance < chaseDistance && !_atack.isCharging && isInView) ProcessInput(Inputs.NotInView);
            else ProcessInput(Inputs.OutOfRange);
        } 
    }


    private void ProcessInput(Inputs input)
    {
        var currentStateTransitions = _transitions[currentState];

        if (currentStateTransitions.ContainsKey(input) && currentState != currentStateTransitions[input])
        {
            currentState = currentStateTransitions[input];
            enters[currentState]();
        }
    }
    #endregion

    private void PlayerDie(object[] parameterContainer)
    {
        ProcessInput(Inputs.OutOfRange);
    }

    public void SetTarget(Transform tr)
    {
        _target = tr;
    }

    #region Updates
    void Idle()
    {
        //if(_rb != null)_rb.isKinematic = true;
    }

    void Atack()
    {
        _atack.Execute();
        //_rb.isKinematic = true;
    }

    void Chase()
    {
        //_rb.isKinematic = true;
    }

    void Return()
    {
        //_rb.isKinematic = true;
        var distance = _fm.originalPosition - transform.position;
        var dir = (distance).normalized;

        if (distance.magnitude < 1f) ProcessInput(Inputs.Idle);


    }

    private void TryToScape()
    {
        
    }
    #endregion

    #region Enters
    void IdleEnter()
    {
        _fm.DeActivate();
        _fm.Activate();

    }

    void AtackEnter()
    {
        _fm.DeActivate();
        _atack.Enter(_target);
    }

    void ChaseEnter()
    {
        _fm.DeActivate();
        _atack.chargingTolerance = 0f;
        _fm.ActivateDirectionedMove(_target, chaseDistanceToTravel, chaseWait);
    }

    void ReturnEnter()
    {
        _fm.DeActivate();
        _fm.ActivateDirectionedMove(_fm.originalPosition, returnDistanceToTravel, returnWait);
    }

    private void TryToScapeEnter()
    {
        _fm.DeActivate();

        #region opositePoint calculation

        var rawDir = (transform.position - _target.position);
        var dir = rawDir.normalized;
        var distance = Mathf.Abs(rawDir.magnitude);
        var opositePoint = transform.position + dir * distance;

        #endregion
        if(gameObject.activeInHierarchy)
            _fm.ActivateDirectionedMove(opositePoint, runDistanceToTravel,runWait);
    }
    #endregion

    #region Pool statics
    public static void InitializeDardo(DardoFSM dardoObj)
    {
        dardoObj.gameObject.SetActive(true);
        dardoObj.GetComponent<SmallSizeObject>().isAbsorved = false;
        var dardoFairymove = dardoObj.GetComponent<FairyMovement>();
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, dardoFairymove.Execute);
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, dardoObj.Execute);
    }

    public static void DestroyDardo(DardoFSM dardoObj)
    {
        dardoObj.gameObject.SetActive(false);
        var dardoFairymove = dardoObj.GetComponent<FairyMovement>();
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, dardoFairymove.Execute);
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, dardoObj.Execute);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.name == "Escupitajo")
        {
            EventManager.DispatchEvent(GameEvent.DARDO_DIE, gameObject, false);
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Shield")
        {
            EventManager.DispatchEvent(GameEvent.DARDO_DIE, gameObject, false);
        }
    }
}
