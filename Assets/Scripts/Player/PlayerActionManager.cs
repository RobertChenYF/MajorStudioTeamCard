using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerActionManager : MonoBehaviour
{
    private GameObject Manager;
    [SerializeField]private Enemy tempTestEnemy;
    [HideInInspector]public Enemy currentTargetEnemy;
    public static CardFunction currentDragCard;
    public List<CardFunction> DrawDeck;
    public List<CardFunction> PlayerHand;
    public List<CardFunction> DiscardPile;
    public List<CardFunction> AttackField;
    public List<CardFunction> ExhaustPile;

    [SerializeField] private int playerHandMaxSize;

    [Header("Deck Position")]
    [SerializeField] private Transform discardPile;
    [SerializeField] private Transform drawDeck;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform attackField;
    [SerializeField] private Transform generateCardPos;
    public BoxCollider2D handArea;

    [Header("UI button")]
    [SerializeField]private Button attackButton;

    [Header("Basic action cost")]
    [SerializeField] private float reDrawActionCost;
    [SerializeField] private float attackActionCost;

    private bool canPlayCard;
    [HideInInspector]public bool attacking;
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindWithTag("GameManager");
        DrawDeck = Manager.GetComponent<GameManager>().GlobalPlayerHand;
        PlayerHand = new List<CardFunction>();
        DiscardPile = new List<CardFunction>();
        AttackField = new List<CardFunction>();

        //DrawDeck = new List<CardFunction>();
        currentTargetEnemy = tempTestEnemy;
        TempStart();
        DrawMutipleCard(5);
        attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCardInHandPos();
        UpdateDragCardPos();
        UpdateCardInAttackField();
    }


    public void PlayCard(CardFunction card)
    {
        if (card.CanPlay())
        {
            Services.resourceManager.ConsumeDrawBar(card.GetDrawCost());
            Services.resourceManager.ConsumeAttackBar(card.GetAttackCost());
            card.Played();
            PlayerHand.Remove(card);
            UpdateCardCanPlay();
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
            DrawDeck[0].transform.position = drawDeck.position;
            PlayerHand.Add(DrawDeck[0]);
            DrawDeck[0].gameObject.GetComponent<SortingGroup>().sortingOrder = PlayerHand.IndexOf(DrawDeck[0]);
            //DrawDeck[0].TriggerEffect();
            DrawDeck.RemoveAt(0);
            //trigger add to hand animation;
            Debug.Log("draw a card");

            Services.actionManager.UpdateCardCanPlay();
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
        yield return new WaitForSeconds(0.0f);

        if (times > 1)
        {
            times--;
            StartCoroutine(DrawOneCoroutine(times));
        }


    }

    public void ReDraw()
    {
        if (Services.resourceManager.CheckDrawBar(reDrawActionCost))
        {
            Services.resourceManager.ConsumeDrawBar(reDrawActionCost);
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
        StartCoroutine(MoveFromTo(card.transform, card.transform.position, discardPile.position, 50));
    }

    public void AddToDiscardPile(CardFunction card)
    {
        DiscardPile.Add(card);
        StartCoroutine(MoveFromTo(card.transform, card.transform.position, discardPile.position, 50));
    }

    public void AddToDrawPile(CardFunction card)
    {
        DiscardPile.Add(card);
        //StartCoroutine(MoveFromTo(card.transform, card.transform.position, drawPile.position, 50)); should cards move towards the draw position or just instantly be there?
    }

    public void AddToExhaustPile(CardFunction card)
    {
        ExhaustPile.Add(card);
        StartCoroutine(MoveFromTo(card.transform, card.transform.position, discardPile.position, 50));
    }

    public void DrawPileRefillCard()
    {
        Shuffle(DiscardPile);
        foreach (CardFunction card in DiscardPile)
        {
            card.transform.position = drawDeck.position;
            DrawDeck.Add(card);
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
        if (Services.resourceManager.CheckAttackBar(attackActionCost))
        {

            Services.resourceManager.ConsumeAttackBar(attackActionCost);
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
    }
    public void GenerateCardAddToDiscardPile(GameObject card)
    {
        GameObject newCard = Instantiate(card, generateCardPos.position, Quaternion.identity);
        AddToDiscardPile(newCard.GetComponent<CardFunction>());
    }

    public void GenerateCardAddToDrawPile(GameObject card)
    {
        GameObject newCard = Instantiate(card, generateCardPos.position, Quaternion.identity);
        AddToDrawPile(newCard.GetComponent<CardFunction>());
    }

   public void UpdateCardCanPlay()
    {
        foreach (CardFunction card in PlayerHand)
        {
            card.CanPlay();
        }
    }
}
