
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class FencePuzzleAbsorver : MediumSizeObject, IVacuumObject
{
    private Collider _collider;

    private new void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        _collider.material.dynamicFriction = 0.6f;
        base.Start();
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }

    private void Execute()
    {
        // _collider.material.dynamicFriction = _isBeeingAbsorved ? 0.0f : 0.6f;
    }

    public new void BlowUp(Transform origin, float atractForce, Vector3 direction)
    {
        var distance = (transform.position - origin.position).magnitude;
        var forceMagnitude = rb.mass * atractForce * 10 / Mathf.Pow(distance, 2);
        forceMagnitude = Mathf.Clamp(forceMagnitude, 0, 2000);
        var force = direction.normalized * forceMagnitude;
        rb.AddForce(force);
    }

    public new void SuckIn(Transform origin, float atractForce)
    {
        var direction = origin.position - transform.position;
        var distance = direction.magnitude;
        var forceMagnitude = (rb.mass) * atractForce / distance;
        var force = direction.normalized * forceMagnitude;
        rb.AddForce(force);
    }

    private new void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
        base.OnDestroy();
    }
    public new void Exit(){}
    public new void ViewFX(bool activate){}

}
