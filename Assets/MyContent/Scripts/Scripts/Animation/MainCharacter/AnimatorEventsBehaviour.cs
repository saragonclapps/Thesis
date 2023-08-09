using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventsBehaviour : MonoBehaviour {
    private Animator _anim;
    public bool landEnd;
    [SerializeField] private AudioPlayer audioPlayer;

    void Start() {
        landEnd = true;
        _anim = GetComponent<Animator>();
    }
    
    private void OnJump() {
        audioPlayer.PlayJumpAndOnLandAudio(0.1f);
    }
    
    private void OnFootStep() {
        audioPlayer.PlayFootStepAudio(1);
    }

    public void LandEnd() {
        landEnd = true;
        _anim.SetBool("toLand", false);
    }
    
    public void LandStart() {
        audioPlayer.PlayJumpAndOnLandAudio(0.3f);
        landEnd = false;
    }
}