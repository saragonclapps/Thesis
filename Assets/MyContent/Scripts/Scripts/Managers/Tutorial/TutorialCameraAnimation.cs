using System;
using UnityEngine;
using Debug = Logger.Debug;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Animator))]
public class TutorialCameraAnimation : MonoBehaviour {
    private Camera _camera;
    private Animator _animator;
    private TutorialSetupEntryData _currentData;
    public CanvasGroup[] toHide;
    
    private void Start() {
        _camera = GetComponent<Camera>();
        _animator = GetComponent<Animator>();
        _camera.enabled = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.O)) {
            Debug.Log("Space pressed");
            StopAnimation();
        }
    }

    public void StartAnimation(TutorialSetupEntryData data) {
        _currentData = data;
        foreach (var cg in toHide) {
            cg.alpha = 0;
        }
        _camera.enabled = true;
        EventManager.DispatchEvent(GameEvent.TUTORIAL_ANIMATION, true);
        _animator.Play(data.animation.name);
    }
    
    public void StopAnimation() {
        foreach (var cg in toHide) {
            cg.alpha = 1f;
        }
        
        _camera.enabled = false;
        EventManager.DispatchEvent(GameEvent.TUTORIAL_ANIMATION, false);
    }
}