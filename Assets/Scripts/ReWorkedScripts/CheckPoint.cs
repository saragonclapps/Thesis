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
}
