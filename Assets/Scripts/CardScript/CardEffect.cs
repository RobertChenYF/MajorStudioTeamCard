using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : MonoBehaviour
{


    public void zz_Effect_AddAttack(int value)
    {
        
        Services.statsManager.GainTempAttack(value);
    }

    public void zz_Effect_ConsumeDrawBar(int value)
    {

    }



    public void zz_Effect_GainArmor(int value)
    {
        
        Services.statsManager.GainArmor(value);
    }

    public void zz_Test_Power_Whenever_Get_Armor(PowerCardFunction card)
    {
        Services.eventManager.Register<PlayerStatsManager.GainArmorEvent>(card.TriggerEffect);
    }
}
