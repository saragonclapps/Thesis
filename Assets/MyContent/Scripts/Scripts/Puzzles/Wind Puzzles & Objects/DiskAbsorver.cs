using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskAbsorver : MonoBehaviour, IVacuumObject {
    public bool isAbsorved { get; set; }

    public bool isAbsorvable { get; }
    public bool isBeeingAbsorved { get; set; }

    public Rigidbody rb { get; set; }
    public Collider collider { get; set; }

    private Vector3 _initialPosition;

    public float speedAttenuator;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        _initialPosition = transform.position;
    }

    private void Execute() {
        if (!isBeeingAbsorved) {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.5f);
            transform.position = Vector3.Lerp(transform.position, _initialPosition, 0.5f);
        }

        isBeeingAbsorved = false;
    }

    public void SuckIn(Transform origin, float attractForce) {
        isBeeingAbsorved = true;
        var dir = (transform.position - origin.position).normalized;
        var distance = Vector3.Distance(transform.position, origin.position);
        transform.position -= dir * attractForce / speedAttenuator * Time.deltaTime;
        if (Mathf.Abs(distance) < 1 && Mathf.Abs(distance) > 0.5f) {
            transform.localScale = Vector3.one * Mathf.Abs(distance);
        }
    }

    public void BlowUp(Transform origin, float atractForce, Vector3 direction) {
    }

    public void ReachedVacuum() {
    }

    public void Shoot(float shootForce, Vector3 direction) {
    }

    public void ViewFX(bool active) {
    }

    public void Exit() {
    }
}