using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    protected bool _isActive;
    public bool relateParent;

    protected bool hasHero;

    public bool isActive { get { return _isActive; } set { _isActive = value; } }

    void OnCollisionEnter(Collision c)
    {
        if (c.collider.gameObject.layer == 9 && relateParent)
        {
            c.transform.SetParent(transform);
            hasHero = true;
        }
    }

    void OnCollisionStay(Collision c)
    {
        if(c.collider.gameObject.layer == 9 && relateParent && c.transform.parent != transform)
        {
            c.transform.SetParent(transform);
            hasHero = true;
        }
    }

    void OnCollisionExit(Collision c)
    {
        if (c.collider.gameObject.layer == 9 && relateParent)
        {
            c.transform.SetParent(null);
            hasHero = false;
        }
    }

}
