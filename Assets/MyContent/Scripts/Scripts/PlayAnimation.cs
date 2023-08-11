using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayAnimation : MonoBehaviour {

    private Animator _animator;
    public bool initalPlay = false;

    void OnEnable() {
        _animator = GetComponent<Animator>();
        // verify if the animator is playing and set the speed accordingly
        if (_animator.runtimeAnimatorController != null) {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime < 1) {
                _animator.SetFloat("speed", (initalPlay ? 1 : 0));
            }
        }

    }

    public void toggleAnim(bool b)
    {
        if (b)
        {
            _animator.SetFloat("speed", 1f);
        }
        else
        {
            _animator.SetFloat("speed", -1f);
        }
    }
}
