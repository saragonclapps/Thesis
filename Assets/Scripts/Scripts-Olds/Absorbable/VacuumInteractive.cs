using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IVacuumObjects: MonoBehaviour {

    public bool isAbsorved;
    public bool isAbsorvable;
    public bool isBeeingAbsorved;
    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public abstract void SuckIn(Transform origin, float atractForce);
    public abstract void BlowUp(Transform origin, float atractForce, Vector3 direction);
    public abstract void ReachedVacuum();
    public abstract void Shoot(float shootForce, Vector3 direction);
    public abstract void ViewFX(bool active);
    public abstract void Exit();
}
