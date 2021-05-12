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
    [SerializeField] protected TextMeshPro IntentCycleText;
    [SerializeField] protected TextMeshPro IntentStatsText;
    [SerializeField] protected SpriteRenderer IntentIconDisplay;
    public GameObject IntentUI;
    public GameObject StatsUI;

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
    public List<EnemyMoveset> startingMove;

    //VisualEffects
    [Header ("Idle Animation")]
    public bool is_Idle;
    [SerializeField] private float idleSmoothingUp;
    [SerializeField] private float idleSmoothingDown;
    [SerializeField] private Vector3 savedIdlePosOffset;
    [SerializeField] private float waitTime;
    [SerializeField] private float hangTime;
    //[SerializeField] private float cutOffVal;
    public Color savedColor;
    private Vector3 idlePosOffset;
    bool goingUp = true;
    //[HideInInspector]
    public Vector3 savedEnemyPos;
    bool playing_idle = false;

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

        //Update Idle Position Offset
        savedColor = this.gameObject.GetComponent<SpriteRenderer>().color;
    }

    public void updateIdlePosOffset()
    {
        savedEnemyPos = this.gameObject.transform.position;
        //print(this.gameObject.transform.position.ToString());
        idlePosOffset = new Vector3(savedIdlePosOffset.x + savedEnemyPos.x, 
            savedIdlePosOffset.y + savedEnemyPos.y, savedIdlePosOffset.z + savedEnemyPos.z);
        is_Idle = true;
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

        //for damage effect testing
        if (Services.visualEffectManager.debug)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (this.gameObject)
                    TakeDamage(1);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (this.gameObject)
                    Services.visualEffectManager.EnemyGainBuffEffect(this.gameObject);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (this.gameObject)
                    Services.visualEffectManager.EnemyGainArmorEffect(this.gameObject);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                GainHp(5);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                Services.visualEffectManager.PlayEnemyDealDamageEffect(this.gameObject.GetComponent<Enemy>());
            }
        }


        if (is_Idle && !playing_idle)
        {
            StartCoroutine(PlayEnemyIdleAnimation());
        }
        if (!is_Idle && playing_idle)
        {
            playing_idle = false;
        }
    }
    IEnumerator PlayEnemyIdleAnimation()
    {
        //print(this.gameObject.transform.position.ToString());
        playing_idle = true;
        while (is_Idle)
        {
            if (goingUp && is_Idle)
            {
                while (Vector3.Distance(this.gameObject.transform.position, idlePosOffset) > 0.05f)
                {
                    if (!is_Idle)
                    {
                        playing_idle = false;
                        yield break;
                    }
                    this.gameObject.transform.position = EaseInOutQuad(this.gameObject.transform.position, idlePosOffset, idleSmoothingUp * Time.deltaTime);
                    yield return null;
                }
                goingUp = false;
                yield return new WaitForSeconds(hangTime);
            }
            else if (is_Idle)
            {
                while (Vector3.Distance(this.gameObject.transform.position, savedEnemyPos) > 0.05f)
                {
                    if (!is_Idle)
                    {
                        playing_idle = false;
                        yield break;
                    }
                    //print("here2");
                    this.gameObject.transform.position = EaseInOutQuad(this.gameObject.transform.position, savedEnemyPos, idleSmoothingDown * Time.deltaTime);
                    yield return null;
                }
                goingUp = true;
                yield return new WaitForSeconds(waitTime);
            }
        }
        playing_idle = false;
    }

    public static float EaseInOutQuad(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value + start;
        value--;
        return -end * 0.5f * (value * (value - 2) - 1) + start;
    }

    public static Vector3 EaseInOutQuad(Vector3 start, Vector3 end, float value)
    {
        return new Vector3(EaseInOutQuad(start.x, end.x, value), EaseInOutQuad(start.y, end.y, value), EaseInOutQuad(start.z, end.z, value));
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
        Services.visualEffectManager.PlayEnemyTakeDamageEffect(this.GetComponent<Enemy>(), damage);

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
        Services.eventManager.Unregister<CombatManager.TimeCycleEnd>(CycleChargeReduce);
        while (enemyBuffList.Count > 0)
        {
            RemoveBuff(enemyBuffList[0]);
        }
        Services.combatManager.AllMainEnemy.Remove(this);
        gameObject.SetActive(false);
    }

    public void GainNewBuff(EnemyBuff newBuff, int stack)
    {

        Services.visualEffectManager.EnemyGainBuffEffect(this.gameObject);

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
        Services.visualEffectManager.EnemyGainArmorEffect(this.gameObject);

        if (value > 0)
        {

            currentArmor += value;
            UpdateDisplayStat();
        }

    }

    public void GainHp(float value)
    {
        Services.visualEffectManager.PlayEnemyGainHealthEffect(this.gameObject.GetComponent<Enemy>(), value);
        currentHp += value;
        currentHp = Mathf.Min(maxHp,currentHp);
        UpdateDisplayStat();
    }
    private void UpdateDisplayStat()
    {
        enemyHpText.text =  currentHp.ToString()+"/" + maxHp.ToString();
        healthBarFill.fillAmount = currentHp / maxHp;
        if (currentArmor>0)
        {
            armorIconDisplay.SetActive(true);
            healthBarFill.color = Color.gray;
            armorAmountDisplay.text = currentArmor.ToString();
        }
        else
        {
            healthBarFill.color = Color.white;
            armorIconDisplay.SetActive(false);
        }
        //enemyHpText.text += currentArmor > 0 ? "\nArmor: " + currentArmor : "";
    }

    private void UpdateVisualIntent()
    {

    }

    private void CycleChargeReduce(AGPEvent e)
    {
        currentChargeCycleTimer--;
        LoseArmor(2);
        if (currentChargeCycleTimer == 0)
        {
            if (currentChargeMove.moves[0] == EnemyMoveset.moveType.dealDamage)
                Services.visualEffectManager.PlayEnemyDealDamageEffect(this.gameObject.GetComponent<Enemy>());
            currentChargeMove.MoveTrigger();
            NextMove();
        }
        UpdateCycleDisplay();
        
    }

    protected void StartAmove()
    {
        foreach (EnemyMoveset a in startingMove)
        {
            a.enemy = this;
        }
        foreach (EnemyMoveset a in moveSet)
        {
            a.enemy = this;
        }
        if (startingMove.Count > 0)
        {
            currentChargeMove = startingMove[0];
            startingMove.RemoveAt(0);
        }
        else
        {
            currentChargeMove = moveSet[0];
        }
        
        currentChargeCycleTimer = currentChargeMove.CycleBeforeMove;
        nextMoveString = currentChargeMove.MoveDisplay();
        SetIntentIcon();
        UpdateCycleDisplay();
    }
    public void LoseArmor(float dmg)
    {
        currentArmor -= dmg;
        currentArmor = Mathf.Max(0, currentArmor);
        UpdateDisplayStat();
    }

    public void SetIntentIcon()
    {
        IntentIconDisplay.sprite = currentChargeMove.MoveIcon(currentChargeMove.moves[0]);
        if (currentChargeMove.moves[0] == EnemyMoveset.moveType.dealDamage)
        {
            IntentStatsText.text = currentChargeMove.damageAmount.ToString();
        }
        else if (currentChargeMove.moves[0] == EnemyMoveset.moveType.GainArmor)
        {
            IntentStatsText.text = currentChargeMove.armorAmount.ToString();
        }
        else if (currentChargeMove.moves[0] == EnemyMoveset.moveType.Special)
        {
            IntentStatsText.text = "?";
        }
    }
    protected void NextMove()
    {
        int a;
        if (moveSet.Contains(currentChargeMove))
        {
            a = moveSet.IndexOf(currentChargeMove);
            a++;
        }
        else
        {
            a = 0;
        }
        
        if (a > moveSet.Count - 1)
        {
            a = 0;
        }
        currentChargeMove = moveSet[a];
        currentChargeCycleTimer = currentChargeMove.CycleBeforeMove;
        SetIntentIcon();
        nextMoveString = currentChargeMove.MoveDisplay();
    }

    protected void UpdateCycleDisplay()
    {
        IntentCycleText.text = currentChargeCycleTimer.ToString();
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


