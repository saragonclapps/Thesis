using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skills;

namespace Player
{
    public class IdleState : IState<Inputs>
    {
        public Dictionary<Inputs, IState<Inputs>> _transitions;

        PlayerController2 _pC;
        Animator _anim;
        Transform _mainCamera;
        Transform transform;
        CameraController _cam;
        SkillController _skill;

        public IdleState(PlayerController2 pC, Animator anim, Transform mainCamera, Transform t)
        {
            _pC = pC;
            _anim = anim;
            _mainCamera = mainCamera;
            transform = t;
            _cam = _mainCamera.GetComponent<CameraController>();
            _skill = _pC.GetComponentInChildren<SkillController>();
        }

        public void Enter()
        {
            _anim.SetBool("toJump", false);
            _cam.ChangeSmoothness(0.3f);
        }

        public void Execute()
        {
            _anim.SetBool("toIdle", true);
            
            if ((GameInput.instance.absorbButton && _skill.currentSkill == Skills.Skills.VACCUM )|| GameInput.instance.blowUpButton)
            {
                var camYRotation = Quaternion.Euler(0, _mainCamera.eulerAngles.y, 0);
                transform.rotation = camYRotation;
            }
        }

        public void Exit()
        {

        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
    }

}
