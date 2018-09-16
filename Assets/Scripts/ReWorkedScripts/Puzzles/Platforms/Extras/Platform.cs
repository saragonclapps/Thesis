using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool isActive;
    public bool relateParent;
    void OnCollisionEnter(Collision c)
    {
        if (c.collider.gameObject.layer == 9 && relateParent)
        {
            c.transform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision c)
    {
        if (c.collider.gameObject.layer == 9 && relateParent)
        {
            c.transform.SetParent(null);
        }
    }

}
