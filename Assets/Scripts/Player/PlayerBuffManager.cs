using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBuffManager : MonoBehaviour
{
    public List<PlayerBuff> currentPlayerBuff;
    public List<BuffHoverDisplay> buffDisplay;
    [SerializeField]private GameObject buffDisplayPrefab;
    public Transform playerBuffDisplay;
    public Sprite EvasionBuffIcon;
    public Sprite BurnBuffIcon;
    public Sprite PlaceHolderBuffIcon;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        currentPlayerBuff = new List<PlayerBuff>();
        buffDisplay = new List<BuffHoverDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //playerBuffTextDisplay.text = "";
        foreach (PlayerBuff a in currentPlayerBuff)
        {
           // playerBuffTextDisplay.text += a.tempString();
        }
    }

    public void GainNewBuff(PlayerBuff newBuff, int stack)
    {
        if (CheckBuff(newBuff) == -1)
        {
        currentPlayerBuff.Add(newBuff);
        newBuff.ActivateBuff();
        newBuff.GainStack(stack - 1);
            UpdateBuffDisplay();
        }
        else
        {
            currentPlayerBuff[CheckBuff(newBuff)].GainStack(stack);
            UpdateBuffDisplay();
        }
        
    }
    public void RemoveBuff(PlayerBuff buff)
    {
        currentPlayerBuff.Remove(buff);
        buff.DeactivateBuff();
        UpdateBuffDisplay();
    }

    public void RemoveAllBuff()
    {
        while(currentPlayerBuff.Count > 0)
        {
            RemoveBuff(currentPlayerBuff[0]);
        }
    }

    public int CheckBuff(PlayerBuff a)
    {
        foreach (PlayerBuff b in currentPlayerBuff)
        {
            if (b.ToString().Equals(a.ToString()))
            {
                return currentPlayerBuff.IndexOf(b);
            }
        }
        return -1;
    }

    public void UpdateBuffDisplay()
    {
        foreach (BuffHoverDisplay display in buffDisplay)
        {
            Destroy(display.gameObject);
        }
        buffDisplay.Clear();
        foreach (PlayerBuff a in currentPlayerBuff)
        {
            GameObject newBuff = Instantiate(buffDisplayPrefab,playerBuffDisplay.transform.position+ new Vector3(0.5f*currentPlayerBuff.IndexOf(a),0,0),Quaternion.identity,playerBuffDisplay);
            newBuff.GetComponent<BuffHoverDisplay>().thisBuff = a;
            newBuff.GetComponent<BuffHoverDisplay>().MakeBuff();
            newBuff.GetComponent<BuffHoverDisplay>().UpdateCount(a.getStack());
            buffDisplay.Add(newBuff.GetComponent<BuffHoverDisplay>());
        }
    }
}

 public class Buff{

    protected Sprite buffIcon = Services.playerBuffManager.PlaceHolderBuffIcon;
    public Sprite getBuffIcon()
    {
        return buffIcon;
    }
    protected int stack = 1;
    public int getStack()
    {
        return stack;
    }
    protected string buffName;
    [SerializeField]protected string buffDescription;
    public string getBuffDescription()
    {
        return buffDescription;
    }
    public string tempString()
    {
        return this.ToString() + " stack: " + stack.ToString() + "\n";
    }

    public virtual void ActivateBuff()
    {
        
    }
    public void GainStack(int amount)
    {
        stack = stack + amount;
    }

    public virtual void TriggerEffect()
    {
        
    }

    public virtual void DeactivateBuff()
    {

    }

}

public class PlayerBuff : Buff
{
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        if (stack == 0)
        {
            Services.playerBuffManager.RemoveBuff(this);
        }
    }

    public void LoseStack()
    {
        stack--;
        if (stack <= 0)
        {
            Services.playerBuffManager.RemoveBuff(this);
        }
    }
}

public class GainAttackWhenGainArmor : PlayerBuff
{

}

public class AdditionalCardwhenRedraw : PlayerBuff
{
    public AdditionalCardwhenRedraw()
    {
        buffDescription = "draw 1 additional card when redraw";
    }
    public override void ActivateBuff()
    {
        base.ActivateBuff();
        Services.eventManager.Register<PlayerActionManager.RedrawEvent>(TriggerEffect);
    }

    public void TriggerEffect(AGPEvent e)
    {
        for (int i = 0; i < stack; i++)
        {
            Services.actionManager.DrawCard();
        }
        
    }
    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        Services.eventManager.Unregister<PlayerActionManager.RedrawEvent>(TriggerEffect);
    }
}

public class Overclock : PlayerBuff
{
    public Overclock()
    {
        buffDescription = "gain 2 green manas every cycle ends";
    }
    public override void ActivateBuff()
    {
        base.ActivateBuff();
        Services.eventManager.Register<CombatManager.TimeCycleEnd>(TriggerEffect);
    }
    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        Services.eventManager.Unregister<CombatManager.TimeCycleEnd>(TriggerEffect);
    }
    public void TriggerEffect(AGPEvent e)
    {
        Services.resourceManager.GainDrawBar(stack * 2);
    }
}
public class Spam : PlayerBuff
{
    public Spam()
    {
        //icon
        buffDescription = "card cost 5 or less trigger twice";
    }

    public override void ActivateBuff()
    {
        base.ActivateBuff();
        Services.eventManager.Register<CardFunction.CardTriggerEvent>(TriggerEffect);
    }

    public void TriggerEffect(AGPEvent e)
    {
        CardFunction.CardTriggerEvent a = (CardFunction.CardTriggerEvent)e;
        if ((a.thisCard.GetAttackCost() <= 5 && a.thisCard.GetDrawCost() == 0)||(a.thisCard.GetAttackCost() == 0 && a.thisCard.GetDrawCost() <= 5))
        {
            a.thisCard.JustTriggerEffect();
        }
    }

    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        Services.eventManager.Unregister<CardFunction.CardTriggerEvent>(TriggerEffect);
    }
}

public class UpdateSecurity : PlayerBuff
{
    public UpdateSecurity()
    {
        //icon
        buffDescription = "draw a card whenevery you play a generated card";
    }

    public override void ActivateBuff()
    {
        base.ActivateBuff();
        Services.eventManager.Register<PlayerActionManager.PlayerPlayCardEvent>(TriggerEffect);
    }

    public void TriggerEffect(AGPEvent e)
    {
        PlayerActionManager.PlayerPlayCardEvent a = (PlayerActionManager.PlayerPlayCardEvent)e;
        if (a.thisCard.generated)
        {
            Services.actionManager.DrawMutipleCard(stack);
        }
    }

    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        Services.eventManager.Unregister<PlayerActionManager.PlayerPlayCardEvent>(TriggerEffect);
    }
}

public class CopyAndPaste : PlayerBuff
{
    public CopyAndPaste()
    {
        //icon
        buffDescription = "refund the cost of the next card you play";
    }

    public override void ActivateBuff()
    {
        base.ActivateBuff();
        Services.eventManager.Register<PlayerActionManager.PlayerPlayCardEvent>(TriggerEffect);
    }

    public void TriggerEffect(AGPEvent e)
    {
        PlayerActionManager.PlayerPlayCardEvent a = (PlayerActionManager.PlayerPlayCardEvent)e;
        if (a.thisCard.GetAttackCost() > 0)
        {
            Services.resourceManager.GainAttackBar(a.thisCard.GetAttackCost());
        }
        else if(a.thisCard.GetDrawCost() > 0)
        {
            Services.resourceManager.GainDrawBar(a.thisCard.GetDrawCost());
        }
        this.LoseStack();
    }

    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        Services.eventManager.Unregister<PlayerActionManager.PlayerPlayCardEvent>(TriggerEffect);
    }
}


public class Evasion : PlayerBuff
{
   
    public Evasion(){

        buffIcon = Services.playerBuffManager.EvasionBuffIcon;
        buffDescription = "the next time you take damage take 0 damage and lose a stack of this";

    }
    public override void ActivateBuff()
    {
        Services.statsManager.TakeDamageEvent.AddListener(TriggerEffect);
    }



    public override void TriggerEffect()
    {
        stack--;
        Services.statsManager.currentDamageAmount = 0;
        base.TriggerEffect();
    }

    public override void DeactivateBuff()
    {
        Services.statsManager.TakeDamageEvent.RemoveListener(TriggerEffect);
    }

}

public class DefenseProtocol : PlayerBuff
{

    public DefenseProtocol()
    {

        //buffIcon = Services.playerBuffManager.EvasionBuffIcon;
        buffDescription = "every time you take damage, add a <i>Wipe</i> to your hand";

    }
    public override void ActivateBuff()
    {
        Services.statsManager.TakeDamageEvent.AddListener(TriggerEffect);
    }



    public override void TriggerEffect()
    {
        for (int i = 0; i < stack; i ++)
        {
            Services.actionManager.GenerateCardAddToHand(Services.cardList.cardWipe);
        }
        
        base.TriggerEffect();
    }

    public override void DeactivateBuff()
    {
        Services.statsManager.TakeDamageEvent.RemoveListener(TriggerEffect);
    }

}

public class Polymorpher : PlayerBuff
{

    public Polymorpher()
    {

        //buffIcon = Services.playerBuffManager.EvasionBuffIcon;
        buffDescription = "every time you take damage heal for 2";

    }
    public override void ActivateBuff()
    {
        Services.eventManager.Register<PlayerStatsManager.PlayerTakeDamageEvent>(TriggerEffect);
    }



    public void TriggerEffect(AGPEvent e)
    {
        Services.statsManager.GainHp(2*stack);
        base.TriggerEffect();
    }

    public override void DeactivateBuff()
    {
        Services.eventManager.Unregister<PlayerStatsManager.PlayerTakeDamageEvent>(TriggerEffect);
    }


}

public class EndProcess : PlayerBuff
{

    public EndProcess()
    {

        //buffIcon = Services.playerBuffManager.EvasionBuffIcon;
        buffDescription = "<b>delete</b> the next card you play";

    }
    public override void ActivateBuff()
    {
        Services.eventManager.Register<PlayerStatsManager.PlayerTakeDamageEvent>(TriggerEffect);
    }



    public void TriggerEffect(AGPEvent e)
    {
        Services.statsManager.GainHp(2 * stack);
        base.TriggerEffect();
    }

    public override void DeactivateBuff()
    {
        Services.eventManager.Unregister<PlayerStatsManager.PlayerTakeDamageEvent>(TriggerEffect);
    }


}

public class EnergyConserver : PlayerBuff
{
    int conservation = 0;
    public EnergyConserver()
    {

        //buffIcon = Services.playerBuffManager.EvasionBuffIcon;
        buffDescription = "gain a Conservation every time you play a card, heal 2 for each Conservation at the end of the combat";

    }
    public override void ActivateBuff()
    {
        Services.eventManager.Register<PlayerActionManager.PlayerPlayCardEvent>(gainStack);
        Services.eventManager.Register<Combat.CombatEndEvent>(TriggerEffect);
    }

    public void gainStack(AGPEvent e)
    {
        conservation++;
    }


    public void TriggerEffect(AGPEvent e)
    {
        Services.statsManager.GainHp(2 * stack * conservation);
        base.TriggerEffect();
    }

    public override void DeactivateBuff()
    {
        Services.eventManager.Unregister<PlayerActionManager.PlayerPlayCardEvent>(gainStack);
        Services.eventManager.Unregister<Combat.CombatEndEvent>(TriggerEffect);
    }


}

public class Lemon : PlayerBuff
{
    
    public Lemon()
    {

        //buffIcon = Services.playerBuffManager.EvasionBuffIcon;
        buffDescription = "take 1 damage every time cycle, gain 5 CPU everytime you take damage";

    }
    public override void ActivateBuff()
    {
        Services.eventManager.Register<CombatManager.TimeCycleEnd>(take1Damage);
        Services.eventManager.Register<PlayerStatsManager.PlayerTakeDamageEvent>(TriggerEffect);
    }

    public void take1Damage(AGPEvent e)
    {
        Services.statsManager.TakeDamage(1*stack);
    }


    public void TriggerEffect(AGPEvent e)
    {
        Services.resourceManager.GainAttackBar(stack*5);
        base.TriggerEffect();
    }

    public override void DeactivateBuff()
    {
        Services.eventManager.Unregister<CombatManager.TimeCycleEnd>(take1Damage);
        Services.eventManager.Unregister<PlayerStatsManager.PlayerTakeDamageEvent>(TriggerEffect);
    }


}

public class Technician : PlayerBuff
{
    public void TriggerEffect(AGPEvent e)
    {
        Services.statsManager.GainArmor(2 * stack);
        base.TriggerEffect();
    }

    public override void ActivateBuff()
    {
        Services.eventManager.Register<CombatManager.TimeCycleEnd>(TriggerEffect);
        base.ActivateBuff();
    }

    public void DeactivateBuff(AGPEvent e)
    {
        Services.eventManager.Unregister<CombatManager.TimeCycleEnd>(TriggerEffect);
        base.DeactivateBuff();
    }
}

public class Manager : PlayerBuff
{
    public void TriggerEffect(AGPEvent e)
    {
        //Services.statsManager.GainArmor(2 * stack);
        base.TriggerEffect();
    }

    public override void ActivateBuff()
    {
        //Services.eventManager.Register<CombatManager.TimeCycleEnd>(TriggerEffect);
        base.ActivateBuff();
    }

    public void DeactivateBuff(AGPEvent e)
    {
        //Services.eventManager.Unregister<CombatManager.TimeCycleEnd>(TriggerEffect);
        base.DeactivateBuff();
    }
}

