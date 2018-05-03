using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class LandState : IState<Inputs>
    {

        Dictionary<Inputs, IState<Inputs>> _transitions;
        Animator _anim;
        PlayerController2 _pC;
        AnimatorEventsBehaviour _aEB;
        CameraController _cam;

        public LandState(Animator anim, PlayerController2 pC, AnimatorEventsBehaviour aEB, CameraController cam)
        {
            _anim = anim;
            _pC = pC;
            _aEB = aEB;
            _cam = cam;
        }

        public void Enter()
        {
            
            _anim.SetBool("toLand", true);
            _cam.ChangeDistance(2f);

        }

        public void Execute()
        {

            _anim.SetBool("toLand", true);
            
        }

        public void Exit()
        {
            /*if(_pC.CheckMove())
                _anim.SetFloat("speed", 1);*/

            _anim.SetBool("toLand", false);
            
            _aEB.LandEnd();

            _pC.isSkillLocked = false;
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }

    }
}
