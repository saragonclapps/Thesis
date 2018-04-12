using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class IdleState : IState<Inputs>
    {
        public Dictionary<Inputs, IState<Inputs>> _transitions;

        PlayerController2 _pC;
        Animator _anim;
        Transform _mainCamera;
        Transform transform;

        public IdleState(PlayerController2 pC, Animator anim, Transform mainCamera, Transform t)
        {
            _pC = pC;
            _anim = anim;
            _mainCamera = mainCamera;
            transform = t;
        }

        public void Enter()
        {
            _anim.SetBool("toJump", false);
            _anim.SetBool("stealth", false);
        }

        public void Execute()
        {
            //_pC.isMoving = Mathf.Abs(GameInput.instance.horizontalMove) > 0.1f || Mathf.Abs(GameInput.instance.verticalMove) > 0.1f;
            if (GameInput.instance.absorbButton || GameInput.instance.blowUpButton)
            {
                var camYRotation = Quaternion.Euler(0, _mainCamera.eulerAngles.y, 0);
                transform.rotation = camYRotation;
            }
        }

        public void Exit()
        {
            //_pC.isMoving = true;
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
    }

}
