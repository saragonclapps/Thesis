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

    private void Start() {
        tutorialSetup.Clean();
        EventManager.AddEventListener(GameEvent.TRIGGER_TUTORIAL, OnTutorial);
        EventManager.AddEventListener(GameEvent.TRIGGER_TUTORIAL_STOP, OnTutorialStop);
    }
    

    private void OnTutorial(object[] p) {
        var tutorialKey = (string)p[0];
#if UNITY_EDITOR
        Debug.Log(this, "tutorialKey: " + tutorialKey);
#endif
        container.StartTransition(true, tutorialKey);
        var tutorialEntry = tutorialSetup.Get(tutorialKey);
        tutorialTextField.text = tutorialEntry.text;
        buttonHowToPlay.sprite = tutorialEntry.button;
    }

    private void OnTutorialStop(object[] p) {
        var tutorialKey = (string)p[0];
        container.StartTransition(false, tutorialKey);
    }

    private void TurnOffGraphics() {
    }
}