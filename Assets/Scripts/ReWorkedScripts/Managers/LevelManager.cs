using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skills;
using System;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    bool isWithTimmer = false;
    public float levelTime;
    public bool isWithPowers;

    public Animator blackOutAnimator;
    public Animator whiteOutAnimator;

    float _timmer;
    float _tick;

    bool _hasDiskette;
    public bool hasDiskette { get { return _hasDiskette; } set { _hasDiskette = value; } }

    public List<Material> breathingScenarioMaterials;

    static LevelManager _instance;
    public static LevelManager instance { get{ return _instance; } }

    //For power Configurations
    /* (later)
    public SkillManager skillManager;
    */
    void Awake()
    {
        _instance = this;
        breathingScenarioMaterials = new List<Material>();
    }

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
        EventManager.AddEventListener(GameEvent.TRANSITION_FADEOUT_LOSE_FINISH, RestartLevel);
        EventManager.AddEventListener(GameEvent.TRANSITION_FADEOUT_WIN_FINISH, NextLevel);
    }

    public void PreviousLevel()
    {
        MasterManager.GetPreviousScene(SceneManager.GetActiveScene());
        SceneManager.LoadScene("LoadingScreen");
    }

    public void NextLevel(object[] parameterContainer)
    {
        EventManager.RemoveEventListener(GameEvent.TRANSITION_FADEOUT_WIN_FINISH, NextLevel);
        var current = SceneManager.GetActiveScene();
        MasterManager.GetNextScene(current);
        //MasterManager.nextScene = next;

        SceneManager.LoadScene("LoadingScreen");
    }

    public void RestartLevel(object[] parameterContainer)
    {
        EventManager.RemoveEventListener(GameEvent.TRANSITION_FADEOUT_LOSE_FINISH, RestartLevel);
        var aux = SceneManager.GetActiveScene();
        SceneManager.LoadScene("LoadingScreen");
    }


    void Execute ()
    {
        if (isWithTimmer)
        {
            if (_timmer > 0) _timmer -= Time.deltaTime;
            else
            {
                blackOutAnimator.SetTrigger("FadeOutLose");
                UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
                HUDManager.instance.DisableTimmerHUD();
                HUDManager.instance.DisablePowerHUD();
            }
            HUDManager.instance.RefreshTimmerHUD(_timmer);
        }
	}

    public void AddBreathingMaterial(Material mat)
    {
        breathingScenarioMaterials.Add(mat);   
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
