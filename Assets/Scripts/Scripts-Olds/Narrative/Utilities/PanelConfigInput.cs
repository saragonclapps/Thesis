using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelConfigInput : MonoBehaviour {
    public Image inputImage;
    public Text tutorialText;

    public void Configure(InputInfo currentInputInfo)
    {
        inputImage.sprite = MasterManager.atlasManager.loadSprite(currentInputInfo.InputGamepadImage);
        tutorialText.text = currentInputInfo.TextTutorial;
    }
}
