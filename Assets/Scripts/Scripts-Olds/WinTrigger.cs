using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour {

    public Image win;

    private void Start()
    {
        win.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            win.enabled = true;
            UpdatesManager.instance.StopUpdates();
        }
    }
}
