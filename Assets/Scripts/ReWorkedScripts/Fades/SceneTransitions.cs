using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitions : MonoBehaviour
{

    public void OnFadeInFinish()
    {
        EventManager.DispatchEvent(GameEvent.TRANSITION_FADEIN_FINISH);
    }
    public void OnFadeOutFinish()
    {
        EventManager.DispatchEvent(GameEvent.TRANSITION_FADEOUT_FINISH);
    }
}
