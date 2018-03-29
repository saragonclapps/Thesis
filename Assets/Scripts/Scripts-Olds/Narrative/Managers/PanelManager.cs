using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;
using System;

public class PanelManager : MonoBehaviour, IManager, IObservable {

	public ManagerState currentState { get; private set; }
    private event Action _subscripts = delegate { };

	private PanelConfigNarrative _leftPanelNarrative;
    private PanelConfigInput _RightPanelInput;
    private NarrativeEvent _currentEventNarrative;
    private TutorialEvent _currentEventInput;
    private bool _leftCharacterActive = true;
	private int _stepIndex = 0;
    public static PanelManager instance;
    public bool isActivePanelNarration;
    public bool isActivePanelInput;
    public bool stateJump = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            instance = this;
        }
    }

    public void FireSequenceInput(int EventInput, float time = 5){
        InitializePanelsInput(EventInput, time);
    }

    public void FireSequenceScene(float scene)
    {
        //stateJump = GameInput.instance.GetLockFeature(GameInput.Features.JUMP);
        //GameInput.instance.ChangeLockFeature(GameInput.Features.JUMP, false);
        _currentEventNarrative = JSONAssembly.RunJSONFactoryForScene(scene);
        InitializePanelsNarrative();
    }

    public void BootSequence() {
        //Debug.Log (string.Format ("{0} is booting up", GetType().Name));

        _leftPanelNarrative = GameObject.Find ("LeftCharacterPanel").GetComponent<PanelConfigNarrative> ();
        _RightPanelInput = GameObject.Find("RightInputPanel").GetComponent<PanelConfigInput>();
        _currentEventNarrative = JSONAssembly.RunJSONFactoryForScene(1);
        _currentEventInput = JSONAssembly.RunJSONFactoryForEvent(-1);
        InitializePanelsNarrative();
        //Debug.Log (string.Format ("{0} status = {1}", GetType().Name, currentState));
    }

    void Update() {
		if (GameInput.instance.chatButton) {
			UpdatePanelState ();
		}
	}

    private void InitializePanelsInput(int EventInput, float time)
    {
        _RightPanelInput.Configure(_currentEventInput.inputInfo[EventInput]);
        StartCoroutine(MasterManager.animationManager.IntroAnimationInput());
        StartCoroutine(WaitTime(time));
    }

    private IEnumerator WaitTime(float time)
    {
        isActivePanelInput = true;
        yield return new WaitForSeconds(time);
        StartCoroutine(MasterManager.animationManager.ExitAnimationInput());
        isActivePanelInput = false;
    }

    private void InitializePanelsNarrative() {
        _stepIndex = 0;
		_leftPanelNarrative.Configure (_currentEventNarrative.dialogues [_stepIndex]);
        StartCoroutine(MasterManager.animationManager.IntroAnimationNarrative ());

		_stepIndex++;
	}

	private void ConfigurePanels() {
        _leftPanelNarrative.Configure(_currentEventNarrative.dialogues[_stepIndex]);
    }

	void UpdatePanelState() {
		if (_stepIndex < _currentEventNarrative.dialogues.Count) {
            isActivePanelNarration = true;
			ConfigurePanels ();
			_leftCharacterActive = !_leftCharacterActive; 
			_stepIndex++;
		} else {
            isActivePanelNarration = false;
            //GameInput.instance.ChangeLockFeature(GameInput.Features.JUMP, stateJump);
            _subscripts();
			StartCoroutine (MasterManager.animationManager.ExitAnimationNarrative ());
		}
	}

    public void Subscribe(Action observer)
    {
        _subscripts += observer;
    }

    public void UnSubscribe(Action observer)
    {
        _subscripts -= observer;
    }
}
