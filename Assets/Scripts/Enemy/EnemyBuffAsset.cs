using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuff : Buff
{
    

    public Enemy thisEnemy;
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        if (stack == 0)
        {
            this.DeactivateBuff();
            thisEnemy.enemyBuffList.Remove(this);
        }
        thisEnemy.UpdateBuffDisplay();
    }
}

public class BodyPart : EnemyBuff
{
    public BodyPart()
    {
        buffDescription = "when this die deal 20 damage to the main body";

    }

    public override void ActivateBuff()
    {
        base.ActivateBuff();
        //Services.eventManager.Register<>
    }
}
public class Burn : EnemyBuff
{
    
    public Burn()
    {
        buffDescription = "Everytime new burn apply, take damage equal to the total stack of burn, lose a stack every time cycle";
        buffIcon = Services.playerBuffManager.BurnBuffIcon;
    }
    public override void ActivateBuff()
    {

        Services.eventManager.Register<CombatManager.TimeCycleEnd>(TriggerEffect);
    }
    public void TakeDamage()
    {
        thisEnemy.TakeDamage(stack);
    }
    public void TriggerEffect(AGPEvent e)
    {
        
        stack--;
        base.TriggerEffect();
    }

    public override void DeactivateBuff()
    {
        Services.eventManager.Unregister<CombatManager.TimeCycleEnd>(TriggerEffect);
    }
}
