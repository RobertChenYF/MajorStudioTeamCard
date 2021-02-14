using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerActionManager : MonoBehaviour
{

    private PlayerResourceManager resourceManager;
    public List<CardFunction> DrawDeck;
    public List<CardFunction> PlayerHand;
    public List<CardFunction> DiscardPile;
    public List<CardFunction> AttackField;

    [SerializeField] private int playerHandMaxSize;
    // Start is called before the first frame update
    void Start()
    {
        resourceManager = GetComponent<PlayerResourceManager>();
        //DrawDeck = new List<CardFunction>();
        PlayerHand = new List<CardFunction>();
        DiscardPile = new List<CardFunction>();
        AttackField = new List<CardFunction>();
        DrawMutipleCard(3);
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

    public bool DrawCard()
    {
        if (DrawDeck.Count == 0 && DiscardPile.Count > 0)
        {
            DrawPileRefillCard();
        }
        else if (DrawDeck.Count == 0 && DiscardPile.Count == 0)
        {
            //no more card in the draw deck no more card in the discard pile
            Debug.Log("no more card in the draw deck no more card in the discard pile");
            return false;
        }

        if (PlayerHand.Count <= playerHandMaxSize - 1)
        {
            PlayerHand.Add(DrawDeck[0]);
            //DrawDeck[0].TriggerEffect();
            DrawDeck.RemoveAt(0);
            //trigger add to hand animation;
            Debug.Log("draw a card");
            return true;
        }
        else
        {
            Debug.Log("hand is full");
            return false;
        }
    }
    public void DrawMutipleCard(int times)
    {
        StartCoroutine(DrawOneCoroutine(times));
    }

    IEnumerator DrawOneCoroutine(int times)
    {
        DrawCard();
        yield return new WaitForSeconds(0.5f);
        
        if (times > 1)
        {
            times--;
            StartCoroutine(DrawOneCoroutine(times));
        }
        
        
    }

    public void ReDraw()
    {
        if (resourceManager.CheckDrawBar(10))
        {
            resourceManager.ConsumeDrawBar(10);
        while (PlayerHand.Count > 0)
        {
            MoveFromHandToDiscardPile(PlayerHand[0]);
        }
        DrawMutipleCard(5);
        }

        
    }
    public void MoveFromHandToDiscardPile(CardFunction card)
    {
        PlayerHand.Remove(card);
        DiscardPile.Add(card);
    }
    public void DrawPileRefillCard()
    {
        Shuffle(DiscardPile);
        foreach (CardFunction card in DiscardPile)
        {
            DrawDeck.Add(card);
        }
        DiscardPile.Clear();
    }

    public void Shuffle(List<CardFunction> cards)
    {

        for (int i = 0; i < cards.Count; i++)
        {
            int rnd = Random.Range(0, cards.Count);
            Swap(cards, i, rnd);
        }

    }

    public void Swap<T>(List<T> list, int i, int j)
    {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }

}
