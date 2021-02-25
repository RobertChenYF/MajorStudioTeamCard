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
            if (PlayerActionManager.instance.PlayerHand.Count != 1)
            {
                card.GetComponent<CardFunction>().canBePlayed = false;
            }
        }

    }
}
