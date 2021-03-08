using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayRequirement : MonoBehaviour
{
    private bool checkCurrentCanPlay(CardFunction card)
    {
        return card.canBePlayed;
    }

    public void Zz_OneCardInHand(CardFunction card)
    {
        if (checkCurrentCanPlay(card.GetComponent<CardFunction>()))
        {
            if (Services.actionManager.PlayerHand.Count != 1)
            {
                card.GetComponent<CardFunction>().canBePlayed = false;
            }
        }

    }

    public void zz_NoCardInAttackField(CardFunction card)
    {
        if (checkCurrentCanPlay(card.GetComponent<CardFunction>()))
        {
            if (Services.actionManager.AttackField.Count > 0)
            {
                card.GetComponent<CardFunction>().canBePlayed = false;
            }
        }
    }
}
