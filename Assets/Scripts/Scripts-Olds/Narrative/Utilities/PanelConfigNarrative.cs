using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelConfigNarrative : MonoBehaviour {
	public Image avatarImage;
	public Image textBG;
	public Text characterName;
	public Text dialogue;
    private const float _TIME_AUTO_PRESS = 3f;
    private const float _TIME_AUTO_WRITE = 0.03f;

    public void Configure(Dialogue currentDialogue) {

		avatarImage.sprite = MasterManager.atlasManager.loadSprite (currentDialogue.atlasImageName);
		characterName.text = currentDialogue.name;

        StopAllCoroutines();
        StartCoroutine(AnimateText(currentDialogue.dialogueText));
    }

	IEnumerator AnimateText(string dialogueText) {
		dialogue.text = "";

		foreach (char letter in dialogueText) {
			dialogue.text += letter;
			yield return new WaitForSeconds (_TIME_AUTO_WRITE);
		}
        StartCoroutine(AutoPressButton());
	}

    private IEnumerator AutoPressButton()
    {
        yield return new WaitForSeconds(_TIME_AUTO_PRESS);
        GameInput.instance.chatButton = true;
    }
}
