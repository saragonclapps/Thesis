using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(CharacterMove))]
[RequireComponent(typeof(Jumper))]
[RequireComponent(typeof(FallState))]
[RequireComponent(typeof(Aiming))]
public class PlayerController : MonoBehaviour {

    #region FSM
    public enum States { Idle, Move, StealthIdle, StealthMove, Locked, Jumping, Falling, Landing}
    public enum Inputs { Move, Stealth, Aiming, Idle, NotAiming, Unstealth, Jump, Land, Fall}

    private Dictionary<States, Action> updates;
    private Dictionary<States, Action> enters;
    private Dictionary<States, Dictionary<Inputs, States>> _transitions;

    public States currentState;
    #endregion


    //State Scripts
    private CharacterMove _movement;
    private Jumper _jump;
    private FallState _fall;
    private Aiming _aim;
    private LandChecker _lc;
    

    //Animation Controller
    [HideInInspector]
    public Animator anim;


    //Inputs Bool
    private bool _isMoving;

    [HideInInspector]
    public bool land;

    [HideInInspector]
    public bool isAiming;

    private int fallCount;

    //Script References
    private Rigidbody _rb;

    //Main camera Reference
    private Transform _mainCamera;
    public float collisionDistance;
    public LayerMask lm;


    //Stealth State
    public bool isStealth;

    //DeathTrigger (Placeholder)
    public Vector3 initialPosition;

    //Stats
    public float life = 100;

    void Awake()
    {
        //Initialize FSM

        //Updates Dictionary
        updates = new Dictionary<States, Action>();
        updates.Add(States.Idle, Idle);
        updates.Add(States.Move, Move);
        updates.Add(States.StealthIdle, StealthIdle);
        updates.Add(States.StealthMove, Stealth);
        updates.Add(States.Locked, Aiming);
        updates.Add(States.Jumping, Jumping);
        updates.Add(States.Falling, Falling);

        //Enters Dictionary
        enters = new Dictionary<States, Action>();
        enters.Add(States.Idle, IdleEnter);
        enters.Add(States.Move, MoveEnter);
        enters.Add(States.StealthIdle, StealthIdleEnter);
        enters.Add(States.StealthMove, StealthMoveEnter);
        enters.Add(States.Locked, AimingEnter);
        enters.Add(States.Jumping, JumpEnter);
        enters.Add(States.Falling, FallEnter);

        //Transitions

        //IdleTransitions
        var idleTransitions = new Dictionary<Inputs, States>();
        idleTransitions.Add(Inputs.Move, States.Move);
        idleTransitions.Add(Inputs.Stealth, States.StealthIdle);
        idleTransitions.Add(Inputs.Aiming, States.Locked);
        idleTransitions.Add(Inputs.Jump, States.Jumping);
        idleTransitions.Add(Inputs.Fall, States.Falling);

        //MoveTransitions
        var moveTransitions = new Dictionary<Inputs, States>();
        moveTransitions.Add(Inputs.Idle, States.Idle);
        moveTransitions.Add(Inputs.Aiming, States.Locked);
        moveTransitions.Add(Inputs.Stealth, States.StealthMove);
        moveTransitions.Add(Inputs.Jump, States.Jumping);
        moveTransitions.Add(Inputs.Fall, States.Falling);

        //LockedTransitions
        var aimingTransitions = new Dictionary<Inputs, States>();
        aimingTransitions.Add(Inputs.NotAiming, States.Idle);
        //aimingTransitions.Add(Inputs.Fall, States.Falling);


        //StealthIdleTransitions
        var stealthIdleTransitions = new Dictionary<Inputs, States>();
        stealthIdleTransitions.Add(Inputs.Move, States.StealthMove);
        stealthIdleTransitions.Add(Inputs.Unstealth, States.Idle);

        //StealthMoveTransitions
        var stealthMoveTransitions = new Dictionary<Inputs, States>();
        stealthMoveTransitions.Add(Inputs.Idle, States.StealthIdle);
        stealthMoveTransitions.Add(Inputs.Unstealth, States.Move);
        stealthMoveTransitions.Add(Inputs.Fall, States.Falling);

        //JumpTransitions
        var jumpTransitions = new Dictionary<Inputs, States>();
        jumpTransitions.Add(Inputs.Land, States.Idle);
        jumpTransitions.Add(Inputs.Fall, States.Falling);

        //Fall Transitions
        var fallTransitions = new Dictionary<Inputs, States>();
        fallTransitions.Add(Inputs.Land, States.Idle);

        _transitions = new Dictionary<States, Dictionary<Inputs, States>>();
        _transitions.Add(States.Idle, idleTransitions);
        _transitions.Add(States.Locked, aimingTransitions);
        _transitions.Add(States.Move, moveTransitions);
        _transitions.Add(States.StealthIdle, stealthIdleTransitions);
        _transitions.Add(States.StealthMove, stealthMoveTransitions);
        _transitions.Add(States.Jumping, jumpTransitions);
        _transitions.Add(States.Falling, fallTransitions);

        

    }

    void Start () {
        //Initialize Movement
        _movement = GetComponent<CharacterMove>();
        //Initialize Jumper
        _jump = GetComponent<Jumper>();
        //Initialize Fall
        _fall = GetComponent<FallState>();
        //Initialize Aim
        _aim = GetComponent<Aiming>();

        //Initialize RigidBody
        _rb = GetComponent<Rigidbody>();

        //Initialize PlayerAnimationController
        anim = GetComponentInChildren<Animator>();

        //Initialize LandChecker
        _lc = GetComponentInChildren<LandChecker>();


        _mainCamera = _movement.cam.transform;
        //Initial FSM State
        currentState = States.Idle;
        IdleEnter();
        Idle();
        
        _isMoving = false;

        //Was Fixed
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);

        //DeathTrigger
        initialPosition = transform.position;

        //Event Daño
        EventManager.AddEventListener(GameEvent.PLAYER_TAKE_DAMAGE, TakeDamage);
        EventManager.AddEventListener(GameEvent.TRANSITION_DEATH_END, RepositionPlayer);

        fallCount = 0;
    }

    void Execute() {
		CheckInputs();
        updates[currentState]();
	}

    /// <summary>
    /// Check for new Inputs to translate to the FSM
    /// </summary>
    private void CheckInputs()
    {
        if (GameInput.instance.aimButton)
        {
            if(_aim.isAiming)
            {
                _aim.Exit();
                ProcessInput(Inputs.NotAiming);
                isAiming = false; //TODO: Ponerlo en el exit de aiming
            }
            else
            {
                ProcessInput(Inputs.Aiming);
                isAiming = true; //TODO: Ponerlo en el enter de aiming
            }
        }
        if (_isMoving)
        {
            ProcessInput(Inputs.Move);
        }
        else ProcessInput(Inputs.Idle);

        if (GameInput.instance.initialJumpButton && !land) ProcessInput(Inputs.Jump);

        if (land)
        {
            ProcessInput(Inputs.Land);
            land = false;
            anim.SetBool("toLand", true);
            anim.SetFloat("velocityY", 0);
        }

        //Triple check for fall state
        if (_rb.velocity.y < -0.2f && !_lc.land) fallCount++;
        else fallCount = 0;
        if(fallCount >= 2) ProcessInput(Inputs.Fall);


        if (GameInput.instance.crouchButton) ProcessInput(Inputs.Stealth);
        else ProcessInput(Inputs.Unstealth);
    }

    /// <summary>
    /// Procces the input given
    /// </summary>
    /// <param name="input"> Inputs enum: for FSM transitions </param>
    void ProcessInput(Inputs input)
    {
        var currentStateTransitions = _transitions[currentState];

        if (currentStateTransitions.ContainsKey(input) && currentState != currentStateTransitions[input])
        {
            currentState = currentStateTransitions[input];
            enters[currentState]();
        }
    }

    /// <summary>
    /// States Updates (execute when the fsm is in this state)
    /// </summary>
    #region Updates
    void Move()
    {
        _isMoving = Math.Abs(GameInput.instance.horizontalMove) > 0.1f || Math.Abs(GameInput.instance.verticalMove) > 0.1f;
        _movement.Execute();
    }

    void Idle()
    {
        _isMoving = Math.Abs(GameInput.instance.horizontalMove) > 0.1f || Math.Abs(GameInput.instance.verticalMove) > 0.1f;
        if(GameInput.instance.absorbButton || GameInput.instance.blowUpButton)
        {
            var camYRotation = Quaternion.Euler(0,_mainCamera.eulerAngles.y, 0);
            //transform.rotation = Quaternion.Slerp(transform.rotation, camYRotation, 0.2f);
            transform.rotation = camYRotation;
        }
    }

    void StealthIdle()
    {
        _isMoving = Math.Abs(GameInput.instance.horizontalMove) > 0.1f || Math.Abs(GameInput.instance.verticalMove) > 0.1f;
    }

    void Stealth()
    {
        _isMoving = Math.Abs(GameInput.instance.horizontalMove) > 0.1f || Math.Abs(GameInput.instance.verticalMove) > 0.1f;
        _movement.Execute();

    }

    void Aiming()
    {
        _aim.Execute();
    }

    void Jumping()
    {
        _jump.Execute();
    }

    void Falling()
    {
        _fall.Execute();
    }
    
    #endregion

    /// <summary>
    /// States Enters (execute when the fsm enters this state)
    /// </summary>
    #region Enters
    void IdleEnter()
    {

        isStealth = true;
        anim.SetBool("toJump", false);
        anim.SetBool("stealth", false);
        _movement.Exit();
        _rb.velocity = Vector3.zero;
    }
    void MoveEnter()
    {

        isStealth = false;
         anim.SetBool("stealth", false);
        _movement.Enter(false);
    }
    void StealthIdleEnter()
    {

        isStealth = true;
        anim.SetBool("stealth", true);
        _movement.Exit();
    }
    void StealthMoveEnter()
    {

        isStealth = true;
        anim.SetBool("stealth", true);
        _movement.Enter(true);
    }
    void AimingEnter()
    {
        isStealth = false;
        _aim.Enter();
        
    }
    void JumpEnter()
    {
        isStealth = false;
        anim.SetBool("toLand", false);
        _jump.Enter();
        _movement.Exit();

    }
    void FallEnter()
    {
        isStealth = false;
        _jump.Exit();
        _fall.Enter();
    }
    #endregion

    void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }

    /// <summary>
    /// Checks forward in order to see if the player can move or not
    /// </summary>
    /// <returns></returns>
    public bool CheckForwardCollision(Vector3 moveDirection, bool forward)
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward);
        return (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, collisionDistance,lm) ||
               Physics.Raycast(transform.position + new Vector3(0,0.3f,0), transform.forward, collisionDistance, lm) ||//Edit
               Physics.Raycast(transform.position + new Vector3(0, 1.5f, 0), transform.forward, collisionDistance, lm)) && forward ||
               Physics.Raycast(transform.position + new Vector3(0, 1, 0), moveDirection, collisionDistance, lm) ||
               Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), moveDirection, collisionDistance, lm) ||
               Physics.Raycast(transform.position + new Vector3(0, 1.5f, 0), moveDirection, collisionDistance, lm);
    }


    //Death Trigger
    private void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.layer == 13)
        {
            Die();
        }
    }

    private void TakeDamage(object[] pC)
    {
        var damage = (float)pC[0];
        life -= damage;
        if(life <= 0)
        {
            Die();
            life = 100;
        }
        if(life > 100)
        {
            life = 100;
        }
    }
    //Die Function PlaceHolder
    void Die()
    {
        EventManager.DispatchEvent(GameEvent.PLAYER_DIE);
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }

    private void RepositionPlayer(object[] parameterContainer)
    {
        transform.position = initialPosition;
        _rb.velocity = Vector3.zero;
        land = true;
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }
}
