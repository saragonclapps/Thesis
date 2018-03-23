using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    public LayerMask layerCollisionActive;
    public LayerMask layerCollisionStop;
    public float minForceFall = 200;
    Animation _animation;

    private bool _hasBeenExecute = false;
    private const string ANIMATION_FALL = "Stick Fall";
    private const string ANIMATION_ERROR_FALL = "Stick Error Fall";

    void Start ()
    {
        _animation = GetComponent<Animation>();
    }

    private void OnCollisionEnter(Collision c)
    {
        if (!_hasBeenExecute && layerCollisionActive.value == 1 << c.gameObject.layer)
        {
            var force = Vector3.Magnitude(c.impulse);//Force
            var direction = transform.up + c.relativeVelocity.normalized;//Direction force.

            print(force > minForceFall);
            print((direction.magnitude > transform.forward.magnitude));
            if (force > minForceFall && (direction.magnitude > transform.forward.magnitude ))
            {
                _hasBeenExecute = true;
                _animation.Play(ANIMATION_FALL);
            }
            else if (!_hasBeenExecute)
            {
                _animation.Rewind(ANIMATION_ERROR_FALL);
                _animation.Play(ANIMATION_ERROR_FALL);
            }

        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (_hasBeenExecute &&  layerCollisionStop.value == 1 << c.gameObject.layer)
        {
            print("stop-2");

            _animation.Stop();
        }
    }
}
