﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckPoint : MonoBehaviour {
    public string checkPointName;
    public Transform cameraPosition;
    public static bool drawGizmos = true;

    private void Start() {
        LevelManager.instance.AddCheckPointToList(this);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer != 9) return;
        LevelManager.instance.SetActiveCheckPoint(checkPointName);
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer != 9) return;
        LevelManager.instance.SetActiveCheckPoint(checkPointName);
    }

    private void OnDrawGizmos() {
        if (!drawGizmos) return;
        var collider = GetComponent<BoxCollider>();
        Gizmos.color = new Color(0, 200, 0, 0.7f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(collider.center, collider.size);
    }
}