using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour {

    public Weight pesimeter;
    private Animator _animator;

    void OnEnable() {
        _animator = GetComponent<Animator>();
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
