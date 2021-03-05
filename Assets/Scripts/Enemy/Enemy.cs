using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy: MonoBehaviour
{
    protected float maxHp;
    protected float currentHp;
    protected TextMeshPro enemyHpText;
    public TextMeshPro enemyBuffText;
    public List<EnemyBuff> enemyBuffList;
    public virtual void Start()
    {
        enemyBuffList = new List<EnemyBuff>();
       // currentHp = maxHp;
    }
    public virtual void Update()
    {
        enemyBuffText.text = "";
        foreach (EnemyBuff a in enemyBuffList)
        {
            enemyBuffText.text += a.tempString();
        }
    }


    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        enemyHpText.text = "Enemy Hp: " + currentHp.ToString();
        if (currentHp <= 0)
        {
            Debug.Log("enemy died");
        }
    }
    

    public void GainNewBuff(EnemyBuff newBuff, int stack)
    {
        if (CheckBuff(newBuff) == -1)
        {
            newBuff.thisEnemy = this;
            enemyBuffList.Add(newBuff);
            newBuff.GainStack(stack - 1);
            newBuff.ActivateBuff();
            
        }
        else
        {
            enemyBuffList[CheckBuff(newBuff)].GainStack(stack);
        }

    }
    public void RemoveBuff(EnemyBuff buff)
    {
        enemyBuffList.Remove(buff);
        buff.DeactivateBuff();
    }

    public int CheckBuff(EnemyBuff a)
    {
        foreach (EnemyBuff b in enemyBuffList)
        {
            if (b.ToString().Equals(a.ToString()))
            {
                return enemyBuffList.IndexOf(b);
            }
        }
        return -1;
    }


}

public class EnemyBuff : Buff
{
    public Enemy thisEnemy;
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        if (stack == 0)
        {
            thisEnemy.enemyBuffList.Remove(this);
        }
    }
}

public class Burn : EnemyBuff
{
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
