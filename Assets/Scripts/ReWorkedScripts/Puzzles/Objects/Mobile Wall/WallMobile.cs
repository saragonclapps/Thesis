using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMobile : MonoBehaviour {

    public float speedModifier;
    Animator _anim;
	void Start ()
    {
        _anim = GetComponent<Animator>();
        _anim.SetFloat("speed", speedModifier);
	}
	
	
}
