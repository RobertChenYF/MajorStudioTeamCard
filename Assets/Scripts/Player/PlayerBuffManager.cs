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
    void Start()
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
            GameObject newBuff = Instantiate(buffDisplayPrefab,playerBuffDisplay.transform.position,Quaternion.identity,playerBuffDisplay);
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
}

public class GainAttackWhenGainArmor : PlayerBuff
{

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

