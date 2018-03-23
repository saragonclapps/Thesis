using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public bool solved;

    public int ID;

    bool matches;
    Rigidbody _rb;

	void Start ()
    {
        _rb = GetComponent<Rigidbody>();
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	void Execute ()
    {
        solved = matches && _rb.IsSleeping();
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 18)
        {
            var touched = collision.collider.GetComponent<KeyHole>();
            if (touched != null)
            {
                if (touched.ID == ID)
                {
                    matches = true;
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.gameObject.layer == 18)
        {
            var touched = collision.collider.GetComponent<KeyHole>();
            if (touched != null)
            {
                if (touched.ID == ID)
                {
                    matches = true;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
            matches = false;
    }
}
