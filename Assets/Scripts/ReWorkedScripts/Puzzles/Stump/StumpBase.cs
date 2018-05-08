using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StumpBase : MonoBehaviour, IFlamableObjects {

    bool _isOnFire;
    public float life;
    public float burnSpeed;

    public bool isBurned;

    public bool isOnFire
    {
        get{ return _isOnFire; }
        set{ _isOnFire = value; }
    }

    public void SetOnFire()
    {
        _isOnFire = true;
        life -= Time.deltaTime * burnSpeed;
        if (life < 0)
        {
            isBurned = true;
        }
    }



}
