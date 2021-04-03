using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Rendering;

public class CardFunction : MonoBehaviour
{

    protected int instanceId;

    [Header("card basic info")]
    public Card card;
    private float attackCost;
    private float drawCost;
    private Card.CardCompany company;
    public Card.CardCompany getCompany()
    {
        return company;
    }
    private Card.CardType cardType;
    private SpriteRenderer splashArt;
    private TextMeshPro nameText;
    private TextMeshPro AttackCostText;
    private TextMeshPro DrawCostText;
    private TextMeshPro effectDiscriptionText;
    private Material mainCardMat;
    private SpriteRenderer backgroundRenderer;
    private SpriteRenderer highlightborder;
    private SpriteRenderer clickhighlightborder;
    [HideInInspector]public GameObject keywordTextBox;
    private Color mouseOverColor = Color.yellow;
    private Color originalColor = Color.white;
    private List<Card.Keywords> containedKeywords;

    public List<Card.Keywords> getKeywords()
    {
        return containedKeywords;
        
    }

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
        clickhighlightborder.enabled = false;
        keywordTextBox.SetActive(false);
        GetComponent<CardDisplayManager>().UpdateVisual();
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
        mainCardMat = backgroundRenderer.material;
        highlightborder = transform.Find("highlight").GetComponent<SpriteRenderer>();
        clickhighlightborder = transform.Find("click highlight").GetComponent<SpriteRenderer>();
        keywordTextBox = transform.Find("keywordText").gameObject;

        if (card.cardSplashArt != null)
        {
            splashArt.sprite = card.cardSplashArt;

        }
        else
        {
            splashArt.sprite = null;
        }
        containedKeywords = new List<Card.Keywords>();
        nameText.text = card.cardName;
        cardType = card.type;
        gameObject.name = card.cardName;
        attackCost = card.attackBarCost;
        drawCost = card.drawBarCost;
        company = card.Company;
        effectDiscriptionText.text = card.cardEffectDiscription;
        containedKeywords = card.containedKeywords;
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

        canBePlayed = (canBePlayed&&Services.actionManager.PlayerHand.Contains(this)&& Services.resourceManager.CheckDrawBar(drawCost)&&Services.resourceManager.CheckAttackBar(attackCost));


        if (highlightborder != null)
        {
            highlightborder.enabled = canBePlayed;
           // mainCardMat.SetFloat("_OutlineEnabled", canBePlayed ? 1 : 0);
        }
        
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
        Services.eventManager.Fire(new CardTriggerEvent(this));
        AfterTriggered();
    }

    public virtual void JustTriggerEffect()
    {
        triggered.Invoke();
    }
    public class CardTriggerEvent : AGPEvent
    {
        public CardFunction thisCard;
        public CardTriggerEvent(CardFunction card)
        {
            thisCard = card;
        }
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
        if (Services.actionManager.InHand(this) != -1)
        {
            
            BringUpOrderInLayer();
        }
        if (containedKeywords.Count > 0)
        {
            keywordTextBox.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        //backgroundRenderer.material.color = originalColor;
        UpdateInHandLayerOrder();
        if (containedKeywords.Count > 0)
        {
            keywordTextBox.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (Services.actionManager.InHand(this) != -1)
        {
            clickhighlightborder.enabled = true;
            //backgroundRenderer.material.color = mouseOverColor;
            BringUpOrderInLayer();
            PlayerActionManager.currentDragCard = this;
        }
        
        
    }

    void OnMouseUp()
    {
        //backgroundRenderer.material.color = originalColor;
        clickhighlightborder.enabled = false;
        if (PlayerActionManager.currentDragCard == this && Services.actionManager.InHand(this) != -1)
        {
            if (Draggedout())
            {
                
                Services.actionManager.PlayCard(this);
                CanPlay();
            }

            UpdateInHandLayerOrder();
        PlayerActionManager.currentDragCard = null;
        }
        
    }
    public void UpdateInHandLayerOrder()
    {
        GetComponent<SortingGroup>().sortingOrder = Services.actionManager.PlayerHand.IndexOf(this);
        transform.localScale = new Vector3(1,1,1);
    }

    public void BringUpOrderInLayer()
    {
        GetComponent<SortingGroup>().sortingOrder = 11;
        transform.localScale = new Vector3(1.1f, 1.1f, 1);
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
