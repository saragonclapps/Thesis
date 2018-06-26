using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelZeroMasterManager : MonoBehaviour {

    public Transform[] cameraPositions;
    public StoryElement story;
    public Animator whiteOutAnimator;

    int storyCount = 0;
    Action<Transform> cameraStory;
	
	void Start ()
    {
        EventManager.AddEventListener(GameEvent.TRANSITION_FADEIN_FINISH, StartStory);
        cameraStory = CameraStoryUpdate;
        EventManager.DispatchEvent(GameEvent.CAMERA_STORY, cameraStory);
        EventManager.AddEventListener(GameEvent.STORY_END, WhiteOut);
        EventManager.AddEventListener(GameEvent.TRANSITION_FADEOUT_FINISH, ChangeLevel);
    }

    private void ChangeLevel(object[] parameterContainer)
    {
        SceneManager.LoadScene(1);
    }

    private void WhiteOut(object[] parameterContainer)
    {
        whiteOutAnimator.SetTrigger("WhiteOut");
    }

    private void StartStory(object[] parameterContainer)
    {
        EventManager.RemoveEventListener(GameEvent.TRANSITION_FADEIN_FINISH, StartStory);
        EventManager.AddEventListener(GameEvent.STORY_NEXT, NextPosition);
        EventManager.AddEventListener(GameEvent.STORY_NEXT, story.LoadDialogue);
        story.LoadDialogue(null);
    }

    private void NextPosition(object[] parameterContainer)
    {
        storyCount++;
    }

    void CameraStoryUpdate(Transform transform)
    {
        transform.position = cameraPositions[storyCount].position;
        transform.rotation = cameraPositions[storyCount].rotation;
    }

    void OnDestroy()
    {
        EventManager.RemoveEventListener(GameEvent.STORY_NEXT, NextPosition);
        EventManager.RemoveEventListener(GameEvent.STORY_NEXT, story.LoadDialogue);
    }
}
