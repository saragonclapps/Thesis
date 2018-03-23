using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPause : MonoBehaviour {

    public string animationName;

	public void OnAnimationEnd()
    {
        var anim = GetComponent<Animation>();
        anim[animationName].speed = 0;
    }
}
