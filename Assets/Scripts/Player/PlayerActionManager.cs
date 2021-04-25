using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class PlayerActionManager : MonoBehaviour
{
    private GameObject Manager;
    
    [HideInInspector]public Enemy currentTargetEnemy;
    public static CardFunction currentDragCard;
    
    public List<CardFunction> DrawDeck;
    public List<CardFunction> PriorityDeck;
    public List<CardFunction> PlayerHand;
    public List<CardFunction> DiscardPile;
    public List<CardFunction> AttackField;
    public List<CardFunction> DeletePile;

    [SerializeField] private int playerHandMaxSize;

    [Header("Deck Position")]
    [SerializeField] private Transform discardPile;
    [SerializeField] private Transform drawDeck;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform attackField;
    [SerializeField] private Transform generateCardPos;
    [SerializeField] public Transform enemyDefaultPos;
    public BoxCollider2D handArea;

    [Header("UI button")]
    [SerializeField]private Button attackButton;

    [Header("Basic action cost")]
    [SerializeField] private float reDrawActionCost;
    [SerializeField] private float attackActionCost;
    [SerializeField] private TextMeshProUGUI redrawButtonText;
    [SerializeField] private TextMeshProUGUI attackButtonText;
    private float currentRedrawCost;
    private float currentAttackCost;
    private bool canPlayCard;
    [HideInInspector]public bool attacking;

    private void UpdateBasicActionCost(AGPEvent e)
    {
        currentAttackCost -= 5;
        currentRedrawCost -= 5;
        currentRedrawCost = Mathf.Max(currentRedrawCost, 0);
        currentAttackCost = Mathf.Max(currentAttackCost, 0);
        //update visual
        UpdateBasicActionCostDisplay();
    }
    // Start is called before the first frame update
    void Awake()
    {
        DrawDeck = new List<CardFunction>();
        PriorityDeck = new List<CardFunction>();
        PlayerHand = new List<CardFunction>();
        DiscardPile = new List<CardFunction>();
        AttackField = new List<CardFunction>();
        DeletePile = new List<CardFunction>();
        //DrawDeck = new List<CardFunction>();
        
        
        //add all cards from the run deck to draw deck

        
        attacking = false;
        
        currentRedrawCost = reDrawActionCost;
        currentAttackCost = attackActionCost;
        UpdateBasicActionCostDisplay();
    }
    private void Start()
    {
        Services.eventManager.Register<CombatManager.TimeCycleEnd>(UpdateBasicActionCost);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCardInHandPos();
        UpdateDragCardPos();
        UpdateCardInAttackField();
    }


    
    private void UpdateBasicActionCostDisplay()
    {
        attackButtonText.text = "Decompress cost " + currentAttackCost.ToString() + " CPU";
        redrawButtonText.text = "Draw cost " + currentRedrawCost.ToString() + " GPU";
    }
    public void PlayCard(CardFunction card)
    {
        if (currentTargetEnemy == null)
        {
            //Pop up
            Services.visualEffectManager.PlayErrorPopUp("Please select a target first!");
        }
        else if (card.CanPlay())
        {
            Services.resourceManager.ConsumeDrawBar(card.GetDrawCost());
            Services.resourceManager.ConsumeAttackBar(card.GetAttackCost());
            card.Played();
            PlayerHand.Remove(card);
            Services.eventManager.Fire(new PlayerPlayCardEvent(card));
            UpdateCardCanPlay();
        }

    }
    public class PlayerPlayCardEvent : AGPEvent
    {
        public CardFunction thisCard;
        public PlayerPlayCardEvent(CardFunction card)
        {
            thisCard = card;
        }
    }
    public CardFunction DrawCard()
    {
        if (DrawDeck.Count == 0 && DiscardPile.Count > 0)
        {
            DrawPileRefillCard();
        }
        else if (DrawDeck.Count == 0 && DiscardPile.Count == 0)
        {
            //no more card in the draw deck no more card in the discard pile
            Debug.Log("no more card in the draw deck no more card in the discard pile");
            return null;
        }

        if (PlayerHand.Count <= playerHandMaxSize - 1)
        {
            int index = 0;
            if (PriorityDeck.Count > 0)
            {
                index = DrawDeck.IndexOf(PriorityDeck[0]);
                PriorityDeck.RemoveAt(0);
            }
            CardFunction temp = DrawDeck[index];
            DrawDeck[index].transform.position = drawDeck.position;
            PlayerHand.Add(DrawDeck[index]);
            DrawDeck[index].gameObject.GetComponent<SortingGroup>().sortingOrder = PlayerHand.IndexOf(DrawDeck[index]);
            //DrawDeck[0].TriggerEffect();
            DrawDeck.RemoveAt(index);
            //trigger add to hand animation;
            Debug.Log("draw a card");

            Services.actionManager.UpdateCardCanPlay();
            return temp;
        }
        else
        {
            Debug.Log("hand is full");
            return null;
        }
    }
    public void DrawMutipleCard(int times)
    {
        StartCoroutine(DrawOneCoroutine(times));
    }

    IEnumerator DrawOneCoroutine(int times)
    {
        DrawCard();
        yield return new WaitForSeconds(0.0f);

        if (times > 1)
        {
            times--;
            StartCoroutine(DrawOneCoroutine(times));
        }


    }

    public void ReDraw()
    {
        if (Services.resourceManager.CheckDrawBar(currentRedrawCost))
        {
            Services.resourceManager.ConsumeDrawBar(currentRedrawCost);
            while (PlayerHand.Count > 0)
            {
                MoveFromHandToDiscardPile(PlayerHand[0]);
            }

            DrawMutipleCard(5);
            Services.eventManager.Fire(new RedrawEvent());
        }

        currentRedrawCost = reDrawActionCost;
        UpdateBasicActionCostDisplay();
    }

    public class RedrawEvent : AGPEvent
    {
        public RedrawEvent()
        {

        }
    }
    public void MoveFromHandToDiscardPile(CardFunction card)
    {

        PlayerHand.Remove(card);
        DiscardPile.Add(card);
        StartCoroutine(MoveFromTo(card.transform, card.transform.position, discardPile.position, 50));
    }

    public void AddToDiscardPile(CardFunction card)
    {
        DiscardPile.Add(card);
        StartCoroutine(MoveFromTo(card.transform, card.transform.position, discardPile.position, 50));
    }

    public void AddToDrawPile(CardFunction card)
    {
        if (card.getKeywords() != null && card.getKeywords().Contains(Card.Keywords.priority))
        {
            Debug.Log("priority");
            PriorityDeck.Add(card);
        }
        DrawDeck.Add(card);
        card.gameObject.transform.position = drawDeck.position;
        //StartCoroutine(MoveFromTo(card.transform, card.transform.position, drawPile.position, 50)); should cards move towards the draw position or just instantly be there?
    }

    public void AddToExhaustPile(CardFunction card)
    {
        DeletePile.Add(card);
        StartCoroutine(MoveFromTo(card.transform, card.transform.position, discardPile.position, 50));
    }

    public void DrawPileRefillCard()
    {
        Shuffle(DiscardPile);
        foreach (CardFunction card in DiscardPile)
        {
            card.transform.position = drawDeck.position;
            AddToDrawPile(card);
        }
        DiscardPile.Clear();
    }

    public void TempStart()
    {
        foreach (CardFunction card in DrawDeck)
        {
            card.transform.position = drawDeck.position;
            card.gameObject.SetActive(true);
        }
        Shuffle(DrawDeck);
    }

    public void UpdateCardInHandPos()
    {
        foreach (CardFunction card in PlayerHand)
        {
            if (currentDragCard != card)
            {
                Vector3 target = hand.position + PlayerHand.IndexOf(card) * new Vector3(1, 0, 0);
                
                card.transform.position = Vector3.MoveTowards(card.transform.position, target, Time.deltaTime * Mathf.Max(Vector3.Distance(card.transform.position, target) * 2, 5));
            }

        }
    }
    public void UpdateCardInAttackField()
    {
        foreach (CardFunction card in AttackField)
        {

            Vector3 target = attackField.position + AttackField.IndexOf(card) * new Vector3(1, 0, 0);
            card.gameObject.GetComponent<SortingGroup>().sortingOrder = AttackField.IndexOf(card);
            card.transform.position = Vector3.MoveTowards(card.transform.position, target, Time.deltaTime * Mathf.Max(Vector3.Distance(card.transform.position, target) * 5, 8));

        }
    }
    public void UpdateDragCardPos()
    {
        if (currentDragCard != null)
        {
            currentDragCard.BringUpOrderInLayer();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(Vector3.Distance(currentDragCard.gameObject.transform.position, Camera.main.transform.position));
            currentDragCard.gameObject.transform.position = rayPoint;
        }

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
    IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed)
    {
        float step = (speed / (a - b).magnitude) * Time.deltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return null;         // Leave the routine and return here in the next frame
        }
        objectToMove.position = b;
    }
    public void AddToAttackField(CardFunction card)
    {
        AttackField.Add(card);
    }

    public int InHand(CardFunction card)
    {
        return PlayerHand.IndexOf(card);
    }
    public void StartAttack()
    {
        if (currentTargetEnemy == null)
        {
            //Pop up
            Services.visualEffectManager.PlayErrorPopUp("Please select a target first!");
        }
        else if (Services.resourceManager.CheckAttackBar(currentAttackCost))
        {

            Services.resourceManager.ConsumeAttackBar(currentAttackCost);
            Services.combatManager.PauseTimeCycle();
            attackButton.interactable = false;
            attacking = true;
            StartCoroutine(AttackCoroutine());
            
        }
        UpdateCardCanPlay();
        Debug.Log("attack");
    }
    IEnumerator AttackCoroutine()
    {
        while (AttackField.Count != 0)
        {
            CardFunction card = AttackField[0];
            card.TriggerEffect();
            AttackField.RemoveAt(0);
            yield return new WaitForSeconds(1.0f);
        }
        
        Debug.Log("end attack");
        //currentTargetEnemy.TakeDamage(Services.statsManager.GetCurrentAttackDmg());
        Services.combatManager.ContinueTimeCycle();
        attacking = false;
        //Services.statsManager.LoseAllTempAttack();
        currentAttackCost = attackActionCost;
        UpdateBasicActionCostDisplay();
        attackButton.interactable = true;

        UpdateCardCanPlay();
        yield return null;
    }

    public void AddToHand(CardFunction card)
    {
        if (PlayerHand.Count <= playerHandMaxSize - 1)
        {
            PlayerHand.Add(card);
            card.gameObject.GetComponent<SortingGroup>().sortingOrder = PlayerHand.IndexOf(card);
        }
        else
        {
            AddToDiscardPile(card);
        }
    }

    public void GenerateCardAddToHand(GameObject card)
    {
        GameObject newCard = Instantiate(card, generateCardPos.position, Quaternion.identity);
        AddToHand(newCard.GetComponent<CardFunction>());
        newCard.GetComponent<CardFunction>().generated = true;
        newCard.transform.SetParent(Services.runStateManager.AllCardsInGame.transform);
    }
    public void GenerateCardAddToDiscardPile(GameObject card)
    {
        GameObject newCard = Instantiate(card, generateCardPos.position, Quaternion.identity);
        AddToDiscardPile(newCard.GetComponent<CardFunction>());
        newCard.GetComponent<CardFunction>().generated = true;
        newCard.transform.SetParent(Services.runStateManager.AllCardsInGame.transform);
    }

    public void GenerateCardAddToDrawPile(GameObject card)
    {
        GameObject newCard = Instantiate(card, generateCardPos.position, Quaternion.identity);
        AddToDrawPile(newCard.GetComponent<CardFunction>());
        newCard.GetComponent<CardFunction>().generated = true;
        newCard.transform.SetParent(Services.runStateManager.AllCardsInGame.transform);
        Shuffle(DrawDeck);
    }

   public void UpdateCardCanPlay()
    {
        foreach (CardFunction card in PlayerHand)
        {
            card.CanPlay();
        }
    }
}
