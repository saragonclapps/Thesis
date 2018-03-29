using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class AnimationManager : MonoBehaviour, IManager {

	public ManagerState currentState { get; private set; }

	Animator panelAnimatorNarrative;
    Animator panelAnimatorInput;


    public void BootSequence() {
		//Debug.Log (string.Format ("{0} is booting up", GetType ().Name));

		panelAnimatorNarrative = GameObject.Find ("Narrative").GetComponent<Animator> ();
        panelAnimatorInput = GameObject.Find("TutorialInput").GetComponent<Animator> ();
        currentState = ManagerState.Completed;

		//Debug.Log (string.Format ("{0} status = {1}", GetType ().Name, currentState));
	}

    public IEnumerator IntroAnimationInput()
    {
        AnimationTuple introAnim = Constants.AnimationTuples.introAnimation;
        panelAnimatorInput.SetBool(introAnim.parameter, introAnim.value);

        yield return new WaitForSeconds(1);
    }

    public IEnumerator ExitAnimationInput()
    {
        AnimationTuple exitAnim = Constants.AnimationTuples.exitAnimation;
        panelAnimatorInput.SetBool(exitAnim.parameter, exitAnim.value);

        yield return new WaitForSeconds(1);
    }


    public IEnumerator IntroAnimationNarrative() {
		AnimationTuple introAnim = Constants.AnimationTuples.introAnimation;
		panelAnimatorNarrative.SetBool (introAnim.parameter, introAnim.value);

		yield return new WaitForSeconds (1);
	}

	public IEnumerator ExitAnimationNarrative() {
		AnimationTuple exitAnim = Constants.AnimationTuples.exitAnimation;
		panelAnimatorNarrative.SetBool (exitAnim.parameter, exitAnim.value);

		yield return new WaitForSeconds (1);
	}

}
