using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatsManager : MonoBehaviour
{
    
    [SerializeField]private float maxHp;
    private float currentHp;
    [SerializeField]private float startAttackDmg;
    [SerializeField]private float defaultArmorDepletion;

    private float currentAttackDmg;
    private float buffedAttackDmg;
    private float currentArmor;
    [SerializeField] private TextMeshPro playerStatsText;


    // Start is called before the first frame update
    void Start()
    {
        
        currentHp = maxHp;
        currentAttackDmg = startAttackDmg;
        Services.eventManager.Register<CombatManager.TimeCycleEnd>(LoseArmorAtCycleEnd);
        TempUpdateDisplayStat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoseArmorAtCycleEnd(AGPEvent e)
    {
        LoseArmor(defaultArmorDepletion);
    }


    void TempUpdateDisplayStat()
    {
        //temporary will replace with visual UI
        playerStatsText.text = "HP: " + currentHp.ToString() + "/" + maxHp.ToString() + "\nArmor: " + currentArmor.ToString() + "\nAttack: " + currentAttackDmg.ToString(); 
    }

    public void TakeDamage(float damage)
    {
        if (currentArmor < damage && currentArmor > 0)
        {
            LoseArmor(currentArmor);
            LoseHp(damage - currentArmor);
        }
        else if(currentArmor >= damage)
        {
            LoseArmor(damage);
        }
        else if(currentArmor <= 0)
        {
            LoseHp(damage);
        }
        TempUpdateDisplayStat();
        
    }
    public class GainArmorEvent : AGPEvent
    {

        public GainArmorEvent(float amount)
        {

        }
    }
    public void GainArmor(float value)
    {
        if (value > 0)
        {
        currentArmor += value;
        TempUpdateDisplayStat();
            Services.eventManager.Fire(new GainArmorEvent(value));
        }
        
    }

    public void LoseArmor(float dmg)
    {
        currentArmor -= dmg;
        currentArmor = Mathf.Max(0, currentArmor);
        TempUpdateDisplayStat();
    }

    public void LoseHp(float dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0)
        {
            Debug.Log("player die");
        }
    }

    public void GainHp(float value)
    {
        currentHp += value;
        currentHp = Mathf.Min(maxHp,currentHp);
    }

    public void GainTempAttack(float value)
    {
        buffedAttackDmg += value;
        currentAttackDmg = startAttackDmg + buffedAttackDmg;
        TempUpdateDisplayStat();
    }
    public void LoseAllTempAttack()
    {
        buffedAttackDmg = 0;
        currentAttackDmg = startAttackDmg + buffedAttackDmg;
        TempUpdateDisplayStat();
    }
    public float GetCurrentAttackDmg()
    {
        return currentAttackDmg;
    }
}
