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

        public LandState(Animator anim, PlayerController2 pC)
        {
            _anim = anim;
            _pC = pC;
        }

        public void Enter()
        {

            _anim.SetBool("toLand", true); 

        }

        public void Execute()
        {
            
        }

        public void Exit()
        {
            if(_pC.CheckMove())
                _anim.SetFloat("speed", 1);
            _anim.SetBool("toLand", false);
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }

    }
}
