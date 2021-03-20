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
public class Burn : EnemyBuff
{
    
    public Burn()
    {
        buffDescription = "At the end of cycle, the target loses "+ "X" + " HP and 1 stack of burn.";
        buffIcon = Services.playerBuffManager.BurnBuffIcon;
    }
    public override void ActivateBuff()
    {

        Services.eventManager.Register<CombatManager.TimeCycleEnd>(TriggerEffect);
    }

    public void TriggerEffect(AGPEvent e)
    {
        thisEnemy.TakeDamage(stack);
        stack--;
        base.TriggerEffect();
    }

    public override void DeactivateBuff()
    {
        Services.eventManager.Unregister<CombatManager.TimeCycleEnd>(TriggerEffect);
    }
}
