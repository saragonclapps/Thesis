using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skills;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using Player;
using Debug = Logger.Debug;

public class LevelManager : MonoBehaviour {
    public float levelTime;
    public bool isWithPowers;

    public Animator blackOutAnimator;
    public Animator whiteOutAnimator;


    bool _hasDiskette;

    public bool hasDiskette {
        get { return _hasDiskette; }
        set { _hasDiskette = value; }
    }

    public List<Material> breathingScenarioMaterials;

    static LevelManager _instance;

    public static LevelManager instance {
        get { return _instance; }
    }

    public List<CheckPoint> checkPoints;

    PlayerController _PC;

    private void Awake() {
        _instance = this;
        checkPoints = new List<CheckPoint>();
        breathingScenarioMaterials = new List<Material>();
    }

    private void Start() {
        _PC = FindObjectOfType<PlayerController>();

        foreach (var cp in checkPoints) {
            if (cp.checkPointName == MasterManager.checkPointName) {
                _PC.transform.position = cp.transform.position;
                _PC.transform.rotation = cp.transform.rotation;
            }
        }

        if (isWithPowers) {
            HUDManager.instance.EnablePowerHUD();
        }

        EventManager.AddEventListener(GameEvent.TRANSITION_FADEOUT_LOSE_FINISH, RestartLevel);
        EventManager.AddEventListener(GameEvent.TRANSITION_FADEOUT_WIN_FINISH, NextLevel);
    }

    public static void PreviousLevel() {
        MasterManager.GetPreviousScene(SceneManager.GetActiveScene());
        SceneManager.LoadScene("LoadingScreen");
    }

    public static void NextLevel(object[] parameterContainer) {
        EventManager.RemoveEventListener(GameEvent.TRANSITION_FADEOUT_WIN_FINISH, NextLevel);
        var current = SceneManager.GetActiveScene();
        MasterManager.GetNextScene(current);
        //MasterManager.nextScene = next;

        SceneManager.LoadScene("LoadingScreen");
    }

    public void RestartLevel(object[] parameterContainer) {
        foreach (var t in checkPoints.Where(t => t.checkPointName == MasterManager.checkPointName)) {
            _PC.RespawnOnCheckPoint(t.transform);
            EventManager.DispatchEvent(GameEvent.CAMERA_NORMAL);
            CutScenesManager.instance.DeActivateCutSceneCamera("DeathFall");
            _PC.cam2.normalState.SetInitialPosition(t.transform);
        }

        blackOutAnimator.SetTrigger("FadeInWithoutReset");
    }

    public void AddCheckPointToList(CheckPoint cp) {
        if (!checkPoints.Contains(cp)) {
            checkPoints.Add(cp);
        }
    }

    public void SetActiveCheckPoint(string cpName) {
        var isIncluded = checkPoints.Any(cp => cp.checkPointName == cpName);

        if (isIncluded) {
            MasterManager.checkPointName = cpName;
            return;
        }
#if UNITY_EDITOR
        Debug.LogWarning("El checkpoint no se encuentra en la lista");
#endif
    }

    public void AddBreathingMaterial(Material mat) {
        breathingScenarioMaterials.Add(mat);
    }

    private void OnDestroy() {
        EventManager.RemoveEventListener(GameEvent.TRANSITION_FADEOUT_LOSE_FINISH, RestartLevel);
    }
}