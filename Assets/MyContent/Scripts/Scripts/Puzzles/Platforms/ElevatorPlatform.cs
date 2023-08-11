using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlatform : Platform, IVacuumObject {
    private bool _goUp;

    public float speed;
    public float lerpValue;
    private float _currentSpeed;

    public float minY;
    public float maxY;

    public float maxYOffset;

    public float gravityValue;

    #region VacuumObject

    private bool _isAbsorved;
    private bool _isAbsorvable;
    private bool _isBeeingAbsorved;

    public bool isAbsorved {
        get { return _isAbsorved; }
        set { _isAbsorved = value; }
    }

    public bool isAbsorvable {
        get { return _isAbsorvable; }
    }

    public bool isBeeingAbsorved {
        get { return _isBeeingAbsorved; }
        set { _isBeeingAbsorved = value; }
    }

    public Rigidbody rb { get; set; }

    public Collider collider { get; set; }

    public float currentSpeed {
        get { return _currentSpeed; }
        set { _currentSpeed = value; }
    }

    public void SuckIn(Transform origin, float atractForce) {
    }

    public void BlowUp(Transform origin, float atractForce, Vector3 direction) {
        if (origin.position.y > transform.position.y) {
            ActivateElevate();
        }
    }

    public void Exit() {
    }

    public void ReachedVacuum() {
    }

    public void Shoot(float shootForce, Vector3 direction) {
    }

    public void ViewFX(bool active) {
    }

    public void ActivateElevate() {
        _goUp = true;
    }

    #endregion


    private void Start() {
        rb = GetComponent<Rigidbody>();
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }

    private void Execute() {
        if (_goUp) {
            if (transform.position.y <= maxY - maxYOffset)
                _currentSpeed = Mathf.Lerp(_currentSpeed, speed, lerpValue);
            else if (transform.position.y <= maxY)
                _currentSpeed = Mathf.Lerp(_currentSpeed, gravityValue, lerpValue);
        }
        else {
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0, lerpValue);
        }

        if (transform.position.y >= minY && !_goUp) {
            transform.position -= transform.up * gravityValue * Time.deltaTime;
        }

        if (transform.position.y <= maxY) {
            transform.position += transform.up * Time.deltaTime * _currentSpeed;
        }

        rb.isKinematic = true;
        _goUp = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, minY, transform.position.z), 0.5f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, maxY, transform.position.z), 0.5f);
    }

    private void OnDestroy() {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}