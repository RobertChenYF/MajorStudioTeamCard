using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PlayerStatsManager : MonoBehaviour
{

    [SerializeField] private float maxHp;
    private float currentHp;
    //[SerializeField] private float startAttackDmg;
    [SerializeField] private float defaultArmorDepletion;
    [HideInInspector] public UnityEvent TakeDamageEvent;
    [HideInInspector] public float currentDamageAmount;
    //private float currentAttackDmg;
    //private float buffedAttackDmg;
    private float currentArmor;
    public float getArmor()
    {
        return currentArmor;
    }
    
    [SerializeField] private TextMeshPro playerStatsText;


    // Start is called before the first frame update
    void Start()
    {
        //Manager = GameObject.FindWithTag("GameManager");
        //maxHp = Manager.GetComponent<GameManager>().PlayerHealth;
        currentHp = maxHp;
        //currentAttackDmg = startAttackDmg;
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
        playerStatsText.text = "HP: " + currentHp.ToString() + "/" + maxHp.ToString() + "\nEncryption: " + currentArmor.ToString();
    }

    public class PlayerTakeDamageEvent: AGPEvent{

        public PlayerTakeDamageEvent(float amount)
        {

        }
    }

    public void TakeDamage(float damage)
    {
        //Play VisualEffect
        StartCoroutine(Services.visualEffectManager.PlayPlayerTakeDamageEffect());

        currentDamageAmount = damage;
        TakeDamageEvent.Invoke();
        if (currentDamageAmount > 0)
        {
        Services.eventManager.Fire(new PlayerTakeDamageEvent(damage));
        if (currentArmor < damage && currentArmor > 0)
        {
            LoseHp(damage - currentArmor);
            LoseArmor(currentArmor);
            
        }
        else if(currentArmor >= damage)
        {
            LoseArmor(damage);
        }
        else if(currentArmor == 0)
        {
            LoseHp(damage);
        }
        TempUpdateDisplayStat();
        }


        
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
        
        Services.eventManager.Fire(new GainArmorEvent(value));
        currentArmor += value;
        TempUpdateDisplayStat();
        }
        
    }

    public void LoseArmor(float dmg)
    {
        currentArmor -= dmg;
        currentArmor = Mathf.Max(0, currentArmor);
        TempUpdateDisplayStat();
    }
    public void LoseAllArmor()
    {
        currentArmor = 0;
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
    /*
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
    */
}
