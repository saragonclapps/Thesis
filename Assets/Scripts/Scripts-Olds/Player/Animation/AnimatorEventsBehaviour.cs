using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventsBehaviour : MonoBehaviour {

    private Animator _anim;
    public bool landEnd;

	void Start () {
        landEnd = true;
        _anim = GetComponent<Animator>();
        EventManager.AddEventListener(GameEvent.DARDO_HIT, SetDamageAnimation);
	}
    private void SetDamageAnimation(object[] pC)
    {
        var target = (Vector3)pC[0];
        var pos = (Vector3)pC[1];

        var dir = (target - pos).normalized;

        var rotation = transform.rotation.eulerAngles.y;

        var angleCos = Mathf.Cos(rotation * Mathf.PI / 180);
        var angleSin = Mathf.Sin(rotation * Mathf.PI / 180);

        var rotatedX = dir.x * angleCos + dir.z * angleSin;
        var rotatedY = -dir.x * angleSin + dir.z * angleCos;

        _anim.SetFloat("hitX", rotatedX);
        _anim.SetFloat("hitY", rotatedY);
        _anim.SetTrigger("hitHead");
        if(!(GameInput.instance.crouchButton || GameInput.instance.sprintButton))
            _anim.SetTrigger("getOut");
    }

    public void LandEnd()
    {
        landEnd = true;
        _anim.SetBool("toLand", false);
    }


}
