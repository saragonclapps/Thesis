using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class LandState : IState<Inputs>
    {

        Dictionary<Inputs, IState<Inputs>> _transitions;
        Animator[] _anim;
        PlayerController2 _pC;
        AnimatorEventsBehaviour _aEB;

        public LandState(Animator[] anim, PlayerController2 pC, AnimatorEventsBehaviour aEB)
        {
            _anim = anim;
            _pC = pC;
            _aEB = aEB;
        }

        public void Enter()
        {
            for (int i = 0; i < _anim.Length; i++)
            {
                _anim[i].SetBool("toLand", true);
            }
        }

        public void Execute()
        {
            for (int i = 0; i < _anim.Length; i++)
            {
                _anim[i].SetBool("toLand", true);
            }
        }

        public void Exit()
        {
            /*if(_pC.CheckMove())
                _anim.SetFloat("speed", 1);*/
            for (int i = 0; i < _anim.Length; i++)
            {
                _anim[i].SetBool("toLand", false);
            }
            _aEB.LandEnd();
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }

    }
}
