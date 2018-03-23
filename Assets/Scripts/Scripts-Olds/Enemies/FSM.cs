using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<InputT>{

    private IState<InputT> currentState;
    public IState<InputT> Current { get { return currentState; } }

	public FSM(IState<InputT> initialState)
    {
        initialState.Enter();
        currentState = initialState;
    }

    public void Execute()
    {
        currentState.Execute();
    }

    public void ProcessInput(InputT input)
    {
        var currentStateTransitions = currentState.Transitions;
        if (currentStateTransitions.ContainsKey(input))
        {
            currentState.Exit();
            currentState = currentStateTransitions[input];
            currentState.Enter();
        }
    }

}
