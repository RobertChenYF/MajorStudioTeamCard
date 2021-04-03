using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : MonoBehaviour
{

    /*
    public void zz_Effect_AddAttack(int value)
    {
        
        Services.statsManager.GainTempAttack(value);
    }
    */


    private bool dedicate(Card.CardCompany company)
    {
        CardFunction temp = Services.actionManager.DrawCard();
        if (temp != null)
        {
            return (temp.getCompany() == company);
        }
        return false;
        
    }
    public void zz_Effect_DeepBurn()
    {
        zz_Deal_Damage(5);
        if (Services.actionManager.currentTargetEnemy.CheckBuff(new Burn()) != -1)
        {
            zz_Deal_Damage(5);
        }
    }
    public void zz_Dedicate_harden(CardFunction card)
    {
        if (dedicate(card.getCompany()))
        {
            Services.playerBuffManager.GainNewBuff(new Evasion(), 1);
        }
        else
        {
            Services.statsManager.GainArmor(6);
        }
    }

    public void zz_Dedicate_burn(CardFunction card)
    {
        if (dedicate(card.getCompany()))
        {
            Services.actionManager.currentTargetEnemy.GainNewBuff(new Burn(), 8);
        }
        else
        {
            Services.actionManager.currentTargetEnemy.GainNewBuff(new Burn(), 4);
        }
    }

    public void zz_Deal_Damage(int amount)
    {
        if (Services.actionManager.currentTargetEnemy != null)
        {
            Services.actionManager.currentTargetEnemy.TakeDamage(amount);
        }
        
    }

    public void zz_Effect_GainArmor(int value)
    {
        
        Services.statsManager.GainArmor(value);
    }
    public void zz_Effect_ScanAndBan()
    {
        int a = Services.actionManager.currentTargetEnemy.CheckBuff(new Burn());
        if ( a != -1)
        {
            int stack = Services.actionManager.currentTargetEnemy.enemyBuffList[a].getStack();
            zz_Deal_Damage(stack * 2);
            Services.actionManager.currentTargetEnemy.RemoveBuff(Services.actionManager.currentTargetEnemy.enemyBuffList[a]);
        }
    }
    public void zz_GainRedMana(float amount)
    {
        Services.resourceManager.GainAttackBar(amount);
    }
    public void zz_GainBlueMana(float amount)
    {
        Services.resourceManager.GainDrawBar(amount);
    }
    public void zz_Effect_GainSpamPower()
    {
        Services.playerBuffManager.GainNewBuff(new Spam(), 1);
    }
    public void zz_Generate_Card_To_Hand(GameObject card)
    {
        Services.actionManager.GenerateCardAddToHand(card);
    }

    public void zz_Generate_Card_To_DiscardPile(GameObject card)
    {
        Services.actionManager.GenerateCardAddToDiscardPile(card);
    }

    public void zz_Generate_Card_To_DrawPile(GameObject card)
    {
        Services.actionManager.GenerateCardAddToDrawPile(card);
    }

    public void zz_Draw_Cards(int amount)
    {
        Services.actionManager.DrawMutipleCard(amount);
    }

    public void zz_Take_Damage(float amount)
    {
        Services.statsManager.TakeDamage(amount);
    }

    public void zz_Gain_Evasion(int stack)
    {
        
        Services.playerBuffManager.GainNewBuff(new Evasion(),stack);
    }
    public void zz_GainOverclocking()
    {
        Services.playerBuffManager.GainNewBuff(new Overclock(),1);
    }

    public void zz_armorAdditionalDraw(int count)
    {
        if (Services.statsManager.getArmor() > 0.0f)
        {
            Services.actionManager.DrawMutipleCard(count);
        }
    }
    public void zz_GainAdditionalRedraw()
    {
        Services.playerBuffManager.GainNewBuff(new AdditionalCardwhenRedraw(),1);
    }
    public void zz_reallocateMemory(CardFunction card)
    {
        zz_Deal_Damage(7);
        if (dedicate(card.getCompany()))
        {
            Services.resourceManager.GainAttackBar(15);
        }
    }
    public void zz_Give_Enemy_Burn(int stack)
    {
        if (Services.actionManager.currentTargetEnemy != null)
        {
            Services.actionManager.currentTargetEnemy.GainNewBuff(new Burn(), stack);
            int a = Services.actionManager.currentTargetEnemy.CheckBuff(new Burn());
            Burn temp = (Burn)Services.actionManager.currentTargetEnemy.enemyBuffList[a];
            temp.TakeDamage();
        }
        
    }

    public void zz_Gain_Technician(int stack)
    {
        Services.playerBuffManager.GainNewBuff(new Technician(), stack);
    }

    public void zz_Gain_Manager(int stack)
    {
        Services.playerBuffManager.GainNewBuff(new Manager(), stack);
    }
    public void zz_Effect_bitshift()
    {
        int a = Services.playerBuffManager.CheckBuff(new Evasion());
        if (a != -1)
        {
            Services.playerBuffManager.currentPlayerBuff[a].LoseStack();
            zz_Effect_GainArmor(20);
        }
    }
    public void zz_On_Hold_Scenario(int stack)
    {
        //Services.playerBuffManager.GainNewBuff(new Hold_Scenario(), stack);
    }

}
