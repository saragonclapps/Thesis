using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StumpBase : MonoBehaviour, IFlamableObjects {

    bool _isOnFire;
    public float life;
    public float burnSpeed;
    Renderer rend;

    public bool isBurned;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

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
        rend.material.color = new Color(1 - life / 1000, 0, 0);
    }



}
