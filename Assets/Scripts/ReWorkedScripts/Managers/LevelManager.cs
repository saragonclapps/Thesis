using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skills;
using System;
using UnityEngine.SceneManagement;
using Player;

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

    List<CheckPoint> checkPoints;
    //For power Configurations
    /* (later)
    public SkillManager skillManager;
    */

    PlayerController _PC;

    void Awake()
    {
        _instance = this;
        checkPoints = new List<CheckPoint>();
        breathingScenarioMaterials = new List<Material>();
        
    }

    void Start ()
    {
        _PC = FindObjectOfType<PlayerController>();
       
        foreach (var cp in checkPoints)
        {
            if(cp.checkPointName == MasterManager.checkPointName)
            {
                _PC.transform.position = cp.transform.position;
                _PC.transform.rotation = cp.transform.rotation;
            }
        }
        if (isWithPowers)
        {
            HUDManager.instance.EnablePowerHUD();
        }
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

    public void AddCheckPointToList(CheckPoint cp)
    {
        if (!checkPoints.Contains(cp))
        {
            checkPoints.Add(cp);
        }
    }

    public void SetActiveCheckPoint(string cpName)
    {
        bool isIncluded = false;
        foreach (var cp in checkPoints)
        {
            if (cp.checkPointName == cpName)
            {
                isIncluded = true;
                break;
            }
        }

        if (isIncluded)
        {
            MasterManager.checkPointName = cpName;
        }
        else
        {
            Debug.LogWarning("El checkpoint no se encuentra en la lista");
        }
    }

    public void AddBreathingMaterial(Material mat)
    {
        breathingScenarioMaterials.Add(mat);   
    }

    private void OnDestroy()
    {

    }
}
