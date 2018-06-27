using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPanel : MonoBehaviour {

    public LevelManager levelManager;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9 && HUDManager.instance.saveDisk)
        {
            levelManager.whiteOutAnimator.SetTrigger("WhiteOut");
        }
    }

}
