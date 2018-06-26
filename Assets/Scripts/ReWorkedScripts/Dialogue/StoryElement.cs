using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryElement : MonoBehaviour {

    public Dialogue[] dialogue;
    int dialogueNumber;


    public void LoadDialogue(object[] parameterContainer)
    {
        DialogueManager.instance.StartDialogue(dialogue[dialogueNumber], true);
        dialogueNumber ++;
    }

    /*private void Start()
    {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }

    void Execute()
    {
        if (GameInput.instance.initialJumpButton)
        {
            LoadDialogue(null);
            EventManager.DispatchEvent(GameEvent.STORY_NEXT);
            EventManager.AddEventListener(GameEvent.STORY_NEXT, LoadDialogue);
            UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
        }
    }*/

}
