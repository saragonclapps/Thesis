using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandIKControl : MonoBehaviour {

    private Animator _anim;

    public bool ikActive = false;
    public Transform ikObject;
    public Transform lookObj;

	void Start ()
    {
        _anim = GetComponent<Animator>();
	}


    private void OnAnimatorIK(int layerIndex)
    {
        if (ikActive)
        {
            _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            _anim.SetIKPosition(AvatarIKGoal.LeftHand, ikObject.position);
            _anim.SetIKRotation(AvatarIKGoal.LeftHand, ikObject.rotation);
        }
        else
        {
            _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }

    }
}
