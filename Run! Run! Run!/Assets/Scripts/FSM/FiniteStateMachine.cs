using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FiniteStateMachine : MonoBehaviour
{
    // list of all our states
    private Dictionary<Type, BaseState> statesList;
    // base state object
    public BaseState state;

    // Update is called once per frame
    void Update()
    {
        // if no state is set, start off in base state
        if(currentState == null)
        {
            currentState = statesList.Values.First();
        }
        else
        {
            // run update in the current state until conditions are met to change state
            var nextState = currentState.StateUpdate();

            // check if next state is the same as current state
            if (nextState != null && nextState != currentState.GetType())
            {
                // go to new state
                SwitchState(nextState);
            }
        }
    }

    // getter/setter
    public BaseState currentState
    { 
        get
        {
            return state;
        }
        set
        {
            state = value;
        }
    }

    // set state to this state
    public void SetState(Dictionary<Type, BaseState> state)
    {
        this.statesList = state;
    }

    void SwitchState(Type nextState)
    {
        // run exit function in current state
        currentState.StateExit();
        // change to new state
        currentState = statesList[nextState];
        //run start function in new state
        currentState.StateEnter();
    }
}
