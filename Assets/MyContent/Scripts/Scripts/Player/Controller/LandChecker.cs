using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = Logger.Debug;

public class LandChecker : MonoBehaviour {

    public bool land;
    public float rayLength = 0.1f;
    private bool _beforeLand;
    private const float COOL_DOWN_TO_CHECK = 3f;

    private void Start() {
        StartCoroutine(SecurityCheck());
    }

    private IEnumerator SecurityCheck() {
        while (true) {
            if (land) {
                _beforeLand = land;
                yield return new WaitForEndOfFrame();
                continue;
            }
            if (_beforeLand != land) {
                _beforeLand = land;
                yield return new WaitForSeconds(COOL_DOWN_TO_CHECK);
                continue;
            }
            // Security check
            if (!IsGrounded()) {
                yield return new WaitForSeconds(COOL_DOWN_TO_CHECK);
                continue;
            }
            land = true;
        }
    }

    private bool IsGrounded() {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, -Vector3.up, out hit, rayLength)) return false;
        return hit.collider.gameObject.layer != 10;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 10) return;
#if UNITY_EDITOR
        Debug.LogColor(this,"ENTER: " + DebugTerrain(other) , "red");
#endif
        land = true;
    }

#if UNITY_EDITOR
    private string DebugTerrain(Collider other) {
        if (other.gameObject.name.IndexOf("collider", StringComparison.Ordinal) != -1) {
            return other.gameObject.name;
        }
        
        return other.gameObject.transform.parent.name;
    }    
#endif

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == 10) return;
        land = true;
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 10) return;
#if UNITY_EDITOR
        Debug.LogColor(this,"EXIT: " + DebugTerrain(other), "red");
#endif
        land = false;

    }

    private void OnDrawGizmos() {
        Color rayColor = land ? Color.green : Color.red; // Verde si está en el suelo, rojo si está en el aire.
        Gizmos.color = rayColor;
        Gizmos.DrawRay(transform.position, -Vector3.up * rayLength);
        return;
        var collider = GetComponent<BoxCollider>();
        Gizmos.color = new Color(0, 0,200, 0.7f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(collider.center, collider.size);
    }
}
