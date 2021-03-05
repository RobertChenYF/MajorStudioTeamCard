using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Enemy: MonoBehaviour
{
    protected float maxHp;
    protected float currentHp;
    protected float currentArmor;
    [SerializeField]protected TextMeshPro enemyHpText;
    public TextMeshPro enemyBuffText;
    [SerializeField] private TextMeshPro enemyNextMoveDisplay;
    private string nextMoveString;
    public List<EnemyBuff> enemyBuffList;
    public List<EnemyMoveset> moveSet;

    private EnemyMoveset currentChargeMove;
    private int currentChargeCycleTimer;
    public virtual void Start()
    {
        enemyBuffList = new List<EnemyBuff>();
        TempUpdateDisplayStat();
        StartAmove();
        Services.eventManager.Register<CombatManager.TimeCycleEnd>(CycleChargeReduce);
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
        if (currentArmor >= damage)
        {
            currentArmor -= damage;
        }
        else
        {
            int a = (int)currentArmor - Mathf.FloorToInt(damage);
            currentArmor = 0;
            currentHp += a;
        }

        TempUpdateDisplayStat();
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
    public void GainArmor(float value)
    {
        if (value > 0)
        {

            currentArmor += value;
            TempUpdateDisplayStat();
        }

    }

    private void TempUpdateDisplayStat()
    {
        enemyHpText.text = "Enemy Hp: " + currentHp.ToString();
        enemyHpText.text += currentArmor > 0 ? "\nArmor: " + currentArmor : "";
    }

    private void CycleChargeReduce(AGPEvent e)
    {
        currentChargeCycleTimer--;
        LoseArmor(2);
        if (currentChargeCycleTimer == 0)
        {
            currentChargeMove.MoveTrigger();
            NextMove();
        }
        UpdateCycleDisplay();
    }

    protected void StartAmove()
    {
        foreach (EnemyMoveset a in moveSet)
        {
            a.enemy = this;
        }
        currentChargeMove = moveSet[0];
        currentChargeCycleTimer = currentChargeMove.CycleBeforeMove;
        nextMoveString = currentChargeMove.MoveDisplay();
        UpdateCycleDisplay();
    }
    public void LoseArmor(float dmg)
    {
        currentArmor -= dmg;
        currentArmor = Mathf.Max(0, currentArmor);
        TempUpdateDisplayStat();
    }
    protected void NextMove()
    {
        int a = moveSet.IndexOf(currentChargeMove);
        a++;
        if (a > moveSet.Count - 1)
        {
            a = 0;
        }
        currentChargeMove = moveSet[a];
        currentChargeCycleTimer = currentChargeMove.CycleBeforeMove;
        nextMoveString = currentChargeMove.MoveDisplay();
    }

    protected void UpdateCycleDisplay()
    {
        enemyNextMoveDisplay.text = "after " + currentChargeCycleTimer + " cycle\n" + nextMoveString;
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
            this.DeactivateBuff();
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
