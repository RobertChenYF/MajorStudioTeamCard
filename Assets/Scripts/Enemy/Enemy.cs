    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Enemy: MonoBehaviour
{
    [SerializeField]protected float maxHp;
    [SerializeField] protected float startingArmor;
    [SerializeField]protected Image healthBarFill;
    [SerializeField] protected GameObject armorIconDisplay;
    [SerializeField] protected TextMeshPro armorAmountDisplay;
    [SerializeField] private GameObject buffDisplayPrefab;
    protected float currentHp;
    protected float currentArmor;
    [SerializeField]protected TextMeshPro enemyHpText;
    public Transform enemyBuffPos;
    [SerializeField] private TextMeshPro enemyNextMoveDisplay;
    private string nextMoveString;
    public List<EnemyBuff> enemyBuffList;
    public List<BuffHoverDisplay> BuffDisplayList;
    public List<EnemyMoveset> moveSet;

    //VisualEffects
    public bool is_Idle;

    private EnemyMoveset currentChargeMove;
    private int currentChargeCycleTimer;
    public virtual void Start()
    {
        enemyBuffList = new List<EnemyBuff>();
        BuffDisplayList = new List<BuffHoverDisplay>();
        currentHp = maxHp;
        currentArmor = startingArmor;
        UpdateDisplayStat();
        StartAmove();
        Services.combatManager.AllMainEnemy.Add(this);
        Services.eventManager.Register<CombatManager.TimeCycleEnd>(CycleChargeReduce);
        // currentHp = maxHp;
    }
    public virtual void Update()
    {
        /*
        enemyBuffText.text = "";
        foreach (EnemyBuff a in enemyBuffList)
        {
            enemyBuffText.text += a.tempString();
        }
        */

        //call idle animation
        //PlayEnemyIdleAnimation(is_Idle);

        //for damage effect testing
        if(Input.GetKeyDown(KeyCode.D))
        {
            if(this.gameObject)
                TakeDamage(10);
        }
    }
    void PlayEnemyIdleAnimation(bool is_Idle)
    {
        if (is_Idle)
        {

        }
    }

    public void UpdateBuffDisplay()
    {
        foreach (BuffHoverDisplay display in BuffDisplayList)
        {
            Destroy(display.gameObject);
        }
        BuffDisplayList.Clear();
        foreach (EnemyBuff a in enemyBuffList)
        {
            GameObject newBuff = Instantiate(buffDisplayPrefab,enemyBuffPos.transform.position,Quaternion.identity,enemyBuffPos);
            newBuff.GetComponent<BuffHoverDisplay>().thisBuff = a;
            newBuff.GetComponent<BuffHoverDisplay>().MakeBuff();
            newBuff.GetComponent<BuffHoverDisplay>().UpdateCount(a.getStack());
            BuffDisplayList.Add(newBuff.GetComponent<BuffHoverDisplay>());
        }
    }

    public void TakeDamage(float damage)
    {
        //Call visual effect
        StartCoroutine(Services.visualEffectManager.PlayEnemyTakeDamageEffect(this.GetComponent<Enemy>(), damage));

        if (currentArmor >= damage)
        {
            LoseArmor(damage);
        }
        else
        {
            int a = (int)currentArmor - Mathf.FloorToInt(damage);
            LoseArmor(damage);
            currentHp += a;
        }

        UpdateDisplayStat();
        if (currentHp <= 0)
        {
            Die();
            Debug.Log("enemy died");
        }
    }
    
    public virtual void Die()
    {
        if (Services.actionManager.currentTargetEnemy == this)
        {
            Services.actionManager.currentTargetEnemy = null;
        }
        Services.combatManager.AllMainEnemy.Remove(this);
        gameObject.SetActive(false);
    }

    public void GainNewBuff(EnemyBuff newBuff, int stack)
    {
        if (CheckBuff(newBuff) == -1)
        {
            newBuff.thisEnemy = this;
            enemyBuffList.Add(newBuff);
            newBuff.GainStack(stack - 1);
            newBuff.ActivateBuff();
            UpdateBuffDisplay();
            
        }
        else
        {
            enemyBuffList[CheckBuff(newBuff)].GainStack(stack);
            UpdateBuffDisplay();
        }

    }
    public void RemoveBuff(EnemyBuff buff)
    {
        enemyBuffList.Remove(buff);
        buff.DeactivateBuff();
        UpdateBuffDisplay();
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
            UpdateDisplayStat();
        }

    }

    private void UpdateDisplayStat()
    {
        enemyHpText.text =  currentHp.ToString()+"/" + maxHp.ToString();
        healthBarFill.fillAmount = currentHp / maxHp;
        if (currentArmor>0)
        {
            armorIconDisplay.SetActive(true);
            armorAmountDisplay.text = currentArmor.ToString();
        }
        else
        {
            armorIconDisplay.SetActive(false);
        }
        //enemyHpText.text += currentArmor > 0 ? "\nArmor: " + currentArmor : "";
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
        UpdateDisplayStat();
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

    public void OnMouseDown()
    {
        if (!Services.actionManager.attacking)
        {
        Services.actionManager.currentTargetEnemy = this;
        Debug.Log("change target enemy to" + name);
        }
        
    }
}


