﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelZeroMasterManager : MonoBehaviour {

    public StoryElement story;
    public Animator whiteOutAnimator;
    public string cutSceneTag;

    int storyCount = 0;
    Action<Transform> cameraStory;
	
	void Start ()
    {
        EventManager.AddEventListener(GameEvent.TRANSITION_FADEIN_FINISH, StartStory);
        EventManager.AddEventListener(GameEvent.STORY_END, WhiteOut);
        EventManager.AddEventListener(GameEvent.TRANSITION_FADEOUT_WIN_FINISH, ChangeLevel);
    }

    private void ChangeLevel(object[] parameterContainer)
    {
        SceneManager.LoadScene(1);
    }

    private void WhiteOut(object[] parameterContainer)
    {
        whiteOutAnimator.SetTrigger("WhiteOut");
        EventManager.RemoveEventListener(GameEvent.STORY_END, WhiteOut);
    }

    private void StartStory(object[] parameterContainer)
    {
        EventManager.DispatchEvent(GameEvent.CAMERA_STORY, cutSceneTag);
        EventManager.RemoveEventListener(GameEvent.TRANSITION_FADEIN_FINISH, StartStory);
        EventManager.AddEventListener(GameEvent.STORY_NEXT, story.LoadDialogue);
        story.LoadDialogue(null);
    }


    void OnDestroy()
    {
        EventManager.RemoveEventListener(GameEvent.STORY_NEXT, story.LoadDialogue);
        EventManager.RemoveEventListener(GameEvent.STORY_END, WhiteOut);
    }
}
