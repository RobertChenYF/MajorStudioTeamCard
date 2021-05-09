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
    [SerializeField]private GameObject wipe;

    private bool dedicate(Card.CardCompany company)
    {
        CardFunction temp = Services.actionManager.DrawCard();
        if (temp != null)
        {
            return (temp.getCompany() == company);
        }
        return false;
        
    }

    public void CheckSequence(CardFunction card)
    {
        if (Services.actionManager.AttackField.Count >= 3)
        {
            card.sequence = true;
        }
    }

    public void CheckInitial(CardFunction card)
    {
        if (Services.actionManager.AttackField.Count == 0)
        {
            card.initial = true;
        }
    }

    public void zz_Effect_BatchedWipe(CardFunction card)
    {
        if (card.sequence)
        {
            card.sequence = false;
            for (int i = 0; i < Services.actionManager.AttackField.Count; i ++)
            {
                Services.actionManager.GenerateCardAddToHand(wipe);
            }
        }
    }

    public void zz_Effect_ChangeIP(CardFunction card)
    {
        if (dedicate(card.getCompany()))
        {
            Services.actionManager.LowerDecompressCost(10);
        }
    }

    public void zz_Effect_Heal(int amount)
    {
        Services.statsManager.GainHp(amount);
    }

    public void zz_Effect_DeepBurn()
    {
        zz_Deal_Damage(5);
        if (Services.actionManager.currentTargetEnemy != null&&Services.actionManager.currentTargetEnemy.CheckBuff(new Burn()) != -1)
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

    public void zz_On_CodePolymorpher(int stack)
    {
        Services.playerBuffManager.GainNewBuff(new Polymorpher(),stack);
    }

    public void zz_CopyAndPaste(CardFunction card)
    {
        if (dedicate(card.getCompany()))
        {
            Services.playerBuffManager.GainNewBuff(new CopyAndPaste(),1);
        }
    }
    public void zz_DefenseProtocol()
    {
        Services.playerBuffManager.GainNewBuff(new DefenseProtocol(),1);
    }

    public void zz_DiskStorage(CardFunction card)
    {
        if (dedicate(card.getCompany()))
        {
            Services.actionManager.GenerateCardAddToHand(wipe);
        }
    }

    public void zz_DoubleDip()
    {
        if (Services.actionManager.currentRedrawCost <= 0)
        {
            zz_Draw_Cards(2);
        }
    }

    public void zz_EnergyConserver()
    {
        Services.playerBuffManager.GainNewBuff(new EnergyConserver(),1);
    }

    public void zz_FileRecovery(CardFunction card)
    {
        if (dedicate(card.getCompany()))
        {
            Services.statsManager.GainHp(8);
        }
    }

    public void zz_Lemon()
    {
        Services.playerBuffManager.GainNewBuff(new Lemon(), 1);
    }

    public void zz_initialDraw2(CardFunction card)
    {
        if (card.initial)
        {
            zz_Draw_Cards(2);
            card.initial = false;
        }
    }

    public void zz_OffenseUplink(CardFunction card)
    {
        if (card.sequence)
        {
            zz_Deal_Damage(8);
            card.sequence = false;
        }
    }

    public void zz_SecurityUplink(CardFunction card)
    {
        if (card.sequence)
        {
            zz_Effect_GainArmor(10);
            card.sequence = false;
        }
    }

    public void zz_initialDealExtra(CardFunction card)
    {
        if (card.initial)
        {
            zz_Deal_Damage(2*Services.actionManager.AttackField.Count);
            card.initial = false;
        }
    }

    public void zz_RefreshDirectory()
    {
        zz_Draw_Cards(Services.actionManager.AttackField.Count);
    }

    public void zz_RiskyArchive(CardFunction card)
    {
        if (card.initial)
        {
            zz_GainRedMana(50);
            card.initial = false;
        }
        else
        {
            zz_GainRedMana(20);
        }
    }

    public void zz_UpdateSecurity()
    {
        Services.playerBuffManager.GainNewBuff(new UpdateSecurity(),1);
    }
}
