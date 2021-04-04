using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerCardFunction : CardFunction
{

    public override void AfterPlayed()
    {
        //move up a bit then trigger effect
        TriggerEffect();
    }

    public override void AfterTriggered()
    {
        Services.actionManager.AddToExhaustPile(this);
    }

}
