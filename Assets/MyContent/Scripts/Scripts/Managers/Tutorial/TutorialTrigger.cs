using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TutorialTrigger : MonoBehaviour {
    public string tutorialName;
    public bool onStart;
    public bool isOneShot;
    private bool _isTriggered;

    private void Start() {
        if (onStart) {
            EventManager.DispatchEvent(GameEvent.TRIGGER_TUTORIAL, tutorialName);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer != 9 || _isTriggered) return;
        if (isOneShot) {
            _isTriggered = true;
        }
        EventManager.DispatchEvent(GameEvent.TRIGGER_TUTORIAL, tutorialName);
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer != 9) return;
        EventManager.DispatchEvent(GameEvent.TRIGGER_TUTORIAL_STOP, tutorialName);
    }

    private void OnDrawGizmos() {
        var collider = GetComponent<BoxCollider>();
        Gizmos.color = new Color(255, 240, 0, 0.7f);
        
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(collider.center, collider.size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(collider.center, collider.size);
    }
}