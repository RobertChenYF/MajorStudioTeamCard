using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCardFunction : CardFunction
{
    public override void AfterPlayed()
    {
        playerActionManager.AddToAttackField(this);
    }
}
