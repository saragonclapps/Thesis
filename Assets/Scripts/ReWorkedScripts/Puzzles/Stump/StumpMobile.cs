using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StumpMobile : MonoBehaviour {

    public StumpBase sBase;
    Animation anim;

    void Start()
    {
        anim = GetComponent<Animation>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (sBase.isBurned && 
            collision.gameObject.GetComponent<IVacuumObject>() != null)
        {
            anim.Play();
        }
    }
}
