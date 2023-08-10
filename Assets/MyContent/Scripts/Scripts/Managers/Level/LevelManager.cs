using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = Logger.Debug;

public class LevelManager : MonoBehaviour {
    public float levelTime;
    public bool isWithPowers;
    private bool _hasDiskette;

    public Animator blackOutAnimator;
    public Animator whiteOutAnimator;
    
    public bool hasDiskette {
        get { return _hasDiskette; }
        set { _hasDiskette = value; }
    }

    private static LevelManager _instance;
    public static LevelManager instance => _instance;

    public List<CheckPoint> checkPoints;

    private PlayerController _pc;

    private void Awake() {
        _instance = this;
        checkPoints = new List<CheckPoint>();
    }

    private void Start() {
        _pc = FindObjectOfType<PlayerController>();

        var checkPointsFiltered = checkPoints
            .Where(cp => cp.checkPointName == MasterManager.checkPointName);
        foreach (var cp in checkPointsFiltered) {
            _pc.transform.position = cp.transform.position;
            _pc.transform.rotation = cp.transform.rotation;
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
        SceneManager.LoadScene("LoadingScreen");
    }

    public void RestartLevel(object[] parameterContainer) {
        var checkpointsFiltered = checkPoints
            .Where(t => t.checkPointName == MasterManager.checkPointName);
        foreach (var checkPoint in checkpointsFiltered) {
            _pc.RespawnOnCheckPoint(checkPoint.transform);
            EventManager.DispatchEvent(GameEvent.CAMERA_NORMAL);
            AudioPlayerEmitter.instance.SetMute(false);
            AudioManager.instance.PlayAudio("Respawn", AudioMode.OneShot, AudioGroup.SFX_POWERS);
            CutScenesManager.instance.DeActivateCutSceneCamera("DeathFall");
            _pc.cam2.normalState.SetInitialPosition(checkPoint.transform);
        }

        blackOutAnimator.SetTrigger("FadeInWithoutReset");
    }

    public void AddCheckPointToList(CheckPoint cp) {
        if (checkPoints.Contains(cp)) return;
        checkPoints.Add(cp);
    }

    public void SetActiveCheckPoint(string cpName) {
        var isIncluded = checkPoints.Any(cp => cp.checkPointName == cpName);

        if (isIncluded) {
            MasterManager.checkPointName = cpName;
            return;
        }
#if UNITY_EDITOR
        Debug.LogWarning("The checkpoint " + cpName + " is not included in the list of checkpoints");
#endif
    }

    private void OnDestroy() {
        EventManager.RemoveEventListener(GameEvent.TRANSITION_FADEOUT_LOSE_FINISH, RestartLevel);
    }
}