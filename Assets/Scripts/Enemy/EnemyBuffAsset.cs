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

public class GainHpWhenPlayerLoseHp : EnemyBuff
{
    public GainHpWhenPlayerLoseHp()
    {
        buffDescription = "Gain health equals to the amount of health player loses";
        //buffIcon = Services.playerBuffManager.BurnBuffIcon;
    }

    public override void ActivateBuff()
    {

        Services.eventManager.Register<PlayerStatsManager.LoseHpEvent>(TriggerEffect);
    }

    public void TriggerEffect(AGPEvent e)
    {
        PlayerStatsManager.LoseHpEvent a = (PlayerStatsManager.LoseHpEvent) e;
        thisEnemy.GainHp(a.value);
        thisEnemy.SummonFollower(Services.cardList.AngryKlippy);
        base.TriggerEffect();
    }

    public override void DeactivateBuff()
    {
        Services.eventManager.Unregister<PlayerStatsManager.LoseHpEvent>(TriggerEffect);
    }
}

public class DieAfterAttack: EnemyBuff
{
    public DieAfterAttack()
    {
        buffDescription = "Die After Attack";
        //buffIcon = Services.playerBuffManager.BurnBuffIcon;
    }

    public override void ActivateBuff()
    {

        Services.eventManager.Register<CombatManager.TimeCycleEnd>(TriggerEffect);
    }

    public void TriggerEffect(AGPEvent e)
    {

        
        base.TriggerEffect();
        thisEnemy.Die();
        
    }

    public override void DeactivateBuff()
    {
        Services.eventManager.Unregister<CombatManager.TimeCycleEnd>(TriggerEffect);
    }
}

public class KlippyDie : EnemyBuff
{
    public KlippyDie()
    {
        buffDescription = "Please do not kill Klippy";
    }

    public override void ActivateBuff()
    {
        Services.eventManager.Register<Enemy.DieEvent>(TriggerEffect);
    }

    public void TriggerEffect(AGPEvent e)
    {
        Debug.Log("EnemyDie");
        Enemy.DieEvent a = (Enemy.DieEvent)e;
        if (a.deadEnemy.gameObject == thisEnemy.gameObject)
        {
            base.TriggerEffect();
            thisEnemy.SummonFollower(Services.cardList.AngryKlippy);

        }
        //summon angry klippy
    }

    public override void DeactivateBuff()
    {
        Services.eventManager.Unregister<Enemy.DieEvent>(TriggerEffect);
    }
}

public class MoreDamage : EnemyBuff
{
    public MoreDamage(){

        buffDescription = "deal 1 extra damage for each stack of this every time I deal damage";
    }

    public override void ActivateBuff()
    {
        Services.eventManager.Register<EnemyMoveset.EnemyDealDamage>(TriggerEffect);
    }

    public void TriggerEffect(AGPEvent e)
    {

        EnemyMoveset.EnemyDealDamage a = (EnemyMoveset.EnemyDealDamage)e;
        
        base.TriggerEffect();
        Services.statsManager.TakeDamage(stack);

       
    }

    public override void DeactivateBuff()
    {
        Services.eventManager.Unregister<EnemyMoveset.EnemyDealDamage>(TriggerEffect);
    }
}

public class USBDie : EnemyBuff
{
    public USBDie()
    {
        buffDescription = "I become Stronger after other enemy dies";
    }

    public override void ActivateBuff()
    {
        Services.eventManager.Register<Enemy.DieEvent>(TriggerEffect);
    }

    public void TriggerEffect(AGPEvent e)
    {
        //Debug.Log("EnemyDie");
        thisEnemy.GainNewBuff(new MoreDamage(),2);
        Services.visualEffectManager.EnemyGainBuffEffect(thisEnemy.gameObject);
        //become stronger
       base.TriggerEffect();
       
        
    }

    public override void DeactivateBuff()
    {
        Services.eventManager.Unregister<Enemy.DieEvent>(TriggerEffect);
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
