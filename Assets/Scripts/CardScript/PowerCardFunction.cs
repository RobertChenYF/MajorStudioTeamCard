using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerCardFunction : CardFunction
{
    [Header("trigger requirement")]
    [SerializeField] protected UnityEvent triggerRequirement;

    

    public override void AfterPlayed()
    {
        triggerRequirement.Invoke();
        //go to power card location
    }

    public void TriggerEffect(AGPEvent e)
    {
        triggered.Invoke();
    }
    public override void AfterTriggered()
    {
        //stay so can be triggered again
    }

}
