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
    }

    public virtual void AfterPlayed()
    {

    }

    public virtual void TriggerEffect()
    {
        triggered.Invoke();
    }

    public virtual void AfterTriggered()
    {
        if (used.GetPersistentEventCount() > 0)
        {
            used.Invoke();
        }
        else
        {
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


}
