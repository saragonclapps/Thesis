using System.Collections;
using System.Collections.Generic;
using Logger;
using UnityEngine;
using Debug = Logger.Debug;

public class HeatTransfer : MonoBehaviour
{
    private IFlamableObjects _fo;
#if UNITY_EDITOR
    public bool LogAllow = false;
#endif
    private void Start ()
    {
        _fo = GetComponent<IFlamableObjects>();
	}

    private void OnCollisionEnter(Collision collision) {
        HeatTransferTo(collision);
    }

    private void OnCollisionStay(Collision collision) {
        HeatTransferTo(collision);
    }
    
    private void OnCollisionExit(Collision collision) {
        HeatTransferTo(collision);
    }

    private void HeatTransferTo(Collision collision) {
#if UNITY_EDITOR
        if (LogAllow) {
            Debug.LogColor(this, "HeatTransfer: 1", LogColor.GREEN);
        }
#endif
        if (!_fo.isOnFire) return;
        if (collision.gameObject.layer != 12) return;
        var otherFo = collision.gameObject.GetComponent<IFlamableObjects>();
#if UNITY_EDITOR
        if (LogAllow) {
            Debug.LogColor(this, "HeatTransfer: 2" + otherFo, LogColor.YELLOW);
        }
#endif
        if (otherFo == null) return;

#if UNITY_EDITOR
        if (LogAllow) {
            Debug.LogColor(this, "HeatTransfer: " + otherFo + " is on fire", LogColor.RED);
        }
#endif

        otherFo.SetOnFire();
    }
}
