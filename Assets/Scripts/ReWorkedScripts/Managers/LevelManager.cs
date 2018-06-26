using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skills;
using System;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public bool isWithTimmer;
    public float levelTime;
    public bool isWithPowers;

    public Animator blackOutAnimator;
    public Animator whiteOutAnimator;

    float _timmer;
    float _tick;

    //For power Configurations
    /* (later)
    public SkillManager skillManager;
    */
	
	void Start ()
    {
        if (isWithTimmer)
        {
            _timmer = levelTime;
            HUDManager.instance.EnableTimmerHUD();
        }

        if (isWithPowers)
        {
            HUDManager.instance.EnablePowerHUD();
        }
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        EventManager.AddEventListener(GameEvent.TRANSITION_FADEOUT_FINISH, RestartLevel);
	}

    private void RestartLevel(object[] parameterContainer)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Execute ()
    {
        if (isWithTimmer)
        {
            if (_timmer > 0) _timmer -= Time.deltaTime;
            else
            {
                blackOutAnimator.SetTrigger("WhiteOut");
                UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
                HUDManager.instance.DisableTimmerHUD();
                HUDManager.instance.DisablePowerHUD();
            }
            HUDManager.instance.RefreshTimmerHUD(_timmer);
        }
	}
}
