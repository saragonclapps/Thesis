using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = Logger.Debug;

public class TrackerModel : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    private void FixedUpdate() {
        if (target == null) {
#if UNITY_EDITOR
            Debug.LogWarning(this, "No target set for tracker model");
#endif
            return;
        }
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
