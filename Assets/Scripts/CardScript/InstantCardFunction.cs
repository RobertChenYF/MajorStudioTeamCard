using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantCardFunction : CardFunction
{
    public override void AfterPlayed()
    {
        //move up a bit then trigger effect
        TriggerEffect();
    }


}
