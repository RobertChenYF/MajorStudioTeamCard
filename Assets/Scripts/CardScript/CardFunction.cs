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
    
 
    [HideInInspector]public bool canBePlayed;

    [Header("requirement fot the card to get played")]
    [SerializeField] protected UnityEvent playRequirement;

    [Header("effects happen when card play, leave it empty as default")]
    [SerializeField] protected UnityEvent played;

    [Header ("effects happen when card triggers")]
    [SerializeField] protected UnityEvent triggered;

    [Header ("what happen to the card after it is used, leave it empty as default")]
    [SerializeField] protected UnityEvent used;

    [HideInInspector]public GameObject linkedCardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        linkedCardPrefab = gameObject;
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

    public virtual bool CanPlay()
    {
        canBePlayed = true;
        playRequirement.Invoke();
        //Debug.Log(PlayerResourceManager.instance.CheckDrawBar(drawCost));

        canBePlayed = (canBePlayed&& Services.resourceManager.CheckDrawBar(drawCost)&&Services.resourceManager.CheckAttackBar(attackCost));
        return (canBePlayed);
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
        else if(used.GetPersistentEventCount() == 0)
        {
            Services.actionManager.AddToDiscardPile(this);
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
        if (Services.actionManager.InHand(this))
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
        if (Services.actionManager.InHand(this))
        {
            PlayerActionManager.currentDragCard = this;
        }
        
        
    }

    void OnMouseUp()
    {
        if (PlayerActionManager.currentDragCard == this && Services.actionManager.InHand(this))
        {
            if (Draggedout())
            {
                Services.actionManager.PlayCard(this);
            }
        PlayerActionManager.currentDragCard = null;
        }
        
    }

    private bool Draggedout()
    {
        BoxCollider2D a = Services.actionManager.handArea;
        
        if (transform.position.x <= a.transform.position.x + a.bounds.size.x/2 && transform.position.x >= a.transform.position.x - a.bounds.size.x/2
            && transform.position.y <= a.transform.position.y + a.bounds.size.y/2 && transform.position.y >= a.transform.position.y - a.bounds.size.y/2)
        {
            return false;
        }
        return true;
    }
}
