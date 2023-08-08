using UnityEngine;
using System;

public enum TutorialSetupEntryDataType {
    NONE = 0,
    BUTTON = 1,
    CAMERA_ANIMATION = 2
}

    
[Serializable]
public class TutorialSetupEntryData {
    public TutorialSetupEntryDataType type = 0;
    // Button
    public string text;
    public Sprite button;
    public string soundKey;
    // Camera Animation
    public AnimationClip animation;
    public bool blockMovement;
}