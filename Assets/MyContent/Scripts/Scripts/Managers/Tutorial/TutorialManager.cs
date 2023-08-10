using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = Logger.Debug;

public class TutorialManager : MonoBehaviour {
    public TutorialSetup tutorialSetup;
    public UITransparentTransition container;
    public Image buttonHowToPlay;
    public TextMeshProUGUI tutorialTextField;
    public TutorialCameraAnimation cameraAnimation;

    private Dictionary<TutorialSetupEntryDataType, Action<string, TutorialSetupEntryData>> _tutoriasPlay =
        new Dictionary<TutorialSetupEntryDataType, Action<string, TutorialSetupEntryData>>();
    
    private Dictionary<TutorialSetupEntryDataType, Action<string, TutorialSetupEntryData>> _tutorialsStop =
        new Dictionary<TutorialSetupEntryDataType, Action<string, TutorialSetupEntryData>>();

    private void Start() {
        EventManager.AddEventListener(GameEvent.TRIGGER_TUTORIAL, OnTutorialPlay);
        EventManager.AddEventListener(GameEvent.TRIGGER_TUTORIAL_STOP, OnTutorialStop);
        
        _tutoriasPlay[TutorialSetupEntryDataType.BUTTON] = OnButtonTutorial;
        _tutorialsStop[TutorialSetupEntryDataType.BUTTON] = ExitButtonTutorial;
        
        _tutoriasPlay[TutorialSetupEntryDataType.CAMERA_ANIMATION] = OnAnimationTutorial;
        _tutorialsStop[TutorialSetupEntryDataType.CAMERA_ANIMATION] = ExitAnimationTutorial;
    }


    private void OnTutorialPlay(object[] p) {
        var tutorialKey = (string)p[0];
#if UNITY_EDITOR
        Debug.Log(this, "PLAY tutorialKey: " + tutorialKey);
#endif
        var tutorialEntry = tutorialSetup.Get(tutorialKey);
        _tutoriasPlay[tutorialEntry.type](tutorialKey, tutorialEntry);
    }
    private void OnTutorialStop(object[] p) {
        var tutorialKey = (string)p[0];
#if UNITY_EDITOR
        Debug.Log(this, "STOP tutorialKey: " + tutorialKey);
#endif
        var tutorialEntry = tutorialSetup.Get(tutorialKey);
        _tutorialsStop[tutorialEntry.type](tutorialKey, tutorialEntry);
    }

    #region Tutorials

    private void OnButtonTutorial(string tutorialKey, TutorialSetupEntryData data) {
        container.StartTransition(true, tutorialKey);
        tutorialTextField.text = data.text;
        buttonHowToPlay.sprite = data.button;
    }
    
    private void ExitButtonTutorial(string tutorialKey, TutorialSetupEntryData data) {
        container.StartTransition(false, tutorialKey);
    }
    
    private void OnAnimationTutorial(string tutorialKey, TutorialSetupEntryData data) {
        cameraAnimation.StartAnimation(data);
    }
    
    private void ExitAnimationTutorial(string tutorialKey, TutorialSetupEntryData data) {
    }
    #endregion

    private void TurnOffGraphics() {
    }
}