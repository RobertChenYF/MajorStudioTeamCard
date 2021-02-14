using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : MonoBehaviour
{

    public PlayerStatsManager statsManager;

    public void zz_Effect_AddAttack(int value)
   {
        Debug.Log("player add attack " + value);
        //statsManager.
    }

    public void zz_Effect_ConsumeDrawBar(int value)
    {

    }

    public void zz_Effect_GainArmor(int value)
    {
        statsManager = GameObject.Find("Player").GetComponent<PlayerStatsManager>();
        statsManager.GainArmor(value);
    }
}
