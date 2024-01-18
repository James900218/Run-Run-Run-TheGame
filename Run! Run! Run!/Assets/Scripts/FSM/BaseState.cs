using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseState
{
    // Called once per frame when in state
    public abstract Type StateUpdate();
    //called once when entering a state
    public abstract Type StateEnter();
    // called once when exiting a state
    public abstract Type StateExit();

}


