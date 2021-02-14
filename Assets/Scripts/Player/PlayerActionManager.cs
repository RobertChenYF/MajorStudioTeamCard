using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerActionManager : MonoBehaviour
{

    private PlayerResourceManager resourceManager;
    public List<CardFunction> DrawDeck;
    public List<CardFunction> PlayerHand;
    public List<CardFunction> DiscardPile;
    public List<CardFunction> AttackField;

    // Start is called before the first frame update
    void Start()
    {
        DrawDeck = new List<CardFunction>();
        PlayerHand = new List<CardFunction>();
        DiscardPile = new List<CardFunction>();
        AttackField = new List<CardFunction>();
    }

    // Update is called once per frame
    void Update()
    {

    }

  
    public void PlayCard(CardFunction card)
    {
        if (resourceManager.CheckAttackBar(card.GetAttackCost()) && resourceManager.CheckDrawBar(card.GetDrawCost()))
        {
            card.Played();
        }
        
    }
}
