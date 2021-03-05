using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBuffManager : MonoBehaviour
{
    public List<PlayerBuff> currentPlayerBuff;
    public TextMeshPro playerBuffTextDisplay;
    // Start is called before the first frame update
    void Start()
    {
        currentPlayerBuff = new List<PlayerBuff>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentPlayerBuff.Count);
        playerBuffTextDisplay.text = "";
        foreach (PlayerBuff a in currentPlayerBuff)
        {
            playerBuffTextDisplay.text += a.tempString();
        }
    }

    public void GainNewBuff(PlayerBuff newBuff, int stack)
    {
        if (CheckBuff(newBuff) == -1)
        {
        currentPlayerBuff.Add(newBuff);
        newBuff.ActivateBuff();
        newBuff.GainStack(stack - 1);
        }
        else
        {
            currentPlayerBuff[CheckBuff(newBuff)].GainStack(stack);
        }
        
    }
    public void RemoveBuff(PlayerBuff buff)
    {
        currentPlayerBuff.Remove(buff);
        buff.DeactivateBuff();
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
}

abstract public class Buff{

    protected Sprite buffIcon;
    protected int stack = 1;
    protected string buffName;
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
}

public class GainAttackWhenGainArmor : PlayerBuff
{

}

public class Evasion : PlayerBuff
{
    
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

