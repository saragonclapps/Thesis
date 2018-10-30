using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckPoint : MonoBehaviour {

    public string checkPointName;

    private void Start()
    {
        LevelManager.instance.AddCheckPointToList(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            LevelManager.instance.SetActiveCheckPoint(checkPointName);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 200, 0, 0.7f); ;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
