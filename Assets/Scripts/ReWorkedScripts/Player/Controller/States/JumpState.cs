using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPCamera;

namespace Player
{
    public class JumpState : IState<Inputs>
    {

        public Dictionary<Inputs, IState<Inputs>> _transitions;

        private Collider _col;
        private Rigidbody _rb;
        //private FallState _fall;
        private LandChecker _lc;

        float _jumpSpeed;
        float _jumpForce;

        //Fall multiplier Variables
        public float fallMultiplier = 2.5f;
        public float lowJumpMultiplier = 2f;

        private float _horizontal;
        private float _vertical;


        [HideInInspector]
        public bool forwardJump;

        //CameraController _cam;
        CameraFSM _cam;
        PlayerController _pC2;
        AnimatorEventsBehaviour _aES;
        //private CharacterMove _cm;

        Transform transform;
        Animator _anim;

        public JumpState(Rigidbody rb, CameraFSM cam, PlayerController pC2, LandChecker lc, AnimatorEventsBehaviour aES, Transform t, Animator anim, float jumpForce, float jumpSpeed)
        {
            _rb = rb;
            _cam = cam;
            _pC2 = pC2;
            _lc = lc;
            //_fall = GetComponent<FallState>();
            _aES = aES;
            //_cm = GetComponent<CharacterMove>();
            transform = t;
            _anim = anim;

            _jumpForce = jumpForce;
            _jumpSpeed = jumpSpeed;
        }

        public void Enter()
        {
            //_cm.SetColliderDimensions(0);
            //Apply jump force
            _rb.velocity = Vector3.up * _jumpForce;
            forwardJump = false;

            //If jumping in place or forward
            /*_horizontal = GameInput.instance.horizontalMove;
            _vertical = GameInput.instance.verticalMove;
            forwardJump = (Mathf.Abs(_horizontal) > 0.1f || Mathf.Abs(_vertical) > 0.1f) && !_pC2.CheckForwardCollision(GetCorrectedForward(), true);*/

            forwardJump = _pC2.Fsm.Last == typeof(MoveState);
            _pC2.jumpForward = forwardJump;
            //Set Animator Parameter

            _anim.SetBool("toJump", true);
            
            _aES.landEnd = false;

            //_cam.ChangeDistance(3f);
            _cam.normalState.unadjustedDistance = 3f;
            //_cam.ChangeSmoothness(0.1f);
            _cam.normalState.positionSmoothness = 0.1f;
            _pC2.isSkillLocked = true;
        }

        public void Execute()
        {
            if (_rb.velocity.y <= 0) _pC2.land = _lc.land;


            if (forwardJump)
            {
                transform.position += transform.forward * _jumpSpeed * Time.deltaTime;
            }

            if (Mathf.Abs(GameInput.instance.horizontalMove) > 0.1f || Mathf.Abs(GameInput.instance.verticalMove) > 0.1f)
            {
                var newDirection = GetCorrectedForward();

                //AirMove
                transform.position += newDirection * Time.deltaTime * _jumpSpeed / 2;
            }

            
            if (!GameInput.instance.jumpButton)
            {
                //Speed up fall
                _rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

        public void Exit()
        {
            _anim.SetBool("toJump", false);
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
