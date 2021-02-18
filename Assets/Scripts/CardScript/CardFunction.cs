using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CardFunction : MonoBehaviour
{

    protected int instanceId;

    [Header("card basic info")]
    public Card card;
    private float attackCost;
    private float drawCost;
    private Card.CardType cardType;
    private SpriteRenderer splashArt;
    private TextMeshPro nameText;
    private TextMeshPro AttackCostText;
    private TextMeshPro DrawCostText;
    private TextMeshPro effectDiscriptionText;
    private SpriteRenderer backgroundRenderer;
    private Color mouseOverColor = Color.yellow;
    private Color originalColor = Color.white;
    protected PlayerActionManager playerActionManager;
    private float distance;
    private bool dragging = false;


    [Header("effects happen when card play, leave it empty as default")]
    [SerializeField] protected UnityEvent played;

    [Header ("effects happen when card triggers")]
    [SerializeField] protected UnityEvent triggered;

    [Header ("what happen to the card after it is used, leave it empty as default")]
    [SerializeField] protected UnityEvent used;
    

    // Start is called before the first frame update
    void Start()
    {

        MakeCard();
        UpdateCostDisplay();
        instanceId = GetInstanceID();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MakeCard()
    {
        splashArt = transform.Find("splash").GetComponent<SpriteRenderer>();
        nameText = gameObject.transform.Find("CardNameText").GetComponent<TextMeshPro>();
        AttackCostText = transform.Find("AttackCost").GetComponent<TextMeshPro>();
        DrawCostText = transform.Find("DrawCost").GetComponent<TextMeshPro>();
        effectDiscriptionText = transform.Find("CardDescription").GetComponent<TextMeshPro>();
        backgroundRenderer = transform.Find("cardBackground").GetComponent<SpriteRenderer>();
        playerActionManager = GameObject.Find("Player").GetComponent<PlayerActionManager>();
        splashArt.sprite = card.cardSplashArt;
        nameText.text = card.cardName;
        cardType = card.type;
        gameObject.name = card.cardName;
        attackCost = card.attackBarCost;
        drawCost = card.drawBarCost;
        effectDiscriptionText.text = card.cardEffectDiscription;
    }

    private void UpdateCostDisplay()
    {
        AttackCostText.text = attackCost.ToString();
        DrawCostText.text = drawCost.ToString();
    }

    public virtual void Played()
    {
        
        Debug.Log("play " + gameObject.name);
        if (played.GetPersistentEventCount() > 0)
        {

            //trigger additional effect if there is any
            played.Invoke();
        }
        AfterPlayed();
    }

    public virtual void AfterPlayed()
    {

    }

    public virtual void TriggerEffect()
    {
        triggered.Invoke();
        AfterTriggered();
    }

    public virtual void AfterTriggered()
    {
        if (used.GetPersistentEventCount() > 0)
        {
            used.Invoke();
        }
        else
        {
            playerActionManager.AddToDiscardPile(this);
            //add back to the discard pile
        }
    }

    public float GetAttackCost()
    {
        return attackCost;
    }

    public float GetDrawCost()
    {
        return drawCost;
    }

    public Card.CardType GetCardType()
    {
        return cardType;
    }

    void OnMouseEnter()
    {
        if (playerActionManager.InHand(this))
        {
        backgroundRenderer.material.color = mouseOverColor;
        }
        
    }

    void OnMouseExit()
    {
        backgroundRenderer.material.color = originalColor;
    }

    void OnMouseDown()
    {
        if (playerActionManager.InHand(this))
        {
            PlayerActionManager.currentDragCard = this;
        }
        
        
    }

    void OnMouseUp()
    {
        if (PlayerActionManager.currentDragCard = this)
        {
            if (Draggedout())
            {
                playerActionManager.PlayCard(this);
            }
        PlayerActionManager.currentDragCard = null;
        }
        
    }

    private bool Draggedout()
    {
        BoxCollider2D a = playerActionManager.handArea;
        
        if (!a.bounds.Contains(transform.position))
        {
            return true;
        }
        return false;
    }
}
