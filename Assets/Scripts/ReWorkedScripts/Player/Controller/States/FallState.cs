using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class FallState : IState<Inputs> {

        public Dictionary<Inputs, IState<Inputs>> _transitions;


        //Implementar lo que viene del JUMP!
       // private Jumper _jump;


        //Fall multiplier Variables
        float fallMultiplier = 2.5f;
        float lowJumpMultiplier = 2f;

        float _horizontal;
        float _vertical;


        //Camera Reference
        //Player controller Reference
        //private PlayerController _pc;


        Rigidbody _rb;
        LandChecker _lc;
        CameraFMS _cam;
        AnimatorEventsBehaviour _aES;
        Animator[] _anim;
        PlayerController2 _pC2;
        Transform transform;

        [HideInInspector]
        public bool land;

        private int landCount = 0;
        private float ypos;

        bool _isJumpingForward;
        float _jumpSpeed;

        public FallState(Rigidbody rb, PlayerController2 pC2, CameraFMS cam, LandChecker lc, AnimatorEventsBehaviour aES, Transform t, Animator[] anim, float jumpSpeed)
        {
            _cam = cam;
            _rb = rb;
            _pC2 = pC2;
            _lc = lc;
            _aES = aES;
            transform = t;
            _anim = anim;
            _jumpSpeed = jumpSpeed;
            /*_jump = GetComponent<Jumper>();
            _pc = GetComponent<PlayerController>();
            _cam = GetComponent<CharacterMove>().cam;
            //Initialize LandChecker 
            _lc = GetComponentInChildren<LandChecker>();
            _aES = GetComponentInChildren<AnimatorEventsBehaviour>();*/

        }

        public void Enter()
        {
            //isJumpingForward = _jump.forwardJump;
            land = false;
            //_aES.landEnd = false;
            landCount = 0;
            ypos = transform.position.y;
            _isJumpingForward = _pC2.jumpForward;
        }

        public void Execute()
        {
            _rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

            var ydif = Mathf.Abs(ypos - transform.position.y);
            var ydifComp = _rb.velocity.y * Time.deltaTime;
            if (ydif <= ydifComp) landCount++;
            else landCount = 0;

            ypos = transform.position.y;

            if (Mathf.Abs(_rb.velocity.y) <= 0.2) landCount++;
            else landCount = 0;

            if (landCount >= 2)
            {
                _pC2.land = true;
                landCount = 0;
            }
            else
            {
                _pC2.land = _lc.land;
            }
            if (!_pC2.CheckForwardCollision(GetCorrectedForward(), true))
            {
                if (_isJumpingForward)
                {
                    transform.position += transform.forward * _jumpSpeed * Time.deltaTime;
                }

                if (Mathf.Abs(GameInput.instance.horizontalMove) > 0.1f || Mathf.Abs(GameInput.instance.verticalMove) > 0.1f)
                {
                    var newDirection = GetCorrectedForward();

                    //AirMove
                    transform.position += newDirection * Time.deltaTime * _jumpSpeed / 2;
                }

            }
            for (int i = 0; i < _anim.Length; i++)
            {
                _anim[i].SetFloat("velocityY", _rb.velocity.y);

            }
        }

        public void Exit()
        {
            for (int i = 0; i < _anim.Length; i++)
            {
                _anim[i].SetBool("toLand", true);
                _anim[i].SetFloat("velocityY", 0);

            }
        }

        Vector3 GetCorrectedForward()
        {
            //Get inputs
            _horizontal = GameInput.instance.horizontalMove;
            _vertical = GameInput.instance.verticalMove;

            //Get cameraForward(2D floor plain)
            var camForwardWithOutY = new Vector3(_cam.transform.forward.x, 0, _cam.transform.forward.z);
            var anglesign = Vector3.Cross(new Vector3(0, 0, 1), camForwardWithOutY).y > 0 ? 1 : -1;
            var _angleCorrection = Vector3.Angle(new Vector3(0, 0, 1), camForwardWithOutY) * anglesign;

            //Get forward multiplying the input vector3 with the quaternion containing the camera angle
            return (Quaternion.Euler(0f, _angleCorrection, 0f) * new Vector3(_horizontal, 0, _vertical)).normalized;
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
    }

}
