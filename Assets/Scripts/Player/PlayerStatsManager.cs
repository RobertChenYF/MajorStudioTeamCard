using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerStatsManager : MonoBehaviour
{

    [SerializeField] private float maxHp;
    private float currentHp;
    //[SerializeField] private float startAttackDmg;
    [SerializeField] private float defaultArmorDepletion;
    [HideInInspector] public UnityEvent TakeDamageEvent;
    [HideInInspector] public float currentDamageAmount;
    [SerializeField] private TextMeshPro ArmorText;
    [SerializeField] private GameObject ArmorIcon;
    [SerializeField] private TextMeshPro HealthText;
    [SerializeField] private Image HealthBarfill;
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
        if (Services.visualEffectManager.debug)
        {
            //Effect Testing
            if (Input.GetKeyDown(KeyCode.F))
            {
                TakeDamage(5);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                GainHp(5);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Services.visualEffectManager.PlayPlayerGainArmorEffect(5);
            }
        }
    }

    private void LoseArmorAtCycleEnd(AGPEvent e)
    {
        LoseArmor(defaultArmorDepletion);
    }


    void TempUpdateDisplayStat()
    {

        if (currentArmor > 0)
        {
            ArmorIcon.SetActive(true);
            HealthBarfill.color = Color.gray;
        }
        else
        {
            ArmorIcon.SetActive(false);
            HealthBarfill.color = Color.white;
        }
        HealthBarfill.fillAmount = currentHp / maxHp;
        ArmorText.text = currentArmor.ToString();
        HealthText.text = currentHp.ToString() + "/" + maxHp.ToString();
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
        Services.visualEffectManager.PlayPlayerTakeDamageEffect(damage);

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
            Services.visualEffectManager.PlayPlayerGainArmorEffect(value);
            Services.eventManager.Fire(new GainArmorEvent(value));
            currentArmor += value;
            TempUpdateDisplayStat();
        }
        
    }

    public void LoseArmor(float dmg)
    {
        if (currentArmor > 0)
            Services.visualEffectManager.PlayPlayerLoseArmorEffect(dmg);
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
        Services.eventManager.Fire(new LoseHpEvent(dmg));
        if (currentHp <= 0)
        {
            Services.runStateManager.ChangeState(new Gameover(Services.runStateManager));
        }
        TempUpdateDisplayStat();
    }

    public class LoseHpEvent : AGPEvent
    {
        public float value;
        public LoseHpEvent(float amount)
        {
            value = amount;
        }
    }

    public void GainHp(float value)
    {
        Services.visualEffectManager.PlayPlayerGainHealthEffect(value);
        currentHp += value;
        currentHp = Mathf.Min(maxHp,currentHp);
        TempUpdateDisplayStat();
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
