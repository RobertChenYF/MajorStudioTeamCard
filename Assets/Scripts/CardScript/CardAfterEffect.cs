using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAfterEffect : MonoBehaviour
{
    public void zz_Exhaust(CardFunction card)
    {
        Services.actionManager.AddToExhaustPile(card);
    }
}
