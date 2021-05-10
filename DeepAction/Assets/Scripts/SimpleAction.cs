using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleAction
{
    //a simple action is kind of like a macro
    //its a single piece of code that does...something
    //it has a ref to its owner and such.

    //examples
    //aoe explosion for x damage
    //play sound at location
    //affect parents stats.

    public abstract void TriggerAction();
    public virtual void InitializeAction(){ }
    public virtual void DestroyAction(){ }
}
