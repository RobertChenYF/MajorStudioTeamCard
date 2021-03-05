using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : MonoBehaviour
{


    public void zz_Effect_AddAttack(int value)
    {
        
        Services.statsManager.GainTempAttack(value);
    }


    public void zz_Effect_GainArmor(int value)
    {
        
        Services.statsManager.GainArmor(value);
    }

    public void zz_Test_Power_Whenever_Get_Armor(PowerCardFunction card)
    {
        Services.eventManager.Register<PlayerStatsManager.GainArmorEvent>(card.TriggerEffect);
    }

    public void zz_Generate_Card_To_Hand(GameObject card)
    {
        Services.actionManager.GenerateCardAddToHand(card);
    }

    public void zz_Generate_Card_To_DiscardPile(GameObject card)
    {
        Services.actionManager.GenerateCardAddToDiscardPile(card);
    }

    public void zz_Draw_Cards(int amount)
    {
        Services.actionManager.DrawMutipleCard(amount);
    }

    public void zz_Take_Damege(float amount)
    {
        Services.statsManager.TakeDamage(amount);
    }

    public void zz_Gain_Evasion(int stack)
    {
        Services.playerBuffManager.GainNewBuff(new Evasion(),stack);
    }

    public void zz_Give_Enemy_Burn(int stack)
    {
        Services.actionManager.currentTargetEnemy.GainNewBuff(new Burn(), stack);
    }
}
