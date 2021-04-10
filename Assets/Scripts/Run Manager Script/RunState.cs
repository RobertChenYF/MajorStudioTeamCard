using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RunState
{
    protected RunStateManager manager; // The manager that contains the state machine.

    public abstract void StateBehavior();

    public virtual void Enter()
    {

    } // Virtual so can be overriden in derived classes.
    public virtual void Leave()
    {


    }

    public RunState(RunStateManager theManager) // Constructor that takes an argument.
    {
        manager = theManager;
    }

}
